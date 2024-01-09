using BT;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour, IInteraction
{
    private bool _isCollection = false; // ä�� ���ΰ�?
    private bool _isExit = false;

    #region ���� ����, �ð�
    // ���� ���� �ð�, ���� ���Ƿ� ����
    [SerializeField] private int _hour; // ���߿� _hour�� ���� ������ �ð� ������ ����
    [SerializeField] private string[] _timeIDs = new string[6]; // ���� ä�� ������ ��� �ð� ID
    [SerializeField] private string _season; // ���߿� _season�� ���� ������ �ð� ������ ����
    [SerializeField] private string[] _seasonIDs = new string[4]; // ���� ä�� ������ ��� ���� ID
    #endregion

    #region ���� ä�� ������ ������ ����Ʈ
    private List<string> normalBugIDList = new List<string>();
    private List<string> rareBugIDList = new List<string>();
    #endregion

    #region ä�� ���� �ð�
    private float _waitTime; // ä�� ��ٸ� �ð�
    private float _spawnTime = 30f; // ä�� ������ �ð� ���� - 10�ʸ��� ä�� ����
    private float _fadeTime = 1f; // ȭ�� ��ο� �ð�
    private float _collectionLatency = 4f; // ä�� �Ϸ� �� ��� ��� ������ ���� �ð�(���� �ð� ���� �ִϸ��̼� ���)
    private float _checkTime = -1;
    #endregion

    /// <summary> ���� ä�� ���� </summary>
    private int fieldIndex = -1;

    #region ��ġ ����
    private Vector3 _targetPos;
    private Vector3 CollectionPosition = new Vector3(-3.4f, -14f, 0);
    #endregion

    private Sprite _pandaImage;
    [SerializeField] private Sprite _pandaCollectionImage;

    private Animator _collectionAnim;
    [SerializeField] private CollectionButton _collectionButton;
    [SerializeField] private GameObject _speechBubble;

    public Action<string> OnCollectionSuccess;
    public Action<string> OnSuccessFrame;
    public Action OnCollectionFail;
    public Action OnExitCollection; // ä�� ȭ�� ����
    public Action<float> OnCollectionButtonClicked;

    // ���߿� ������� �߰��ȴٸ� ����&����� �߰��ϱ�
    private float normal = 0.87f;

    #region ä�� ���� üũ
    private float _successRate = 0.8f;
    private bool _isSuccessCollection;
    public bool IsSuccessCollection 
    {
        get { return _isSuccessCollection; }
        set
        {
            _isSuccessCollection = value;

            // ä�� ����: �ִϸ��̼� ���� + �ؽ�Ʈ �����
            if (_isSuccessCollection)
            {
                CollectionSuccess();
            }
            else
            {
                CollectionFail();
            }
            _isCollection = false;
            _isExit = true;
        }
    }
    #endregion

    private void Start()
    {
        _waitTime = 29;
        //_collectionButton.OnCollectionButtonClicked += ClickCollectionButton;
        OnCollectionButtonClicked += ClickCollectionButton;

    }
    private void OnDestroy()
    {
        //_collectionButton.OnCollectionButtonClicked -= ClickCollectionButton;
        OnCollectionButtonClicked -= ClickCollectionButton;
    }

    private void Update()
    {
        // ä�� ���ð� üũ
        if (_waitTime < _spawnTime && !_isCollection && !_isExit)
        {
            _waitTime += Time.deltaTime;
        }
        else if(_waitTime >= _spawnTime)
        {
            _waitTime = 0;
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<Animator>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
        }

        // ä�� ���̸� ī�޶� ���
        if(_isCollection)
        {
            CameraLock();

            // ä�� �����ð��� �ƴϸ� _time = -1
            if(_checkTime >= 0)
            {
                _checkTime += Time.deltaTime;
                if( _checkTime >= _collectionLatency) // ä�� �����ð��� �������� Ȯ��
                {
                    IsSuccess();
                    _checkTime = -1;
                }
            }
        }
        else if(Input.GetMouseButtonDown(0) && _isExit)
        {
            _isExit = false;
            _collectionAnim.SetBool("IsCollectionLatency", false);
            Invoke("ExitCollection", 0.4f);
            OnExitCollection?.Invoke();
        }
    }

    public void StartInteraction()
    {
        OnCollectionButtonClicked?.Invoke(_fadeTime); // ȭ�� FadeOut

        //// ä�� ���� ������ ����
        //_collectionButton.gameObject.SetActive(true);
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void UpdateInteraction()
    {

    }

    public void ExitInteraction()
    {

    }


    private void ClickCollectionButton(float fadeTime)
    {
        //_fadeTime = fadeTime;
        Invoke("ReadyCollection", _fadeTime);
    }

    private void ReadyCollection()
    {
        StarterPanda starterPanda = DatabaseManager.Instance.StartPandaInfo.StarterPanda;

        _pandaImage = starterPanda.GetComponent<SpriteRenderer>().sprite;
        if(_collectionAnim == null)
        {
            _collectionAnim = starterPanda.GetComponent<Animator>();
        }

        // ĳ���Ͱ� ä�� ����Ʈ�� �̵�
        starterPanda.gameObject.transform.position = CollectionPosition;

        // �Ǵ� ä�� �̹����� ����
        DatabaseManager.Instance.StartPandaInfo.StarterPanda.gameObject.GetComponent<SpriteRenderer>().sprite = _pandaCollectionImage;
        Debug.Log("�Ǵ� �̹��� ���� �� " + DatabaseManager.Instance.StartPandaInfo.StarterPanda.gameObject.GetComponent<SpriteRenderer>().sprite.name);

        // ī�޶� �߽��� ĳ���ͷ� �����ǵ��� �̵�
        _targetPos = new Vector3(starterPanda.transform.position.x, starterPanda.transform.position.y, Camera.main.transform.position.z);
        Camera.main.gameObject.transform.position = _targetPos;

        _isCollection = true;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Animator>().enabled = false;

        // ȭ�� ������ �ð��� ���߾� ä�� ����
        Invoke("StartCollection", _fadeTime);
    }

    /// <summary>
    /// ī�޶� �������� ���ϰ� ���� </summary>
    public void CameraLock()
    {
        Camera.main.gameObject.transform.position = _targetPos;
    }

    /// <summary>
    /// ä�� ������ �� ���� </summary>
    private void StartCollection()
    {
        // ä�� �ִϸ��̼� �Ǵٿ� ��ǳ�� ����
        _collectionAnim.enabled = true;
        _collectionAnim.SetTrigger("IsCollecting");

        _speechBubble.SetActive(true);
        _speechBubble.GetComponent<Animator>().enabled = true;
    }

    /// <summary>
    /// ä�� �Ϸ� �� ��� ������ �� ���� �ð����� ���� </summary>
    public void CollectionLatency()
    {
        _collectionAnim.SetBool("IsCollectionLatency", true); // ä�� ��� ������ �� �ִϸ��̼�
        _checkTime = 0;
    }

    /// <summary>
    /// �������� �������� üũ </summary>
    public void IsSuccess()
    {
        float random = UnityEngine.Random.Range(0, 1f);
        Debug.Log("���� ��: "+ random);
        if (_successRate < random)
        {
            _collectionAnim.SetTrigger("IsCollectionFail");
            IsSuccessCollection = false;
            return;
        }
        _collectionAnim.SetTrigger("IsCollectionSuccess");
        IsSuccessCollection = true;
    }

    /// <summary>
    /// �������� �� ������ �� �������� �� ���� ä�� - �켱 Bug </summary>
    private void CollectionSuccess()
    {
        // ������ �ð��� ����ؼ� �� �� �������� �����ؾ� �� + ���
        // 1. ���� üũ 2. �ð� üũ 3. ��� üũ(����) 4. ������ ����(����)  

        // ���� ������ ���� �ð��� ����Ǿ��ٸ� ä���� �ð� ����
        if (_timeIDs[5] != "GTS" + (21 + _hour)) // ���߿� _hour�� ���� ������ �ð� ������ ����
        {
            // �ð� ����
            SetTime();

            if (_seasonIDs[3] != _season)
            {
                // ���� ����
                SetSeason();
            }

            foreach (Item item in DatabaseManager.Instance.GetBugItemList())
            {
                if (item is GatheringItem gatheringItem)
                {
                    // ���� �ð��� ä�� �����ϴٸ�(1, 2 üũ)
                    if (Array.Exists(_timeIDs, time => time == gatheringItem.Time) && Array.Exists(_seasonIDs, season => season == gatheringItem.Season))
                    {
                        if(item.Rank == "����")
                        {
                            rareBugIDList.Add(item.Id);
                        }
                        else
                        {
                            normalBugIDList.Add(item.Id);
                        }
                    }
                }
            }
        }

        float randomRarity = UnityEngine.Random.Range(0f, 1f); // 3. ��� ����
        int random;
        string collectionID;

        if (randomRarity < normal ) 
        {
            Debug.Log("normal");
            random = UnityEngine.Random.Range(0, normalBugIDList.Count); // 4. ������ ����
            collectionID = normalBugIDList[random];
        }
        else
        {
            random = UnityEngine.Random.Range(0, rareBugIDList.Count); // 4. ������ ����
            collectionID = rareBugIDList[random];
        }

        OnCollectionSuccess?.Invoke(collectionID); // �ִϸ��̼� ���� �� �����ϴ� ������ �����ؾ� ��

        fieldIndex = 0; // ������ 0��

        // �κ��丮�� ������ �̵�
        GameManager.Instance.Player.GatheringItemInventory[fieldIndex].AddById(InventoryItemField.GatheringItem, fieldIndex, collectionID);


        // ���� ������Ʈ

    }

    private void CollectionFail()
    {
        //_collectionAnim.SetBool("IsCollectionEnd", true);
        OnCollectionFail?.Invoke();
    }

    private void ExitCollection()
    {
        _collectionAnim.enabled = false;
        _collectionAnim.gameObject.GetComponent<SpriteRenderer>().sprite = _pandaImage;
    }

    private void SetTime()
    {
        // �ð� ����
        _timeIDs[0] = "GTS00";
        _timeIDs[1] = "GTS0" + (1 + _hour / 12);
        _timeIDs[2] = "GTS0" + (3 + _hour / 6);
        if(7 + _hour / 4 > 10)
        {
            _timeIDs[3] = "GTS" + (7 + _hour / 4);
        }
        else
        {
            _timeIDs[3] = "GTS0" + (7 + _hour / 4);
        }
        _timeIDs[4] = "GTS" + (13 + _hour / 3);
        _timeIDs[5] = "GTS" + (21 + _hour);
    }

    private void SetSeason()
    {
        _seasonIDs[0] = "WAS";

        switch (_season)
        {
            case "WSP":
                _seasonIDs[1] = "WSS";
                _seasonIDs[2] = "WWS";
                break;
            case "WSU":
                _seasonIDs[1] = "WSS";
                _seasonIDs[2] = "WSF";
                break;
            case "WFA":
                _seasonIDs[1] = "WFW";
                _seasonIDs[2] = "WSF";
                break;
            case "WWT":
                _seasonIDs[1] = "WFW";
                _seasonIDs[2] = "WWS";
                break;
        }

        _seasonIDs[3] = _season;
    }
}
