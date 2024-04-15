using System;
using System.Collections.Generic;
using UnityEngine;


namespace BT
{
    public class StarterPanda : Panda
    {
        public static StarterPanda Instance;

        public bool IsSwitchingScene;
        private BehaviorTree _behaviorTree;

        private float _feelingTimer;
        private string StarterStateImage = "StarterStateImage"; //스타터 판다 상태이미지ID

        //[Tooltip("애니메이션 개수")]
        //[SerializeField] private int _animationCount = 1;

        [Tooltip("판다 크기 1.5배로 키울 맵 ID")]
        [SerializeField] private List<string> _MediumMapID = new List<string>();
        [Tooltip("판다 크기 2배로 키울 맵 ID")]
        [SerializeField] private List<string> _largeMapID = new List<string>();
        private Vector3 _pandaScale;
        private Vector3 _pandaMediumScale;
        private Vector3 _pandaLargeScale;


        private string _map;
        private Animator _animator;
        public int Num;
        //private float _time;

        //[System.Serializable]
        //public struct MapAnimationData
        //{
        //    public string key;
        //    public int[] values;
        //    public Transform PandaTransform;
        //}

        //public List<MapAnimationData> _mapAnimationDic;
        private bool _isConversation; // 대화 중인지 확인


        private void Awake()
        {
            // 나중에 로딩 씬 만들면 삭제
            var obj = FindObjectsOfType<StarterPanda>();
            if (obj.Length == 1)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            // 판다 세팅
            _pandaID = 0;
            PandaData pandaData = DatabaseManager.Instance.GetPandaData(_pandaID);

            //스타터 판다 mbti를 판다 데이터에 저장
            DatabaseManager.Instance.SetStarterMBTI(DatabaseManager.Instance.StartPandaInfo.Mbti);
            
            if (DatabaseManager.Instance.UserInfo.IsExistingUser)
            {
                DatabaseManager.Instance.UpdatePandaIntimacy(_pandaID, DatabaseManager.Instance.StartPandaInfo.Intimacy);
                DatabaseManager.Instance.UpdatePandaHappiness(_pandaID, DatabaseManager.Instance.StartPandaInfo.Happiness);
                if(DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID != "")
                {
                    CostumeManager.Instance.CostumeDic[DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID].CostumeSlot.SetActive(true);
                }
            }

            SetPandaData(pandaData);


            _behaviorTree = new BehaviorTree(SettingBT());

            StateHandler?.Invoke(StarterStateImage, 0); //판다의 처음 상태 이미지 설정


            //_time = 0f;
            _map = TimeManager.Instance.CurrentMap;
            _isConversation = false;
            _animator = GetComponent<Animator>();
            //if (_animationCount > 1)
            //{
            //    ChangeAnimation();
            //}

            _pandaScale = gameObject.transform.localScale;
            _pandaMediumScale = gameObject.transform.localScale * 1.5f;
            _pandaLargeScale = gameObject.transform.localScale * 2f;
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
            // 씬 전환되면 1초 뒤 실행
            if (IsSwitchingScene)
            {
                IsSwitchingScene = false;
                Invoke("SwitchingScene", 0.5f);
            }

            if(_uiPanda != null)
            {
                //PandaMouseClick();
                //ShowStateImage();
                //GiveAGift();
            }

            //_time += Time.deltaTime;
            //if (_isConversation) // 대화 중인 경우 애니메이션 중지
            //{
            //    StopAnimation();
            //}
            //else if (_animationCount > 1) // 애니메이션이 여러 개인 경우 랜덤 변경
            //{
            //    _animator.speed = 1f;
            //    if (_map != TimeManager.Instance.CurrentMap)
            //    {
            //        _map = TimeManager.Instance.CurrentMap;
            //        ChangeAnimation();
            //    }
            //}

            if (_map != TimeManager.Instance.CurrentMap)
            {
                _map = TimeManager.Instance.CurrentMap;
                SetPandaSize();
            }

            // 판다 행복도 지속적으로 감소
            ChangeHappiness(-Time.deltaTime * 0.001f);

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
                StateHandler?.Invoke(StarterStateImage, 2);
                ChangeIntimacy(-0.001f);

                return INode.ENodeState.Success;
            }

            return INode.ENodeState.Failure;
        }


        //슬픔 감정표현
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


        //외로움 감정표현
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
            //판다 데이터도 변경
            DatabaseManager.Instance.UpdatePandaIntimacy(_pandaID, _intimacy);
            DatabaseManager.Instance.StartPandaInfo.Intimacy = _intimacy;
        }

        public override void ChangeHappiness(float changeHappiness)
        {
            _happiness += changeHappiness;

            DatabaseManager.Instance.UpdatePandaHappiness(_pandaID, _happiness);
            DatabaseManager.Instance.StartPandaInfo.Happiness = _happiness;
        }

        /// <summary>
        /// Scene 전환될 때 실행 </summary>
        public void SwitchingScene()
        {
            SetUIPanda();
            StateHandler?.Invoke(StarterStateImage, 0); //판다의 처음 상태 이미지 설정
        }

        /// <summary>
        /// UI가 존재하면 true 반환 </summary>
        public bool SetFalseUI()
        {
            if(_uiPanda != null)
            {
                _uiPanda.gameObject.SetActive(false);
                return true;
            }
            else
            {
                return false;
            }
        }

        //private void ChangeAnimation()
        //{
        //    _animator.speed = 1f;
        //    _animator.Play("Idle");

        //    // 현재 맵의 애니메이션 번호 찾기
        //    int[] nums = _mapAnimationDic.Find(x => x.key == _map).values;

        //    // 애니메이션 중 랜덤 선택
        //    Num = UnityEngine.Random.Range(0, nums.Length);

        //    // 애니메이션에 맞는 위치 설정
        //    //SetPosition();

        //    _animator.SetInteger("Num", nums[Num]); // 현재 맵의 _num번째 애니메이션 실행
        //}

        private void StopAnimation()
        {
            _animator.speed = 0f;
        }

        //private void SetPosition()
        //{
        //    gameObject.transform.position = _mapAnimationDic.Find(x => x.key == _map).PandaTransform.position;
        //}

        private void SetPandaSize()
        {
            if (_largeMapID.Contains(_map))
            {
                gameObject.transform.localScale = _pandaLargeScale;
            }
            else if (_MediumMapID.Contains(_map))
            {
                gameObject.transform.localScale = _pandaMediumScale;
            }
            else
            {
                gameObject.transform.localScale = _pandaScale;
            }
        }
    }
}
