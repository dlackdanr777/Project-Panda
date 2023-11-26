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

    [SerializeField]
    protected UIPanda _uiPanda;
    protected bool _isUISetActive;
    protected float _stateImageTimer = 1f;

    protected string PandaID;
    protected string PandaName;
    protected Sprite PandaImage;

    /// <summary>�Ǵ� Mbti</summary>
    public string Mbti;
    protected Preference _preference;

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

    protected bool _isCameraRequest {  get; private set; }
    protected bool _isGift;


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
    /// �Ǵ� ���� �̹��� ǥ��
    /// </summary>
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
    protected void SetPandaData(PandaData pandaData)
    {
        PandaName = pandaData.PandaName;
        Mbti = pandaData.MBTI;
        _intimacy = pandaData.Intimacy;
        _happiness = pandaData.Happiness;
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
}