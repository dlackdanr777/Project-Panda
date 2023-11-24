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

    [SerializeField]
    protected UIPanda _uiPanda;
    protected bool _isUISetActive;
    protected float _stateImageTimer = 1f;

    protected bool _isCameraRequest {  get; private set; }

    protected MBTIData MbtiData;
    /// <summary>����</summary>
    [SerializeField] // ���߿� �Ⱥ��̵��� ����
    protected string _mbti { get; set; } // ���� �޾ƿͼ� ���� ����

    /// <summary>ģ�е�</summary>
    protected int _intimacy { get; private set; }

    /// <summary>�ູ��</summary>
    [SerializeField]
    [Range(-10, 10)] public float _happiness;
    /// <summary>���� �ູ��</summary>
    protected float _lastHappiness;

    protected Preference _preference;

    //�Ʒ��� ���� ����, ģ�е� ���� �Լ��� �߻��Լ��� �ۼ�
    /// <summary>
    /// ģ�е� ����
    /// </summary>
    protected abstract void ChangeIntimacy(int changeIntimacy);

    protected abstract void SetPreference(string mbti);

    protected virtual void Awake()
    {
        MbtiData = new MBTIData();
        MbtiData.SetMBTI();
    }

    /// <summary>
    /// �Ǵ� Ŭ���ϸ� UI ǥ��
    /// </summary>
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