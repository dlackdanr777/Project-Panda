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
    private Animator _collectionAnim;
    [SerializeField] private CollectionButton _collectionButton;
    [SerializeField] private GameObject _speechBubble;

    public Action OnCollectionSuccess;
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
            // ä�� ����
            if (_isSuccessCollection)
            {
                Debug.Log("ä�� ����");
                OnCollectionSuccess?.Invoke();
            }
            else
            {
                Debug.Log("ä�� ����");
                OnCollectionFail?.Invoke();
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
        OnCollectionSuccess += CollectionSuccess;
        OnCollectionFail += CollectionFail;

    }
    private void OnDestroy()
    {
        _collectionButton.OnCollectionButtonClicked -= ClickCollectionButton;
        OnCollectionSuccess -= CollectionSuccess;
        OnCollectionFail -= CollectionFail;
    }

    private void Update()
    {
        if (_time < _spawnTime)
        {
            _time += Time.deltaTime;
        }
        else
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
            OnExitCollection?.Invoke();
            ExitCollection();
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
        Debug.Log("ä�������Դϴ� ~~");

        // ä�� �ִϸ��̼� �Ǵٿ� ��ǳ�� ����
        _collectionAnim.enabled = true;
        _collectionAnim.SetTrigger("IsCollecting");
        _speechBubble.SetActive(true);
        _speechBubble.GetComponent<Animator>().enabled = true;

        // ��ǳ�� ...�� ������ ����ǥ ǥ��
        // ����ǥ ��ġ�ϸ� ä�� ���� ���� ���� �˷���
        // �ִϸ��̼� ���� + �ؽ�Ʈ �����
        // �κ��丮�� �������� ���� ����

        // ä�� ����
        //IsCollection = false;
    }

    /// <summary>
    /// �������� �������� üũ </summary>
    public void IsSuccess()
    {
        Debug.Log("�������� Ȯ��");
        float random = UnityEngine.Random.Range(0, 1);
        if (_successRate < random)
        {
            IsSuccessCollection = false;
        }
        IsSuccessCollection = true;
    }


    private void CollectionSuccess()
    {
        _collectionAnim.SetBool("IsCollectionEnd", true); // ä�� �Ϸ� �� ����
        // �κ��丮�� ������ �̵�
        // ���� ������Ʈ
    }

    private void CollectionFail()
    {
        _collectionAnim.SetBool("IsCollectionEnd", true);
    }

    private void ExitCollection()
    {
        _collectionAnim.SetBool("IsCollectionEnd", false);

        _collectionAnim.enabled = false; // ���ܼ� �׳� �Ǵ� �ִϸ��̼� ����� �� ���� üũ�� ���� enable = false �ϴ°� ���� �� ����..
        _collectionAnim.gameObject.GetComponent<SpriteRenderer>().sprite = _pandaImage;
    }

}
