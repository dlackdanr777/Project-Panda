using System.Collections.Generic;
using UnityEngine;


namespace BT
{
    public class StarterPanda : Panda
    {
        private BehaviorTree _behaviorTree;

        private float _feelingTimer;
        private string StarterStateImage = "StarterStateImage"; //��Ÿ�� �Ǵ� �����̹���ID

        private void Awake()
        {
            // ���߿� �ε� �� ����� ����
            var obj = FindObjectsOfType<StarterPanda>();
            if (obj.Length == 1)
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            // �Ǵ� ����
            _pandaID = 0;
            PandaData pandaData = DatabaseManager.Instance.GetPandaData(_pandaID);
            //��Ÿ�� �Ǵ� mbti�� �Ǵ� �����Ϳ� ����
            DatabaseManager.Instance.SetStarterMBTI(Mbti);
            SetPandaData(pandaData);

            _behaviorTree = new BehaviorTree(SettingBT());

            SetUIPanda();
            StateHandler?.Invoke(StarterStateImage, 0); //�Ǵ��� ó�� ���� �̹��� ����

            _preference = DatabaseManager.Instance.SetPreference(Mbti);

            //test �� �����Ǿ����� Ȯ�� - ���߿� �����
            Debug.Log("�Ǵ�ID: " + _pandaID + "�Ǵ� �̸�: " + _pandaName + "�Ǵ� �ູ��: " + _happiness);
            Debug.Log("����: ������" + _preference._favoriteToy + "����: ����"+ _preference._favoriteSnack);
            Debug.Log("�Ǵ� �̹���" + _pandaImage.name);
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
            GiveAGift();


            // �Ǵ� �ູ�� ���������� ����
            if (_happiness > -10)
            {
                _happiness -= Time.deltaTime * 0.001f;
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
                StateHandler?.Invoke(StarterStateImage, 2);
                ChangeIntimacy(-0.001f);

                return INode.ENodeState.Success;
            }

            return INode.ENodeState.Failure;
        }


        //���� ����ǥ��
        private INode.ENodeState Sad()
        {
            if (_happiness >= -5)
            {
                StateHandler?.Invoke(StarterStateImage, 3);
                ChangeIntimacy(-0.002f);

                return INode.ENodeState.Success;
            }

            return INode.ENodeState.Failure;
        }


        //�ܷο� ����ǥ��
        private INode.ENodeState Lonely()
        {
            if (_happiness >= -10)
            {
                StateHandler?.Invoke(StarterStateImage, 4);
                ChangeIntimacy(-0.003f);

                return INode.ENodeState.Success;
            }

            return INode.ENodeState.Failure;
        }
        #endregion

        public override void ChangeIntimacy(float changeIntimacy)
        {
            if (changeIntimacy > 0)
            {
                _intimacy += changeIntimacy;
            }
            else
            {
                _intimacy += changeIntimacy;
            }
            //�Ǵ� �����͵� ����
            DatabaseManager.Instance.UpdatePandaIntimacy(_pandaID, _intimacy);
        }

        public override void ChangeHappiness(float changeHappiness)
        {
            _happiness += changeHappiness;
            DatabaseManager.Instance.UpdatePandaHappiness(_pandaID, _happiness);
        }

    }
}
