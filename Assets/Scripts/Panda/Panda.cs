using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>NPC �Ǵ�, ��Ÿ�� �Ǵ��� �θ� Ŭ����</summary>
public abstract class Panda : MonoBehaviour
{
    [SerializeField]
    protected UIPanda _uiPanda;
    protected bool _isUISetActive; 
    protected float _stateImageTimer = 1f;

    //���� �̹��� ���� �׼�
    public Action<int> StateHandler;
    public Action<float, float, Action> UIAlphaHandler;
    public Action<GameObject, float, float, Action> AlphaImageHandler;

    /// <summary>����</summary>
    protected string _mbtiData;

    /// <summary>ģ�е�</summary>
    protected int _intimacy;

    /// <summary>�ູ��</summary>
    [SerializeField]
    [Range(-10, 10)] protected float _happiness;
    /// <summary>���� �ູ��</summary>
    protected float _lastHappiness;

    //�Ʒ��� ���� ����, ģ�е� ���� �Լ��� �߻��Լ��� �ۼ�
    protected abstract void ChangeIntimacy(int changeIntimacy);

    protected abstract void SetPreference(string mbti);


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
            AlphaImageHandler?.Invoke(_uiPanda.gameObject.transform.GetChild(1).gameObject, 1f, 0.7f, () =>
            {
                AlphaImageHandler?.Invoke(_uiPanda.gameObject.transform.GetChild(1).gameObject, 0f, 0.7f, null);
            });
            _stateImageTimer = 0f;
        }
        _lastHappiness = _happiness;

    }
}
