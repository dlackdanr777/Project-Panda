using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muks.DataBind;
using System.Linq;
using System;
using UnityEngine.UI;
using Muks.Tween;
using UnityEngine.U2D;
using Unity.VisualScripting;
using System.Reflection;

public class UIWeather : MonoBehaviour
{
    [SerializeField] private WeatherApp _weatherApp;

    //���̾ƿ��� �ִ� ��
    [SerializeField] private GameObject _layoutGroup;

    //������ �����ִ� �Ϲ����� ����
    [SerializeField] private GameObject _uiSlot;

    //���� ��¥�� �����ִ� ����
    [SerializeField] private GameObject _uiTodaySlot;

    [SerializeField] private Image _attendanceStamp;

    [SerializeField] private Sprite _attendanceSprite;

    [SerializeField]
    private List<UIWeatherSlot> _slots;

    private List<WeatherData> _weekWeathers;

    private WeatherData _todayWeatherData;

    public event Action OnRewardedHandler;


    private void OnEnable()
    {
        GameManager.Instance.FriezeCameraMove = true;
        GameManager.Instance.FriezeCameraZoom = true;
    }

    private void OnDisable()
    {
        GameManager.Instance.FriezeCameraMove = false;
        GameManager.Instance.FriezeCameraZoom = false;
    }

    public void InitSlot()
    {
        _slots = new List<UIWeatherSlot>();
        _slots = _layoutGroup.GetComponentsInChildren<UIWeatherSlot>().ToList();

        for(int i = 0, count = _slots.Count; i < count; i++)
        {
            _slots[i].UpdateUI(_weekWeathers[i], i + 1);
        }

        //���Ե��� �������ش�.
        for (int i = 0, count = _slots.Count; i < count; i++)
        {

            //���� ���� ���� �����̶��?
            if ((UserInfo.DayCount % 7) -1 == i)
            {
                //������ ���޵Ǵ� ���� �ƴϸ�?
                if (!_weatherApp.IsCanReward)
                {
                    _slots[i].AttendanceComplated(_attendanceSprite);
                }
                else
                {
                    //����ȹ�� �̺�Ʈ
                    //RewardAnime(_slots[i]);
                    _slots[i].AttendanceComplated(_attendanceSprite);
                    _todayWeatherData = _weekWeathers[i];
                    //�̺�Ʈ�ڵ鷯 ����
                    OnRewardedHandler?.Invoke();
                }

                //�ݺ����� �������´�.
                break;
            }

            //�ƴ϶��
            else
            {
                //�⼮������ ����ش�.
                _slots[i].AttendanceComplated(_attendanceSprite);
            }
        }

    }


    public void Init()
    {
        _weekWeathers = _weatherApp.GetWeekWeathers().ToList();
        _attendanceStamp.gameObject.SetActive(false);
        InitSlot();
        SetBind();
        gameObject.SetActive(false);
    }


    private void SetBind()
    {
        DataBind.SetButtonValue("UI Weather Exit Button", () => gameObject.SetActive(false));
        DataBind.SetButtonValue("UI Weather Open Button", () => gameObject.SetActive(!gameObject.activeSelf));
        DataBind.SetSpriteValue("TodayWeatherImage", _todayWeatherData.WeatherSprite);
    }


    //�⼮üũ�� �������� �߻��� �̺�Ʈ
    private void RewardAnime(UIWeatherSlot uiWeatherSlot)
    {
        _attendanceStamp.gameObject.SetActive(true);

        RectTransform rectTransform = !_attendanceStamp.GetComponent<RectTransform>()
            ? _attendanceStamp.AddComponent<RectTransform>()
            : _attendanceStamp.GetComponent<RectTransform>();

        _attendanceStamp.transform.position = uiWeatherSlot.AttendanceStamp.transform.position;
        _attendanceStamp.transform.rotation = uiWeatherSlot.AttendanceStamp.transform.rotation;
        _attendanceStamp.sprite = _attendanceSprite;

        Vector2 tmepSizeDelta = rectTransform.sizeDelta;
        rectTransform.sizeDelta = new Vector2(200, 200);


        Tween.RectTransfromSizeDelta(_attendanceStamp.gameObject, rectTransform.sizeDelta, 0.1f);
        Tween.RectTransfromSizeDelta(_attendanceStamp.gameObject, tmepSizeDelta, 0.5f, TweenMode.Quadratic, () =>
        {
            uiWeatherSlot.AttendanceStamp.gameObject.SetActive(true);
            uiWeatherSlot.AttendanceStamp.sprite = _attendanceSprite;
            _attendanceStamp.gameObject.SetActive(false);
            
            //���ʿ� ������ ȹ���� �� �ֵ��� �ؾ���
        });

    }
}
