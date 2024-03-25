using BackEnd;
using LitJson;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace Muks.BackEnd
{
    public class LoginManager : MonoBehaviour
    {

        [Tooltip("아이디 입력 필드")]
        [SerializeField] private InputField _idInput;

        [Tooltip("비밀번호 입력 필드")]
        [SerializeField] private InputField _passwordInput;


        [Tooltip("로그인 버튼")]
        [SerializeField] private Button _loginButton;

        [Tooltip("회원가입 버튼")]
        [SerializeField] private Button _signupButton;


        [Tooltip("로그인 성공 텍스트")]
        [SerializeField] private TextMeshProUGUI _loginText;

        [SerializeField] private InputField _input;

        [SerializeField] private Button _hashButton;


        void Start()
        {
            _loginText.gameObject.SetActive(false);

            //버튼 클릭 이벤트 추가
            _loginButton.onClick.AddListener(Login);
            _signupButton.onClick.AddListener(SignUp);

            _hashButton.onClick.AddListener(GetGoogleHash);

            Invoke("GuestLogin", 1f);
        }


        public void GetGoogleHash()
        {
            string googleHashKey = Backend.Utils.GetGoogleHash();

            if (!string.IsNullOrEmpty(googleHashKey))
            {
                Debug.Log(googleHashKey);
                if (_input != null)
                    _input.text = googleHashKey;

            }
        }


        /// <summary>로그인 함수</summary>
        private void Login()
        {
            string id = _idInput.text;
            string pw = _passwordInput.text;

            BackendManager.Instance.CustomLogin(id, pw, 10, (bro) =>
            {
                HideLoginUI();
                LoadData();
                LoadNextScene(bro);

            });
        }


        /// <summary>회원가입 함수</summary>
        private void SignUp()
        {
            string id = _idInput.text;
            string pw = _passwordInput.text;

            BackendManager.Instance.CustomSignup(id, pw);
        }


        private void GuestLogin()
        {
            BackendManager.Instance.GuestLogin(10, (bro) =>
            {
                HideLoginUI();
                LoadData();
                LoadNextScene(bro);
            });
        }


        /// <summary>로그인 성공시 로그인 관련 ui들을 비활성화 시키는 함수</summary>
        private void HideLoginUI()
        {
            _idInput.gameObject.SetActive(false);
            _passwordInput.gameObject.SetActive(false);
            _loginButton.gameObject.SetActive(false);
            _signupButton.gameObject.SetActive(false);

            _loginText.gameObject.SetActive(true);
        }


        //전체 데이터를 서버에서 불러오는 씬
        private void LoadData()
        {
            //LoadingSceneManager.LoadScene("NewUserSceneMuksTest", LoadingType.FirstLoading);
            DatabaseManager.Instance.ItemDatabase.LoadData();
            BackendManager.Instance.GetMyData("UserInfo", 10, DatabaseManager.Instance.UserInfo.LoadUserInfoData);
            BackendManager.Instance.GetMyData("Challenges", 10, DatabaseManager.Instance.UserInfo.LoadChallengesData);
            BackendManager.Instance.GetMyData("Story", 10, DatabaseManager.Instance.UserInfo.LoadStoryData);
            BackendManager.Instance.GetMyData("Attendance", 10, DatabaseManager.Instance.UserInfo.LoadAttendanceData);
            BackendManager.Instance.GetMyData("NPC", 10, DatabaseManager.Instance.UserInfo.LoadNPCData);
            BackendManager.Instance.GetMyData("Bamboo", 10, GameManager.Instance.Player.LoadBambooData);
            BackendManager.Instance.GetMyData("Mail", 10, GameManager.Instance.Player.LoadMailData);
            BackendManager.Instance.GetMyData("Inventory", 10, DatabaseManager.Instance.UserInfo.LoadInventoryData);
            BackendManager.Instance.GetMyData("Book", 10, DatabaseManager.Instance.UserInfo.LoadBookData);
            BackendManager.Instance.GetMyData("Furniture", 10, DatabaseManager.Instance.FurniturePosDatabase.LoadFurnitureData);
            BackendManager.Instance.GetMyData("StarterPandaInfo", 10, DatabaseManager.Instance.StartPandaInfo.LoadPandaInfoData);
            BackendManager.Instance.GetMyData("BambooField", 10, BambooFieldSystem.Instance.LoadBambooFieldData);
            DatabaseManager.Instance.Challenges.LoadData();
            CostumeManager.Instance.LoadData();
            //DatabaseManager.Instance.NPCDatabase.LoadData();
            DatabaseManager.Instance.DialogueDatabase.LoadData();
            DatabaseManager.Instance.RecipeDatabase.LoadData();
            DatabaseManager.Instance.AlbumDatabase.LoadData();
            DatabaseManager.Instance.AttendanceDatabase.LoadData();
            DatabaseManager.Instance.MessageDatabase.LoadData();

            BackendManager.Instance.Login = true;
        }


        /// <summary>로그인 성공시 다음 씬으로 넘어가게 하는 함수</summary>
        /// 차후 첫 로그인 이면 NewUser씬, 아니면 기존유저 씬으로 넘어가게 해야함
        private void LoadNextScene(BackendReturnObject bro)
        {
            Muks.Tween.Tween.TransformMove(gameObject, transform.position, 3, TweenMode.Constant, () =>
            {
                if (!BackendManager.Instance.Login)
                {
                    Debug.LogError("뒤끝 로그인이 되지않아 씬을 로드하지 않습니다.");
                    return;
                }

                //기존 회원인 경우 통합씬으로
                if (DatabaseManager.Instance.UserInfo.IsExistingUser)
                    LoadingSceneManager.LoadScene("24_01_09_Integrated", LoadingType.FirstLoading);

                //신규 회원인 경우 인트로 씬으로 넘어간다.
                else
                    LoadingSceneManager.LoadScene("IntroScene", LoadingType.FirstLoading);
            });
        
        }
    }
}


