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

        [Tooltip("���̵� �Է� �ʵ�")]
        [SerializeField] private InputField _idInput;

        [Tooltip("��й�ȣ �Է� �ʵ�")]
        [SerializeField] private InputField _passwordInput;


        [Tooltip("�α��� ��ư")]
        [SerializeField] private Button _loginButton;

        [Tooltip("ȸ������ ��ư")]
        [SerializeField] private Button _signupButton;


        [Tooltip("�α��� ���� �ؽ�Ʈ")]
        [SerializeField] private TextMeshProUGUI _loginText;

        [SerializeField] private InputField _input;

        [SerializeField] private Button _hashButton;


        void Start()
        {
            _loginText.gameObject.SetActive(false);

            //��ư Ŭ�� �̺�Ʈ �߰�
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


        /// <summary>�α��� �Լ�</summary>
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


        /// <summary>ȸ������ �Լ�</summary>
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


        /// <summary>�α��� ������ �α��� ���� ui���� ��Ȱ��ȭ ��Ű�� �Լ�</summary>
        private void HideLoginUI()
        {
            _idInput.gameObject.SetActive(false);
            _passwordInput.gameObject.SetActive(false);
            _loginButton.gameObject.SetActive(false);
            _signupButton.gameObject.SetActive(false);

            _loginText.gameObject.SetActive(true);
        }


        //��ü �����͸� �������� �ҷ����� ��
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


        /// <summary>�α��� ������ ���� ������ �Ѿ�� �ϴ� �Լ�</summary>
        /// ���� ù �α��� �̸� NewUser��, �ƴϸ� �������� ������ �Ѿ�� �ؾ���
        private void LoadNextScene(BackendReturnObject bro)
        {
            Muks.Tween.Tween.TransformMove(gameObject, transform.position, 3, TweenMode.Constant, () =>
            {
                if (!BackendManager.Instance.Login)
                {
                    Debug.LogError("�ڳ� �α����� �����ʾ� ���� �ε����� �ʽ��ϴ�.");
                    return;
                }

                //���� ȸ���� ��� ���վ�����
                if (DatabaseManager.Instance.UserInfo.IsExistingUser)
                    LoadingSceneManager.LoadScene("24_01_09_Integrated", LoadingType.FirstLoading);

                //�ű� ȸ���� ��� ��Ʈ�� ������ �Ѿ��.
                else
                    LoadingSceneManager.LoadScene("IntroScene", LoadingType.FirstLoading);
            });
        
        }
    }
}


