using System.Collections.Generic;
using UnityEngine;


namespace BT
{
    public class StarterPanda : Panda
    {

        private BehaviorTree _behaviorTree;

        private float _feelingTimer;

        private void Awake()
        {
            _behaviorTree = new BehaviorTree(SettingBT());
        }


        private void OnEnable()
        {
            //3초마다 BT를 실행한다.
            InvokeRepeating("StartBT", 1, 1);
        }


        private void OnDisable()
        {
            //해당 오브젝트가 꺼졌을경우 반복을 중지한다.
            CancelInvoke("StartBT");
        }


        private void Update()
        {
            if (_happiness > 100)
            {
                _happiness = 0;
            }

            _happiness += 1;

            _feelingTimer += Time.deltaTime;
        }


        private void StartBT()
        {
            _behaviorTree.Operate();
        }


        //BehaviorTree에 들어갈 INode들을 설정하여 반환하는 함수
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

            Debug.Log("시간이 충족되지 않았습니다.");
            return false;
                
        }


        /// <summary> 감정 표현을 실행하는 노드 </summary>
        private INode FeelingsNode()
        {
            Debug.Log("감정표현 실행");
            List<INode> nodes = new List<INode>()
        {
            //노드를 순서대로 입력한다.
            new ActionNode(Ecstatic),
            new ActionNode(Pleased),
            new ActionNode(Nomal),
            new ActionNode(Sad),
            new ActionNode(Lonely)
        };

            return new SelectorNode(nodes);
        }


        //매우 행복해하는 감정표현 
        private INode.ENodeState Ecstatic()
        {
            //행복도가 90이상이면?
            if (_happiness >= 90)
            {
                //매우 행복함 감정표현 관련 행동을 코드로 작성하면 됩니다.
                Debug.Log("엄청난 행복함을 느낍니다.");

                //Success일경우 해당 이 노드만 실행
                return INode.ENodeState.Success;
            }

            //Failure일 경우 같은 높이의 다음 노드 실행
            return INode.ENodeState.Failure;
        }


        //행복해하는 감정표현
        private INode.ENodeState Pleased()
        {
            //행복도가 70이상이면?
            if (_happiness >= 70)
            {
                //행복함 감정표현 관련 행동을 코드로 작성하면 됩니다.
                Debug.Log("행복함을 느낍니다.");

                //Success일경우 해당 이 노드만 실행
                return INode.ENodeState.Success;
            }

            //Failure일 경우 같은 높이의 다음 노드 실행
            return INode.ENodeState.Failure;
        }


        //평범함 감정표현
        private INode.ENodeState Nomal()
        {
            //행복도가 50이상이면?
            if (_happiness >= 50)
            {
                //평범함 감정표현 관련 행동을 코드로 작성하면 됩니다.
                Debug.Log("아무행동도 하지 않습니다.");

                //Success일경우 해당 이 노드만 실행
                return INode.ENodeState.Success;
            }

            //Failure일 경우 같은 높이의 다음 노드 실행
            return INode.ENodeState.Failure;
        }


        //슬픔 감정표현
        private INode.ENodeState Sad()
        {
            //행복도가 30이상이면?
            if (_happiness >= 30)
            {
                //슬픔 감정표현 관련 행동을 코드로 작성하면 됩니다.
                Debug.Log("슬픔을 느낍니다.");

                //Success일경우 해당 이 노드만 실행
                return INode.ENodeState.Success;
            }

            //Failure일 경우 같은 높이의 다음 노드 실행
            return INode.ENodeState.Failure;
        }


        //외로움 감정표현
        private INode.ENodeState Lonely()
        {
            //행복도가 0이상이면?
            if (_happiness >= 0)
            {
                //슬픔 감정표현 관련 행동을 코드로 작성하면 됩니다.
                Debug.Log("외로움을 느낍니다.");

                //Success일경우 해당 이 노드만 실행
                return INode.ENodeState.Success;
            }

            //Failure일 경우 같은 높이의 다음 노드 실행
            return INode.ENodeState.Failure;
        }
    }

}
