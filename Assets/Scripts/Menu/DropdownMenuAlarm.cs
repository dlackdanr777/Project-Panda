using Muks.DataBind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownMenuAlarm : MonoBehaviour
{
    [SerializeField] private Image _alarmImage;

    private List<BindData<bool>> _bindDataList = new List<BindData<bool>>();

    private void Start()
    {
        LoadingSceneManager.OnLoadSceneHandler += ChangeSceneEvent;

        _bindDataList.Add(DataBind.GetBoolBindData("InvenAlarm"));
        _bindDataList.Add(DataBind.GetBoolBindData("AttendanceAlarm"));
        _bindDataList.Add(DataBind.GetBoolBindData("ChallengesAlarm"));
        _bindDataList.Add(DataBind.GetBoolBindData("DiaryAlarm"));
        
        for(int i = 0, count =  _bindDataList.Count; i < count; i++)
        {
            _bindDataList[i].CallBack += AlarmCheck;
        }

        AlarmCheck(false);
    }


    private void AlarmCheck(bool value)
    {
        bool alarm = false;

        alarm = DataBind.GetBoolValue("InvenAlarm") || alarm;
        alarm = DataBind.GetBoolValue("AttendanceAlarm") || alarm;
        alarm = DataBind.GetBoolValue("ChallengesAlarm") || alarm;
        alarm = DataBind.GetBoolValue("DiaryAlarm") || alarm;
        if(alarm)
            _alarmImage.gameObject.SetActive(true);
        else
            _alarmImage.gameObject.SetActive(false);

    }

    private void ChangeSceneEvent()
    {
        for (int i = 0, count = _bindDataList.Count; i < count; i++)
        {
            _bindDataList[i].CallBack -= AlarmCheck;
        }

        LoadingSceneManager.OnLoadSceneHandler -= ChangeSceneEvent;
    }


}
