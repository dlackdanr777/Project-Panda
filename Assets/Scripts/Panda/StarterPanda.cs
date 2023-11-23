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
        private string StarterStateImage = "StarterStateImage"; //스타터 판다 상태이미지ID

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
            Debug.Log("성향: 아이템" + _preference._favoriteToy + "성향: 간식"+ _preference._favoriteSnack);
        }


        private void OnEnable()
        {
            //3초마다 BT를 실행
            InvokeRepeating("StartBT", 1, 1);
        }


        private void OnDisable()
        {
            //해당 오브젝트가 꺼졌을경우 반복을 중지
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
            if (_feelingTimer < 10)
            {
                _feelingTimer = 0;
                return true;
            }

            //Debug.Log("시간이 충족되지 않았습니다.");
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
            //행복도가 9이상이면?
            if (_happiness >= 9)
            {
                //매우 행복함 감정표현 관련 행동을 코드로 작성
                StateHandler?.Invoke(StarterStateImage, 0);

                //Success일경우 해당 이 노드만 실행
                return INode.ENodeState.Success;
            }

            //Failure일 경우 같은 높이의 다음 노드 실행
            return INode.ENodeState.Failure;
        }


        //행복해하는 감정표현
        private INode.ENodeState Pleased()
        {
            if (_happiness >= 5)
            {
                //행복함 감정표현 관련 행동을 코드로 작성
                StateHandler?.Invoke(StarterStateImage, 1);

                return INode.ENodeState.Success;
            }

            return INode.ENodeState.Failure;
        }


        //평범함 감정표현
        private INode.ENodeState Nomal()
        {
            if (_happiness >= 0)
            {
                //평범함 감정표현 관련 행동을 코드로 작성
                StateHandler?.Invoke(StarterStateImage, 2);

                return INode.ENodeState.Success;
            }

            return INode.ENodeState.Failure;
        }


        //슬픔 감정표현
        private INode.ENodeState Sad()
        {
            if (_happiness >= -5)
            {
                //슬픔 감정표현 관련 행동을 코드로 작성
                StateHandler?.Invoke(StarterStateImage, 3);

                return INode.ENodeState.Success;
            }

            return INode.ENodeState.Failure;
        }


        //외로움 감정표현
        private INode.ENodeState Lonely()
        {
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
            if (changeIntimacy > 0)
            {
                //친밀도 상승
            }
            else
            {
                //친밀도 하락
            }
        }
        // mbti에서 설정..?
        protected override void SetPreference(string mbti)
        {
            //mbti 정보를 통해 판다 취향 설정
            //_preference = new Preference();
        }

    }
}
