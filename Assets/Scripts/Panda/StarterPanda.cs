using Muks.DataBind;
using Muks.Tween;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;


namespace BT
{
    public class StarterPanda : Panda
    {

        private BehaviorTree _behaviorTree;

        private float _feelingTimer;

        private void Awake()
        {
            _behaviorTree = new BehaviorTree(SettingBT());
            _uiPanda.gameObject.SetActive(true);
            _happiness = 9;
            _lastHappiness = _happiness;
            StateHandler?.Invoke(0);
        }


        private void OnEnable()
        {
            //3�ʸ��� BT�� �����Ѵ�.
            InvokeRepeating("StartBT", 1, 1);
        }


        private void OnDisable()
        {
            //�ش� ������Ʈ�� ��������� �ݺ��� �����Ѵ�.
            CancelInvoke("StartBT");
        }


        private void Update()
        {
            PandaMouseClick();
            ShowStateImage();
            _happiness -= Time.deltaTime;
            _feelingTimer += Time.deltaTime;
        }

        private void PandaMouseClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
                if (hit.collider == GetComponent<Collider2D>())
                {
                    ToggleUIPandaButton();
                }
            }
        }
        public void ToggleUIPandaButton()
        {
            _isUISetActive = !_isUISetActive;
            //_uiPanda.transform.GetChild(0).gameObject.SetActive(_isUISetActive);
            if( _isUISetActive )
            {
                _uiPanda.transform.GetChild(0).gameObject.SetActive(true);
                UIAlphaHandler?.Invoke(1f, 1f, null);
            }
            else
            {
                UIAlphaHandler?.Invoke(1f, 1f, () => 
                { _uiPanda.transform.GetChild(0).gameObject.SetActive(true);
                });
            }

        }
        public void ShowStateImage()
        {
            Debug.Log("�ູ��"+Mathf.RoundToInt(_happiness));
            Debug.Log("���� �ູ��"+Mathf.RoundToInt(_lastHappiness));

            if (Mathf.RoundToInt(_happiness)!=Mathf.RoundToInt(_lastHappiness))
            {
                Debug.Log("�̹��� true");
                _uiPanda.transform.GetChild(1).gameObject.SetActive(true);
                _lastHappiness = _happiness;
            }
            else
            {
                _uiPanda.transform.GetChild(1).gameObject.SetActive(false);
            }
        }

        #region BT
        private void StartBT()
        {
            _behaviorTree.Operate();
        }


        //BehaviorTree�� �� INode���� �����Ͽ� ��ȯ�ϴ� �Լ�
        private INode SettingBT()
        {
            List<INode> nodes = new List<INode>()
        {
            new ConditionNode(FeelingsNode(), () => FeelingCondition())
        };

            return new SelectorNode(nodes);
        }

        
        private bool FeelingCondition()
        {
            if(_feelingTimer < 10)
            {
                _feelingTimer = 0;
                return true;
            }

            Debug.Log("�ð��� �������� �ʾҽ��ϴ�.");
            return false;
                
        }


        /// <summary> ���� ǥ���� �����ϴ� ��� </summary>
        private INode FeelingsNode()
        {
            Debug.Log("����ǥ�� ����");
            List<INode> nodes = new List<INode>()
        {
            //��带 ������� �Է��Ѵ�.
            new ActionNode(Ecstatic),
            new ActionNode(Pleased),
            new ActionNode(Nomal),
            new ActionNode(Sad),
            new ActionNode(Lonely)
        };

            return new SelectorNode(nodes);
        }


        //�ſ� �ູ���ϴ� ����ǥ�� 
        private INode.ENodeState Ecstatic()
        {
            //�ູ���� 90�̻��̸�?
            if (_happiness >= 9)
            {
                //�ſ� �ູ�� ����ǥ�� ���� �ൿ�� �ڵ�� �ۼ��ϸ� �˴ϴ�.
                Debug.Log("��û�� �ູ���� �����ϴ�.");
                StateHandler?.Invoke(0);

                //Success�ϰ�� �ش� �� ��常 ����
                return INode.ENodeState.Success;
            }

            //Failure�� ��� ���� ������ ���� ��� ����
            return INode.ENodeState.Failure;
        }


        //�ູ���ϴ� ����ǥ��
        private INode.ENodeState Pleased()
        {
            //�ູ���� 70�̻��̸�?
            if (_happiness >= 5)
            {
                //�ູ�� ����ǥ�� ���� �ൿ�� �ڵ�� �ۼ��ϸ� �˴ϴ�.
                Debug.Log("�ູ���� �����ϴ�.");
                StateHandler?.Invoke(1);

                //Success�ϰ�� �ش� �� ��常 ����
                return INode.ENodeState.Success;
            }

            //Failure�� ��� ���� ������ ���� ��� ����
            return INode.ENodeState.Failure;
        }


        //����� ����ǥ��
        private INode.ENodeState Nomal()
        {
            //�ູ���� 50�̻��̸�?
            if (_happiness >= 0)
            {
                //����� ����ǥ�� ���� �ൿ�� �ڵ�� �ۼ��ϸ� �˴ϴ�.
                Debug.Log("�ƹ��ൿ�� ���� �ʽ��ϴ�.");
                StateHandler?.Invoke(2);

                //Success�ϰ�� �ش� �� ��常 ����
                return INode.ENodeState.Success;
            }

            //Failure�� ��� ���� ������ ���� ��� ����
            return INode.ENodeState.Failure;
        }


        //���� ����ǥ��
        private INode.ENodeState Sad()
        {
            //�ູ���� 30�̻��̸�?
            if (_happiness >= -5)
            {
                //���� ����ǥ�� ���� �ൿ�� �ڵ�� �ۼ��ϸ� �˴ϴ�.
                Debug.Log("������ �����ϴ�.");
                StateHandler?.Invoke(3);

                //Success�ϰ�� �ش� �� ��常 ����
                return INode.ENodeState.Success;
            }

            //Failure�� ��� ���� ������ ���� ��� ����
            return INode.ENodeState.Failure;
        }


        //�ܷο� ����ǥ��
        private INode.ENodeState Lonely()
        {
            //�ູ���� 0�̻��̸�?
            if (_happiness >= -10)
            {
                //���� ����ǥ�� ���� �ൿ�� �ڵ�� �ۼ��ϸ� �˴ϴ�.
                Debug.Log("�ܷο��� �����ϴ�.");
                StateHandler?.Invoke(4);

                //Success�ϰ�� �ش� �� ��常 ����
                return INode.ENodeState.Success;
            }

            //Failure�� ��� ���� ������ ���� ��� ����
            return INode.ENodeState.Failure;
        }
        #endregion

        public override void ChangeIntimacy(int changeIntimacy)
        {
            if(changeIntimacy > 0)
            {
                //ģ�е� ���
            }
            else
            {
                //ģ�е� �϶�
            }
        }
        public override void SetPreference(string mbti)
        {
            //mbti ������ ���� �Ǵ� ���� ����
        }
    } 
}
