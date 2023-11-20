using System.Collections.Generic;
using UnityEngine;


namespace BT
{
    public class StarterPanda : Panda
    {

        private BehaviorTree _behaviorTree;

        private void Awake()
        {
            _behaviorTree = new BehaviorTree(SettingBT());
        }


        private void OnEnable()
        {
            //3�ʸ��� BT�� �����Ѵ�.
            InvokeRepeating("StartBT", 3, 3);
        }


        private void OnDisable()
        {
            //�ش� ������Ʈ�� ��������� �ݺ��� �����Ѵ�.
            CancelInvoke("StartBT");
        }


        private void Update()
        {
            if (_happiness > 100)
            {
                _happiness = 0;
            }

            _happiness += 1;
        }


        private void StartBT()
        {
            _behaviorTree.Operate();
        }


        //BehaviorTree�� �� INode���� �����Ͽ� ��ȯ�ϴ� �Լ�
        private INode SettingBT()
        {
            List<INode> nodes = new List<INode>()
        {
            FeelingsNode()
        };

            return new SelectorNode(nodes);
        }


        /// <summary> ���� ǥ���� �����ϴ� ��� </summary>
        private INode FeelingsNode()
        {
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
            if (_happiness >= 90)
            {
                //�ſ� �ູ�� ����ǥ�� ���� �ൿ�� �ڵ�� �ۼ��ϸ� �˴ϴ�.
                Debug.Log("��û�� �ູ���� �����ϴ�.");

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
            if (_happiness >= 70)
            {
                //�ູ�� ����ǥ�� ���� �ൿ�� �ڵ�� �ۼ��ϸ� �˴ϴ�.
                Debug.Log("�ູ���� �����ϴ�.");

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
            if (_happiness >= 50)
            {
                //����� ����ǥ�� ���� �ൿ�� �ڵ�� �ۼ��ϸ� �˴ϴ�.
                Debug.Log("�ƹ��ൿ�� ���� �ʽ��ϴ�.");

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
            if (_happiness >= 30)
            {
                //���� ����ǥ�� ���� �ൿ�� �ڵ�� �ۼ��ϸ� �˴ϴ�.
                Debug.Log("������ �����ϴ�.");

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

}
