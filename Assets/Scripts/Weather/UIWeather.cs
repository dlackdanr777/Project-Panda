using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muks.DataBind;
using System.Linq;
using System;
using UnityEngine.UI;
using Muks.Tween;

public class UIWeather : UIView
{
    [SerializeField] private WeatherDatabase _weatherApp;

    [SerializeField] private Transform _slotLayoutParent;

    [SerializeField] private GameObject _daySlotLayoutGroup;

    [SerializeField] private UIWeatherSlot _todayUISlot;

    [SerializeField] private UIWeatherSlot _slotPrefab;


    private List<UIWeatherSlot> _slots = new List<UIWeatherSlot>();

    private Vector3 _tmpSize;

    private Vector3 _targetSize => new Vector3(0.7f, 0.7f, 0.7f);

    public event Action OnRewardedHandler;

    public override void Show()
    {
        ShowAnime();
    }

    public override void Hide()
    {
        HideAnime();
    }


    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
        //SlotInit();

        _tmpSize = gameObject.transform.localScale;
    }

    
    private void ShowAnime()
    {
        gameObject.SetActive(true);
        VisibleState = VisibleState.Appearing;
        gameObject.transform.localScale = _targetSize;

        Tween.TransformScale(gameObject, _tmpSize, 0.3f, TweenMode.EaseOutBack, () =>
        {
            VisibleState = VisibleState.Appeared;
        });

    }


    private void HideAnime()
    {
        VisibleState = VisibleState.Disappearing;
        Tween.TransformScale(gameObject, _targetSize * 0.5f, 0.5f, TweenMode.EaseInBack, () =>
        {
            VisibleState = VisibleState.Disappeared;
            gameObject.SetActive(false);
        });
    }


    private void SlotInit()
    {
        List<WeatherRewardData> list = DatabaseManager.Instance.WeatherDatabase.GetWeatherRewardDataForDayCount();

        for (int i = 0, count = list.Count; i < count; i++)
        {
            UIWeatherSlot slot;

            //���� ����UI�� ��� ũ�� ����ؾ� �ϹǷ� �̸� �����ص� UISlot�� Ȱ���ؾ��մϴ�.
            //�׷��⿡ 0��° �迭�� ���� ������ ��� ifó���� ���� ������ �����մϴ�.
            if (i != 0)
            {
                slot = Instantiate(_slotPrefab);
                slot.transform.parent = _slotLayoutParent;
            }
            else
            {
                slot = _todayUISlot;
            }

            slot.UpdateUI(list[i]);
            _slots.Add(slot);
        }
    }

   /*public void InitSlot()
    {
        _slots = new List<UIWeatherSlot>();
        _slots = _slotLayoutGroup.GetComponentsInChildren<UIWeatherSlot>().ToList();
        _daySlots = _daySlotLayoutGroup.GetComponentsInChildren<UIDaySlot>().ToList();

        for (int i = 0, count = _slots.Count; i < count; i++)
        {
            _slots[i].UpdateUI(_weekWeathers[i], i + 1);
            _daySlots[i].SetDayText(i + 1);
        }

        //���Ե��� �������ش�.
        for (int i = 0, count = _slots.Count; i < count; i++)
        {
            //���� ���� ���� �����̶��?
            if ((DatabaseManager.Instance.UserInfo.DayCount -1 % 6) == i)
            {
                _todayWeatherData = _weekWeathers[i];
                _daySlots[i].Init(true);

                //������ ���޵Ǵ� ���̸�?
                if (_weatherApp.IsCanReward)
                {

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
                _daySlots[i].Init(true);
            }
        }


    }


    public void Init()
    {
        _weekWeathers = _weatherApp.GetWeekWeathers().ToList();
        InitSlot();
        SetBind();
        gameObject.SetActive(false);
    }


    private void SetBind()
    {
        DataBind.SetSpriteValue("TodayWeatherImage", _todayWeatherData.WeatherSprite);
        string weekText = DatabaseManager.Instance.UserInfo.TODAY.DayOfWeek.ToString().Substring(0, 3);
        DataBind.SetTextValue("WeekText", weekText);
    }*/




    /* //�⼮üũ�� �������� �߻��� �̺�Ʈ
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
 */
}

