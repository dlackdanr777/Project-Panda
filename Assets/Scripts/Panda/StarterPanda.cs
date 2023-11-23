using Muks.DataBind;
using Muks.Tween;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;


namespace BT
{
    public class StarterPanda : Panda
    {
        private BehaviorTree _behaviorTree;

        private float _feelingTimer;
        private string StarterStateImage = "StarterStateImage"; //��Ÿ�� �Ǵ� �����̹���ID

        //public StarterPanda(string mbti)
        //{
        //    _mbtiData = mbti;
        //    _happiness = 9;
        //    _lastHappiness = _happiness;
        //    SetPreference(_mbtiData);
        //}


        protected override void Awake()
        {
            base.Awake();

            _happiness = 9;
            _lastHappiness = _happiness;

            _behaviorTree = new BehaviorTree(SettingBT());
            _uiPanda.gameObject.SetActive(true);
            StateHandler?.Invoke(StarterStateImage, 0);

            _preference = MbtiData.SetPreference(_mbti);
            Debug.Log("����: ������" + _preference._favoriteToy + "����: ����"+ _preference._favoriteSnack);
        }


        private void OnEnable()
        {
            //3�ʸ��� BT�� ����
            InvokeRepeating("StartBT", 1, 1);
        }


        private void OnDisable()
        {
            //�ش� ������Ʈ�� ��������� �ݺ��� ����
            CancelInvoke("StartBT");
        }


        private void Update()
        {
            PandaMouseClick();
            ShowStateImage();

            if (_happiness > -10)
            {
                _happiness -= Time.deltaTime * 0.1f;
            }
            else
            {
                _happiness = -10;
            }
            _feelingTimer += Time.deltaTime;
            _stateImageTimer += Time.deltaTime;
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
            if (_feelingTimer < 10)
            {
                _feelingTimer = 0;
                return true;
            }

            //Debug.Log("�ð��� �������� �ʾҽ��ϴ�.");
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
            //�ູ���� 9�̻��̸�?
            if (_happiness >= 9)
            {
                //�ſ� �ູ�� ����ǥ�� ���� �ൿ�� �ڵ�� �ۼ�
                StateHandler?.Invoke(StarterStateImage, 0);

                //Success�ϰ�� �ش� �� ��常 ����
                return INode.ENodeState.Success;
            }

            //Failure�� ��� ���� ������ ���� ��� ����
            return INode.ENodeState.Failure;
        }


        //�ູ���ϴ� ����ǥ��
        private INode.ENodeState Pleased()
        {
            if (_happiness >= 5)
            {
                //�ູ�� ����ǥ�� ���� �ൿ�� �ڵ�� �ۼ�
                StateHandler?.Invoke(StarterStateImage, 1);

                return INode.ENodeState.Success;
            }

            return INode.ENodeState.Failure;
        }


        //����� ����ǥ��
        private INode.ENodeState Nomal()
        {
            if (_happiness >= 0)
            {
                //����� ����ǥ�� ���� �ൿ�� �ڵ�� �ۼ�
                StateHandler?.Invoke(StarterStateImage, 2);

                return INode.ENodeState.Success;
            }

            return INode.ENodeState.Failure;
        }


        //���� ����ǥ��
        private INode.ENodeState Sad()
        {
            if (_happiness >= -5)
            {
                //���� ����ǥ�� ���� �ൿ�� �ڵ�� �ۼ�
                StateHandler?.Invoke(StarterStateImage, 3);

                return INode.ENodeState.Success;
            }

            return INode.ENodeState.Failure;
        }


        //�ܷο� ����ǥ��
        private INode.ENodeState Lonely()
        {
            if (_happiness >= -10)
            {
                //���� ����ǥ�� ���� �ൿ�� �ڵ�� �ۼ�
                StateHandler?.Invoke(StarterStateImage, 4);

                return INode.ENodeState.Success;
            }

            return INode.ENodeState.Failure;
        }
        #endregion

        protected override void ChangeIntimacy(int changeIntimacy)
        {
            if (changeIntimacy > 0)
            {
                //ģ�е� ���
            }
            else
            {
                //ģ�е� �϶�
            }
        }
        // mbti���� ����..?
        protected override void SetPreference(string mbti)
        {
            //mbti ������ ���� �Ǵ� ���� ����
            //_preference = new Preference();
        }

    }
}
