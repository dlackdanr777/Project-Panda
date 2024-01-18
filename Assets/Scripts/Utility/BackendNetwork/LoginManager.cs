using BackEnd;
using System.Threading.Tasks;
using UnityEngine;
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
        [SerializeField] private Text _loginText;


        void Start()
        {
            _loginText.gameObject.SetActive(false);

            //��ư Ŭ�� �̺�Ʈ �߰�
            _loginButton.onClick.AddListener(Login);
            _signupButton.onClick.AddListener(SignUp);
        }


        /// <summary>�α��� �Լ�</summary>
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


        /// <summary>ȸ������ �Լ�</summary>
        private void SignUp()
        {
            string id = _idInput.text;
            string pw = _passwordInput.text;

            BackendManager.Instance.CustomSignup(id, pw);
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


        /// <summary>�α��� ������ ���� ������ �Ѿ�� �ϴ� �Լ�</summary>
        /// ���� ù �α��� �̸� NewUser��, �ƴϸ� �������� ������ �Ѿ�� �ؾ���
        private void LoadNextScene()
        {
            LoadingSceneManager.LoadScene("NewUserSceneMuksTest");
            BackendManager.Instance.GetMyData("UserInfo", 10, DatabaseManager.Instance.UserInfo.LoadUserInfoData);
            DatabaseManager.Instance.DialogueDatabase.LoadData();
        }
    }
}


