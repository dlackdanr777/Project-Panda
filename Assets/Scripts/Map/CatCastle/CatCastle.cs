using Muks.DataBind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatCastle : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;

    [SerializeField] private GameObject _backGround;
    //[SerializeField] private GameObject _catWorld;
    [SerializeField] private GameObject _catCastle;

    [Header("Button")]
    [SerializeField] private GameObject _enterCatCastleButton;
    [SerializeField] private GameObject _exitCatCastleButton;

    [Header("Sky")]
    [SerializeField] private GameObject _day;
    [SerializeField] private GameObject _evening;
    [SerializeField] private GameObject _night;

    private Vector2 _mapSize;
    private Vector2 _catCastleMapSize;

    private float _fadeTime = 1f;

    private int _dayEndTime;
    private int _eveningEndTime;
    private int _nightEndTime;

    void Start()
    {
        _mapSize = _cameraController.MapSize;
        _catCastleMapSize = new Vector2(15f, _cameraController.MapSize.y);

        _dayEndTime = TimeManager.Instance.DayEndTime;
        _eveningEndTime = TimeManager.Instance.EveningEndTime;
        _nightEndTime = TimeManager.Instance.NightEndTime;

        DataBind.SetButtonValue("EnterCatCastleButton", () => {

            FadeInOutManager.Instance.FadeIn(_fadeTime, () => {
                EnterCatCastle();
                FadeInOutManager.Instance.FadeOut(_fadeTime);
            });
        });

        DataBind.SetButtonValue("ExitCatCastleButton", () => {

            FadeInOutManager.Instance.FadeIn(_fadeTime, () => {
                ExitCatCastle();
                FadeInOutManager.Instance.FadeOut(_fadeTime);
            });
        });

    }

    private void EnterCatCastle()
    {
        _cameraController.MapSize = _catCastleMapSize;

        _backGround.SetActive(false);
        //_catWorld.SetActive(false);
        _enterCatCastleButton.SetActive(false);
        _exitCatCastleButton.SetActive(true);
        _catCastle.SetActive(true);

        if (TimeManager.Instance.GameHour >= _nightEndTime && TimeManager.Instance.GameHour < _dayEndTime)
        {
            _day.SetActive(true);
            _night.SetActive(false);
        }
        else if (TimeManager.Instance.GameHour >= _dayEndTime && TimeManager.Instance.GameHour < _eveningEndTime)
        {
            _evening.SetActive(true);
            _day.SetActive(false);
        }
        else
        {
            _night.SetActive(true);
            _evening.SetActive(false);
        }
    }

    private void ExitCatCastle()
    {
        _cameraController.MapSize = _mapSize;
        _cameraController.transform.position = new Vector3(_cameraController.MapCenter.x + 10, _cameraController.MapCenter.y, _cameraController.transform.position.z);

        _backGround.SetActive(true);
        //_catWorld.SetActive(true);
        _enterCatCastleButton.SetActive(true);
        _exitCatCastleButton.SetActive(false);
        _catCastle.SetActive(false);

        TimeManager.Instance.CheckMap();
    }
}