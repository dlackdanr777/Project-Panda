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

        [SerializeField] private InputField _input;

        [SerializeField] private Button _hashButton;


        void Start()
        {
            _loginText.gameObject.SetActive(false);

            //��ư Ŭ�� �̺�Ʈ �߰�
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

    //���ʹϾ� ���� ������ �ø� �� �����Ƿ� �߰��� ������ �� Class List
    private List<SaveStickerData> _saveStickerDataList = new List<SaveStickerData>();

    #region SaveAndLoadSticker

    public void LoadStickerData(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        if (json.Count <= 0)
        {
            Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
            return;
        }

        else
        {
            for (int i = 0, count = json[0]["StickerReceived"].Count; i < count; i++)
            {
                string item = json[0]["StickerReceived"][i].ToString();
                StickerReceived.Add(item);
            }

            _saveStickerDataList.Clear();
            for (int i = 0; i < json[0]["StickerDataArray"].Count; i++)
            {
                SaveStickerData item = JsonUtility.FromJson<SaveStickerData>(json[0]["StickerDataArray"][i].ToJson());
                _saveStickerDataList.Add(item);
                StickerDataArray.Add(item.GetStickerData());
            }

            LoadUserReceivedSticker();
            LoadUserStickerData();
            Debug.Log("Sticker Load����");
        }
    }


    public void SaveStickerData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Sticker";

        if (!Backend.IsLogin)
        {
            Debug.LogError("�ڳ��� �α��� �Ǿ����� �ʽ��ϴ�.");
            return;
        }

        if (maxRepeatCount <= 0)
        {
            Debug.LogErrorFormat("{0} ��Ʈ�� ������ �޾ƿ��� ���߽��ϴ�.", selectedProbabilityFileId);
            return;
        }

        BackendReturnObject bro = Backend.GameData.Get(selectedProbabilityFileId, new Where());

        switch (BackendManager.Instance.ErrorCheck(bro))
        {
            case BackendState.Failure:
                Debug.LogError("�ʱ�ȭ ����");
                break;

            case BackendState.Maintainance:
                Debug.LogError("���� ���� ��");
                break;

            case BackendState.Retry:
                Debug.LogWarning("���� ��õ�");
                SaveInventoryData(maxRepeatCount - 1);
                break;

            case BackendState.Success:

                if (bro.GetReturnValuetoJSON() != null)
                {
                    if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                    {
                        InsertStickerData(selectedProbabilityFileId);
                    }
                    else
                    {
                        UpdateStickerData(selectedProbabilityFileId, bro.GetInDate());
                    }
                }
                else
                {
                    InsertStickerData(selectedProbabilityFileId);
                }

                Debug.LogFormat("{0}������ �����߽��ϴ�..", selectedProbabilityFileId);
                break;
        }
    }


    /// <summary> ���� ��ƼĿ ������ ���� �Լ� </summary>
    public void InsertStickerData(string selectedProbabilityFileId)
    {
        SaveUserStickerData();

        _saveStickerDataList.Clear();
        foreach (StickerData data in StickerDataArray)
        {
            _saveStickerDataList.Add(data.GetSaveStickerData());
        }

        Param param = GetStickerParam();

        Debug.LogFormat("��ƼĿ ������ ������ ��û�մϴ�.");

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> ���� ��ƼĿ ������ ���� �Լ� </summary>
    public void UpdateStickerData(string selectedProbabilityFileId, string inDate)
    {
        SaveUserStickerData();

        _saveStickerDataList.Clear();
        foreach (StickerData data in StickerDataArray)
        {
            _saveStickerDataList.Add(data.GetSaveStickerData());
        }

        Param param = GetStickerParam();

        Debug.LogFormat("��ƼĿ ������ ������ ��û�մϴ�.");

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> ������ ������ ��ƼĿ �����͸� ��� ��ȯ�ϴ� Ŭ���� </summary>
    public Param GetStickerParam()
    {
        Param param = new Param();
        param.Add("StickerReceived", StickerReceived);
        param.Add("StickerDataArray", _saveStickerDataList);
        return param;
    }

    #endregion
}


