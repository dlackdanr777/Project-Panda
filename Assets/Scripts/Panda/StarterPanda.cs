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
        private string StarterStateImage = "StarterStateImage"; //스타터 판다 상태이미지ID
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
            //3초마다 BT를 실행한다.
=======
            //3초마다 BT를 실행
>>>>>>> parent of 588028c (Revert "Merge pull request #39 from dlackdanr777/hye")
            InvokeRepeating("StartBT", 1, 1);
        }


        private void OnDisable()
        {
<<<<<<< HEAD
            //해당 오브젝트가 꺼졌을경우 반복을 중지한다.
=======
            //해당 오브젝트가 꺼졌을경우 반복을 중지
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

<<<<<<< HEAD
            Debug.Log("시간이 충족되지 않았습니다.");
=======
            //Debug.Log("시간이 충족되지 않았습니다.");
>>>>>>> parent of 588028c (Revert "Merge pull request #39 from dlackdanr777/hye")
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
<<<<<<< HEAD
            //행복도가 90이상이면?
            if (_happiness >= 90)
            {
                //매우 행복함 감정표현 관련 행동을 코드로 작성하면 됩니다.
                Debug.Log("엄청난 행복함을 느낍니다.");
=======
            //행복도가 9이상이면?
            if (_happiness >= 9)
            {
                //매우 행복함 감정표현 관련 행동을 코드로 작성
                StateHandler?.Invoke(StarterStateImage, 0);
>>>>>>> parent of 588028c (Revert "Merge pull request #39 from dlackdanr777/hye")

                //Success일경우 해당 이 노드만 실행
                return INode.ENodeState.Success;
            }

            //Failure일 경우 같은 높이의 다음 노드 실행
            return INode.ENodeState.Failure;
        }


        //행복해하는 감정표현
        private INode.ENodeState Pleased()
        {
<<<<<<< HEAD
            //행복도가 70이상이면?
            if (_happiness >= 70)
            {
                //행복함 감정표현 관련 행동을 코드로 작성하면 됩니다.
                Debug.Log("행복함을 느낍니다.");

                //Success일경우 해당 이 노드만 실행
                return INode.ENodeState.Success;
            }

            //Failure일 경우 같은 높이의 다음 노드 실행
=======
            if (_happiness >= 5)
            {
                //행복함 감정표현 관련 행동을 코드로 작성
                StateHandler?.Invoke(StarterStateImage, 1);

                return INode.ENodeState.Success;
            }

>>>>>>> parent of 588028c (Revert "Merge pull request #39 from dlackdanr777/hye")
            return INode.ENodeState.Failure;
        }


        //평범함 감정표현
        private INode.ENodeState Nomal()
        {
<<<<<<< HEAD
            //행복도가 50이상이면?
            if (_happiness >= 50)
            {
                //평범함 감정표현 관련 행동을 코드로 작성하면 됩니다.
                Debug.Log("아무행동도 하지 않습니다.");

                //Success일경우 해당 이 노드만 실행
                return INode.ENodeState.Success;
            }

            //Failure일 경우 같은 높이의 다음 노드 실행
=======
            if (_happiness >= 0)
            {
                //평범함 감정표현 관련 행동을 코드로 작성
                StateHandler?.Invoke(StarterStateImage, 2);

                return INode.ENodeState.Success;
            }

>>>>>>> parent of 588028c (Revert "Merge pull request #39 from dlackdanr777/hye")
            return INode.ENodeState.Failure;
        }


        //슬픔 감정표현
        private INode.ENodeState Sad()
        {
<<<<<<< HEAD
            //행복도가 30이상이면?
            if (_happiness >= 30)
            {
                //슬픔 감정표현 관련 행동을 코드로 작성하면 됩니다.
                Debug.Log("슬픔을 느낍니다.");

                //Success일경우 해당 이 노드만 실행
                return INode.ENodeState.Success;
            }

            //Failure일 경우 같은 높이의 다음 노드 실행
=======
            if (_happiness >= -5)
            {
                //슬픔 감정표현 관련 행동을 코드로 작성
                StateHandler?.Invoke(StarterStateImage, 3);

                return INode.ENodeState.Success;
            }

>>>>>>> parent of 588028c (Revert "Merge pull request #39 from dlackdanr777/hye")
            return INode.ENodeState.Failure;
        }


        //외로움 감정표현
        private INode.ENodeState Lonely()
        {
<<<<<<< HEAD
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

=======
            if (_happiness >= -10)
            {
                //슬픔 감정표현 관련 행동을 코드로 작성
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
                //친밀도 상승
            }
            else
            {
                //친밀도 하락
            }
        }
        protected override void SetPreference(string mbti)
        {
            //mbti 정보를 통해 판다 취향 설정
        }
    } 
>>>>>>> parent of 588028c (Revert "Merge pull request #39 from dlackdanr777/hye")
}
