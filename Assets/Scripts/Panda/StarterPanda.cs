<<<<<<< HEAD
using System.Collections.Generic;
using UnityEngine;
=======
using Muks.DataBind;
using Muks.Tween;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;
>>>>>>> parent of 588028c (Revert "Merge pull request #39 from dlackdanr777/hye")


namespace BT
{
    public class StarterPanda : Panda
    {
<<<<<<< HEAD

        private BehaviorTree _behaviorTree;

        private float _feelingTimer;
=======
        private BehaviorTree _behaviorTree;

        private float _feelingTimer;
        private string StarterStateImage = "StarterStateImage"; //��Ÿ�� �Ǵ� �����̹���ID
>>>>>>> parent of 588028c (Revert "Merge pull request #39 from dlackdanr777/hye")

        private void Awake()
        {
            _behaviorTree = new BehaviorTree(SettingBT());
<<<<<<< HEAD
=======
            _uiPanda.gameObject.SetActive(true);
            _happiness = 9;
            _lastHappiness = _happiness;
            StateHandler?.Invoke(StarterStateImage, 0);
>>>>>>> parent of 588028c (Revert "Merge pull request #39 from dlackdanr777/hye")
        }


        private void OnEnable()
        {
<<<<<<< HEAD
            //3�ʸ��� BT�� �����Ѵ�.
=======
            //3�ʸ��� BT�� ����
>>>>>>> parent of 588028c (Revert "Merge pull request #39 from dlackdanr777/hye")
            InvokeRepeating("StartBT", 1, 1);
        }


        private void OnDisable()
        {
<<<<<<< HEAD
            //�ش� ������Ʈ�� ��������� �ݺ��� �����Ѵ�.
=======
            //�ش� ������Ʈ�� ��������� �ݺ��� ����
>>>>>>> parent of 588028c (Revert "Merge pull request #39 from dlackdanr777/hye")
            CancelInvoke("StartBT");
        }


        private void Update()
        {
<<<<<<< HEAD
            if (_happiness > 100)
            {
                _happiness = 0;
            }

            _happiness += 1;

            _feelingTimer += Time.deltaTime;
        }


=======
            PandaMouseClick();
            ShowStateImage();

            if(_happiness > -10)
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
>>>>>>> parent of 588028c (Revert "Merge pull request #39 from dlackdanr777/hye")
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

<<<<<<< HEAD
            Debug.Log("�ð��� �������� �ʾҽ��ϴ�.");
=======
            //Debug.Log("�ð��� �������� �ʾҽ��ϴ�.");
>>>>>>> parent of 588028c (Revert "Merge pull request #39 from dlackdanr777/hye")
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
<<<<<<< HEAD
            //�ູ���� 90�̻��̸�?
            if (_happiness >= 90)
            {
                //�ſ� �ູ�� ����ǥ�� ���� �ൿ�� �ڵ�� �ۼ��ϸ� �˴ϴ�.
                Debug.Log("��û�� �ູ���� �����ϴ�.");
=======
            //�ູ���� 9�̻��̸�?
            if (_happiness >= 9)
            {
                //�ſ� �ູ�� ����ǥ�� ���� �ൿ�� �ڵ�� �ۼ�
                StateHandler?.Invoke(StarterStateImage, 0);
>>>>>>> parent of 588028c (Revert "Merge pull request #39 from dlackdanr777/hye")

                //Success�ϰ�� �ش� �� ��常 ����
                return INode.ENodeState.Success;
            }

            //Failure�� ��� ���� ������ ���� ��� ����
            return INode.ENodeState.Failure;
        }


        //�ູ���ϴ� ����ǥ��
        private INode.ENodeState Pleased()
        {
<<<<<<< HEAD
            //�ູ���� 70�̻��̸�?
            if (_happiness >= 70)
            {
                //�ູ�� ����ǥ�� ���� �ൿ�� �ڵ�� �ۼ��ϸ� �˴ϴ�.
                Debug.Log("�ູ���� �����ϴ�.");

                //Success�ϰ�� �ش� �� ��常 ����
                return INode.ENodeState.Success;
            }

            //Failure�� ��� ���� ������ ���� ��� ����
=======
            if (_happiness >= 5)
            {
                //�ູ�� ����ǥ�� ���� �ൿ�� �ڵ�� �ۼ�
                StateHandler?.Invoke(StarterStateImage, 1);

                return INode.ENodeState.Success;
            }

>>>>>>> parent of 588028c (Revert "Merge pull request #39 from dlackdanr777/hye")
            return INode.ENodeState.Failure;
        }


        //����� ����ǥ��
        private INode.ENodeState Nomal()
        {
<<<<<<< HEAD
            //�ູ���� 50�̻��̸�?
            if (_happiness >= 50)
            {
                //����� ����ǥ�� ���� �ൿ�� �ڵ�� �ۼ��ϸ� �˴ϴ�.
                Debug.Log("�ƹ��ൿ�� ���� �ʽ��ϴ�.");

                //Success�ϰ�� �ش� �� ��常 ����
                return INode.ENodeState.Success;
            }

            //Failure�� ��� ���� ������ ���� ��� ����
=======
            if (_happiness >= 0)
            {
                //����� ����ǥ�� ���� �ൿ�� �ڵ�� �ۼ�
                StateHandler?.Invoke(StarterStateImage, 2);

                return INode.ENodeState.Success;
            }

>>>>>>> parent of 588028c (Revert "Merge pull request #39 from dlackdanr777/hye")
            return INode.ENodeState.Failure;
        }


        //���� ����ǥ��
        private INode.ENodeState Sad()
        {
<<<<<<< HEAD
            //�ູ���� 30�̻��̸�?
            if (_happiness >= 30)
            {
                //���� ����ǥ�� ���� �ൿ�� �ڵ�� �ۼ��ϸ� �˴ϴ�.
                Debug.Log("������ �����ϴ�.");

                //Success�ϰ�� �ش� �� ��常 ����
                return INode.ENodeState.Success;
            }

            //Failure�� ��� ���� ������ ���� ��� ����
=======
            if (_happiness >= -5)
            {
                //���� ����ǥ�� ���� �ൿ�� �ڵ�� �ۼ�
                StateHandler?.Invoke(StarterStateImage, 3);

                return INode.ENodeState.Success;
            }

>>>>>>> parent of 588028c (Revert "Merge pull request #39 from dlackdanr777/hye")
            return INode.ENodeState.Failure;
        }


        //�ܷο� ����ǥ��
        private INode.ENodeState Lonely()
        {
<<<<<<< HEAD
            //�ູ���� 0�̻��̸�?
            if (_happiness >= 0)
            {
                //���� ����ǥ�� ���� �ൿ�� �ڵ�� �ۼ��ϸ� �˴ϴ�.
                Debug.Log("�ܷο��� �����ϴ�.");

                //Success�ϰ�� �ش� �� ��常 ����
                return INode.ENodeState.Success;
            }

            //Failure�� ��� ���� ������ ���� ��� ����
            return INode.ENodeState.Failure;
        }
    }

=======
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
            if(changeIntimacy > 0)
            {
                //ģ�е� ���
            }
            else
            {
                //ģ�е� �϶�
            }
        }
        protected override void SetPreference(string mbti)
        {
            //mbti ������ ���� �Ǵ� ���� ����
        }
    } 
>>>>>>> parent of 588028c (Revert "Merge pull request #39 from dlackdanr777/hye")
}
