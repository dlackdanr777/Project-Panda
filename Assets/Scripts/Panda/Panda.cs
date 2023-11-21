using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>NPC 판다, 스타터 판다의 부모 클래스</summary>
public abstract class Panda : MonoBehaviour
{
    [SerializeField]
    protected UIPanda _uiPanda;
    protected bool _isUISetActive; 
    protected float _stateImageTimer = 1f;

    //상태 이미지 변경 액션
    public Action<int> StateHandler;
    public Action<float, float, Action> UIAlphaHandler;
    public Action<GameObject, float, float, Action> AlphaImageHandler;

    /// <summary>성향</summary>
    protected string _mbtiData;

    /// <summary>친밀도</summary>
    protected int _intimacy;

    /// <summary>행복도</summary>
    [SerializeField]
    [Range(-10, 10)] protected float _happiness;
    /// <summary>이전 행복도</summary>
    protected float _lastHappiness;

    //아래에 성향 관련, 친밀도 관련 함수를 추상함수로 작성
    protected abstract void ChangeIntimacy(int changeIntimacy);

    protected abstract void SetPreference(string mbti);


    /// <summary>
    /// 판다 클릭하면 UI 표시
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
    /// 판다 상태 이미지 표시
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
