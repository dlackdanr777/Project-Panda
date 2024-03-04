using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTime : MonoBehaviour
{
    [Header("NPC가 나타나는 시간")]
    [SerializeField] private bool _day;
    [SerializeField] private bool _evening;
    [SerializeField] private bool _night;

    private ETime _eTime;

    void Start()
    {
        _eTime = TimeManager.Instance.ETime;
        SetNPC();
    }

    void Update()
    {
        if(_eTime != TimeManager.Instance.ETime)
        {
            _eTime = TimeManager.Instance.ETime;
            SetNPC();
        }
        
    }

    private void SetNPC()
    {
        if (_eTime == ETime.Day)
        {
            gameObject.SetActive(_day);
        }
        else if (_eTime == ETime.Evening)
        {
            gameObject.SetActive(_evening);
        }
        else if (_eTime == ETime.Night)
        {
            gameObject.SetActive(_night);
        }
    }
}
