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
            Debug.Log("행복도"+Mathf.RoundToInt(_happiness));
            Debug.Log("이전 행복도"+Mathf.RoundToInt(_lastHappiness));

            if (Mathf.RoundToInt(_happiness)!=Mathf.RoundToInt(_lastHappiness))
            {
                Debug.Log("이미지 true");
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
            if (_happiness >= 9)
            {
                //매우 행복함 감정표현 관련 행동을 코드로 작성하면 됩니다.
                Debug.Log("엄청난 행복함을 느낍니다.");
                StateHandler?.Invoke(0);

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
            if (_happiness >= 5)
            {
                //행복함 감정표현 관련 행동을 코드로 작성하면 됩니다.
                Debug.Log("행복함을 느낍니다.");
                StateHandler?.Invoke(1);

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
            if (_happiness >= 0)
            {
                //평범함 감정표현 관련 행동을 코드로 작성하면 됩니다.
                Debug.Log("아무행동도 하지 않습니다.");
                StateHandler?.Invoke(2);

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
            if (_happiness >= -5)
            {
                //슬픔 감정표현 관련 행동을 코드로 작성하면 됩니다.
                Debug.Log("슬픔을 느낍니다.");
                StateHandler?.Invoke(3);

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
            if (_happiness >= -10)
            {
                //슬픔 감정표현 관련 행동을 코드로 작성하면 됩니다.
                Debug.Log("외로움을 느낍니다.");
                StateHandler?.Invoke(4);

                //Success일경우 해당 이 노드만 실행
                return INode.ENodeState.Success;
            }

            //Failure일 경우 같은 높이의 다음 노드 실행
            return INode.ENodeState.Failure;
        }
        #endregion

        public override void ChangeIntimacy(int changeIntimacy)
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
        public override void SetPreference(string mbti)
        {
            //mbti 정보를 통해 판다 취향 설정
        }
    } 
}
