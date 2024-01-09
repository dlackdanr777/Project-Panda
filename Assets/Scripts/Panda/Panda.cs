using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>NPC 판다, 스타터 판다의 부모 클래스</summary>
public abstract class Panda : MonoBehaviour, IInteraction
{
    //상태 이미지 변경 액션
    public Action<string, int> StateHandler;
    public Action<float, float, Action> UIAlphaHandler;
    public Action<GameObject, float, float, Action> ImageAlphaHandler;
    public Action GiftHandler;

    /// <summary>판다 Mbti</summary>
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

    /// <summary>판다 친밀도</summary>
    [SerializeField]
    [Range(0, 100)] protected float _intimacy;
    public float Intimacy
    {
        get { return _intimacy; }
        private set { }
    }

    /// <summary>판다 행복도</summary>
    [SerializeField]
    [Range(-10, 10)] protected float _happiness;
    public float Happiness
    {
        get { return _happiness; }
        private set { }
    }
    /// <summary>이전 행복도</summary>
    [Range(-10, 10)] protected float _lastHappiness;

    //아래에 성향 관련, 친밀도 관련 함수를 추상함수로 작성
    /// <summary>
    /// 친밀도 변경 </summary>
    public abstract void ChangeIntimacy(float changeIntimacy);
    public abstract void ChangeHappiness(float changeHappiness);

    public void GiveAGift()
    {
        // 선물 랜덤 조건 설정
        if (UnityEngine.Random.Range(0, 10) == 9 && _isGift == false)
        {
            Debug.Log("판다가 주는 선물");
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
    /// 판다 상태 이미지 표시 </summary>
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

    // 나중에 삭제
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
    /// 판다의 UI 생성 후 세팅 </summary>
    protected void SetUIPanda()
    {
        //UIPanda 프리팹 불러오기
        _uiPandaParent = GameObject.Find("UIPandas");

        _uiPanda = Instantiate(_uiPandaPrefab, transform.position, Quaternion.identity, _uiPandaParent.transform);
        _uiPanda.Init(this);
        _uiPanda.gameObject.SetActive(true);
    }

}