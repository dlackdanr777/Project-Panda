using BT;
using System;
using UnityEngine;

public class Collection : MonoBehaviour, IInteraction
{
    private bool _isCollection = false; // ä�� ���ΰ�?
    private bool _isExit = false;

    private float _time;
    private float _spawnTime = 30f; // ä�� ������ �ð� ���� - 10�ʸ��� ä�� ����
    private float _fadeTime; // ȭ�� ��ο� �ð�

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
    public Action OnCollectionFail;
    public Action OnExitCollection; // ä�� ȭ�� ����

    #region ä�� ���� üũ
    private float _successRate = 0.7f;
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
                Debug.Log("ä�� ����");
                CollectionSuccess();
            }
            else
            {
                Debug.Log("ä�� ����");
                CollectionFail();
            }
            _isCollection = false;
            _isExit = true;
        }
    }
    #endregion

    private void Start()
    {
        _time = 29;
        _collectionButton.OnCollectionButtonClicked += ClickCollectionButton;

    }
    private void OnDestroy()
    {
        _collectionButton.OnCollectionButtonClicked -= ClickCollectionButton;
    }

    private void Update()
    {
        if (_time < _spawnTime && !_isCollection)
        {
            _time += Time.deltaTime;
        }
        else if(_time >= _spawnTime)
        {
            _time = 0;
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
        }

        // ä�� ���̸� ī�޶� ���
        if(_isCollection)
        {
            CameraLock();
        }
        else if(Input.GetMouseButtonDown(0) && _isExit)
        {
            _isExit = false;
            _collectionAnim.SetBool("IsCollectionEnd", false);
            Invoke("ExitCollection", 0.4f);
            OnExitCollection?.Invoke();
        }
    }

    public void StartInteraction()
    {
        // ä�� ���� ������ ����
        _collectionButton.gameObject.SetActive(true);
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
        _fadeTime = fadeTime;
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

        // ī�޶� ĳ���Ͱ� �߾����� �����ǵ��� �̵�
        _targetPos = new Vector3(starterPanda.transform.position.x, starterPanda.transform.position.y, Camera.main.transform.position.z);
        Camera.main.gameObject.transform.position = _targetPos;

        _isCollection = true;
        GetComponent<SpriteRenderer>().enabled = false;

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
    /// �������� �������� üũ </summary>
    public void IsSuccess()
    {
        Debug.Log("�������� Ȯ��");
        float random = UnityEngine.Random.Range(0, 1f);
        Debug.Log("���� ��: "+ random);
        if (_successRate < random)
        {
            IsSuccessCollection = false;
            return;
        }
        IsSuccessCollection = true;
    }


    private void CollectionSuccess()
    {
        // �������� �� ������ �� �������� �� ���� ä�� - �켱 snack���� �ϱ�
        int random = UnityEngine.Random.Range(0, DatabaseManager.Instance.ItemDatabase.ItemCount[1]);
        Debug.Log("����: " + random);
        string collectionID = DatabaseManager.Instance.ItemDatabase.ItemList[1][random].Id;

        OnCollectionSuccess?.Invoke(collectionID);
        _collectionAnim.SetBool("IsCollectionEnd", true); // ä�� �Ϸ� �� ����
        
        // �κ��丮�� ������ �̵�
        // ���� ������Ʈ
    }

    private void CollectionFail()
    {
        OnCollectionFail?.Invoke();
        _collectionAnim.SetBool("IsCollectionEnd", true);
    }

    private void ExitCollection()
    {
        _collectionAnim.enabled = false; // ���ܼ� �׳� �Ǵ� �ִϸ��̼� ����� �� ���� üũ�� ���� enable = false �ϴ°� ���� �� ����..
        _collectionAnim.gameObject.GetComponent<SpriteRenderer>().sprite = _pandaImage;
    }

}
