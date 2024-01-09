using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>NPC �Ǵ�, ��Ÿ�� �Ǵ��� �θ� Ŭ����</summary>
public abstract class Panda : MonoBehaviour, IInteraction
{
    //���� �̹��� ���� �׼�
    public Action<string, int> StateHandler;
    public Action<float, float, Action> UIAlphaHandler;
    public Action<GameObject, float, float, Action> ImageAlphaHandler;
    public Action GiftHandler;

    /// <summary>�Ǵ� Mbti</summary>
    public string Mbti;
    public bool IsCameraRequest;

    protected bool _isUISetActive;
    protected bool _isGift;
    protected float _stateImageTimer = 1f;

    protected int _pandaID;
    protected string _pandaName;
    protected Sprite _pandaImage;
    //protected Preference _preference;

    protected GameObject _uiPandaParent;
    [SerializeField] protected UIPanda _uiPandaPrefab;
    protected UIPanda _uiPanda;

    /// <summary>�Ǵ� ģ�е�</summary>
    [SerializeField]
    [Range(0, 100)] protected float _intimacy;
    public float Intimacy
    {
        get { return _intimacy; }
        private set { }
    }

    /// <summary>�Ǵ� �ູ��</summary>
    [SerializeField]
    [Range(-10, 10)] protected float _happiness;
    public float Happiness
    {
        get { return _happiness; }
        private set { }
    }
    /// <summary>���� �ູ��</summary>
    [Range(-10, 10)] protected float _lastHappiness;

    //�Ʒ��� ���� ����, ģ�е� ���� �Լ��� �߻��Լ��� �ۼ�
    /// <summary>
    /// ģ�е� ���� </summary>
    public abstract void ChangeIntimacy(float changeIntimacy);
    public abstract void ChangeHappiness(float changeHappiness);

    public void GiveAGift()
    {
        // ���� ���� ���� ����
        if (UnityEngine.Random.Range(0, 10) == 9 && _isGift == false)
        {
            Debug.Log("�Ǵٰ� �ִ� ����");
            _isGift = true;
            GiftHandler?.Invoke();
        }
    }

    public void TakeAGift()
    {
        _isGift = false;
        _uiPanda.gameObject.transform.GetChild(2).gameObject.SetActive(false);
    }


    public void ToggleUIPandaButton()
    {
        _isUISetActive = !_isUISetActive;
        if (_isUISetActive)
        {
            _uiPanda.transform.GetChild(0).gameObject.SetActive(true);
            UIAlphaHandler?.Invoke(1f, 1f, null);
        }
        else
        {
            UIAlphaHandler?.Invoke(0f, 1f, () =>
            {
                _uiPanda.transform.GetChild(0).gameObject.SetActive(false);
            });
        }
    }

    /// <summary>
    /// �Ǵ� ���� �̹��� ǥ�� </summary>
    public void ShowStateImage()
    {

        if (Mathf.FloorToInt(_happiness) != Mathf.FloorToInt(_lastHappiness) && _stateImageTimer > 2f)
        {
            ImageAlphaHandler?.Invoke(_uiPanda.gameObject.transform.GetChild(1).gameObject, 1f, 0.7f, () =>
            {
                ImageAlphaHandler?.Invoke(_uiPanda.gameObject.transform.GetChild(1).gameObject, 0f, 0.7f, null);
            });
            _stateImageTimer = 0f;
        }
        _lastHappiness = _happiness;

    }

    public void StartInteraction()
    {
        ToggleUIPandaButton();
    }

    public void UpdateInteraction()
    {

    }

    public void ExitInteraction()
    {
        ToggleUIPandaButton();
    }

    // ���߿� ����
    protected void PandaMouseClick()
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

    protected void SetPandaData(PandaData pandaData)
    {

        _pandaName = pandaData.PandaName;
        Mbti = pandaData.MBTI;
        _intimacy = pandaData.Intimacy;
        _happiness = pandaData.Happiness;
        _lastHappiness = _happiness;
        _pandaImage = pandaData.CurrrentImage;
        GetComponent<SpriteRenderer>().sprite = _pandaImage;
    }

    /// <summary>
    /// �Ǵ��� UI ���� �� ���� </summary>
    protected void SetUIPanda()
    {
        //UIPanda ������ �ҷ�����
        _uiPandaParent = GameObject.Find("UIPandas");

        _uiPanda = Instantiate(_uiPandaPrefab, transform.position, Quaternion.identity, _uiPandaParent.transform);
        _uiPanda.Init(this);
        _uiPanda.gameObject.SetActive(true);
    }

}