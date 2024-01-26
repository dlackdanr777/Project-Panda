using BackEnd;
using LitJson;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
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
        [SerializeField] private Text _loginText;

        [SerializeField] private InputField _input;

        [SerializeField] private Button _hashButton;


        void Start()
        {
            _loginText.gameObject.SetActive(false);

            //버튼 클릭 이벤트 추가
            _loginButton.onClick.AddListener(Login);
            _signupButton.onClick.AddListener(SignUp);

            _hashButton.onClick.AddListener(GetGoogleHash);
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
                LoadNextScene();
            });


        }


        /// <summary>회원가입 함수</summary>
        private void SignUp()
        {
            string id = _idInput.text;
            string pw = _passwordInput.text;

            BackendManager.Instance.CustomSignup(id, pw);
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


        /// <summary>로그인 성공시 다음 씬으로 넘어가게 하는 함수</summary>
        /// 차후 첫 로그인 이면 NewUser씬, 아니면 기존유저 씬으로 넘어가게 해야함
        private void LoadNextScene()
        {
            LoadingSceneManager.LoadScene("NewUserSceneMuksTest");
            DatabaseManager.Instance.ItemDatabase.LoadData();
            BackendManager.Instance.GetMyData("UserInfo", 10, DatabaseManager.Instance.UserInfo.LoadUserInfoData);
            BackendManager.Instance.GetMyData("Inventory", 10, DatabaseManager.Instance.UserInfo.LoadInventoryData);
            BackendManager.Instance.GetMyData("Sticker", 10, DatabaseManager.Instance.UserInfo.LoadStickerData);
            BackendManager.Instance.GetMyData("Furniture", 10, DatabaseManager.Instance.FurniturePosDatabase.LoadFurnitureData);
            BackendManager.Instance.GetMyData("StarterPandaInfo", 10, DatabaseManager.Instance.StartPandaInfo.LoadPandaInfoData);
            CostumeManager.Instance.LoadData();
            DatabaseManager.Instance.NPCDatabase.LoadData();
            DatabaseManager.Instance.DialogueDatabase.LoadData();
            DatabaseManager.Instance.RecipeDatabase.LoadData();
            DatabaseManager.Instance.AlbumDatabase.LoadData();
        }
    }

}


