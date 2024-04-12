using UnityEngine;
using Muks.DataBind;

public class CityHall : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;

    [SerializeField] private GameObject _backGround;
    //[SerializeField] private GameObject _market;
    [SerializeField] private GameObject _cityHall;
    [SerializeField] private GameObject _cityHallOffice;

    [Header("Button")]
    [SerializeField] private GameObject _enterCityHallButton;
    [SerializeField] private GameObject _exitCityHallButton;
    [SerializeField] private GameObject _enterCityHallOfficeButton;
    [SerializeField] private GameObject _exitCityHallOfficeButton;

    [Header("Sky")]
    [Tooltip("0:Day 1:Evening 2:Night")]
    [SerializeField] private GameObject[] _skyCityHall;

    [SerializeField] private GameObject[] _skyOffice;

    private Vector2 _mapSize;
    private Vector2 _cityHallMapSize;
    private Vector2 _cityHallOfficeMapSize;

    private float _fadeTime = 1f;

    private int _dayEndTime;
    //private int _eveningEndTime;
    private int _nightEndTime;

    void Start()
    {
        _mapSize = new Vector2(59.5f, _cameraController.MapSize.y);
        _cityHallMapSize = new Vector2(28.5f, _cameraController.MapSize.y);
        _cityHallOfficeMapSize = new Vector2(45, _cameraController.MapSize.y);

        _dayEndTime = TimeManager.Instance.DayEndTime;
        //_eveningEndTime = TimeManager.Instance.EveningEndTime;
        _nightEndTime = TimeManager.Instance.NightEndTime;

        DataBind.SetUnityActionValue("EnterCityHallButton", () => {

            FadeInOutManager.Instance.FadeIn(_fadeTime, () => {
                EnterCityHall();
                FadeInOutManager.Instance.FadeOut(_fadeTime);
            });
        });

        DataBind.SetUnityActionValue("ExitCityHallButton", () => {

            FadeInOutManager.Instance.FadeIn(_fadeTime, () => {
                ExitCityHall();
                FadeInOutManager.Instance.FadeOut(_fadeTime);
            });
        });

        DataBind.SetUnityActionValue("EnterCityHallOfficeButton", () => {

            FadeInOutManager.Instance.FadeIn(_fadeTime, () => {
                EnterCityHallOffice();
                FadeInOutManager.Instance.FadeOut(_fadeTime);
            });
        });

        DataBind.SetUnityActionValue("ExitCityHallOfficeButton", () => {

            FadeInOutManager.Instance.FadeIn(_fadeTime, () => {
                ExitCityHallOffice();
                FadeInOutManager.Instance.FadeOut(_fadeTime);
            });
        });

    }

    private void EnterCityHall()
    {
        _cameraController.MapSize = _cityHallMapSize;

        _backGround.SetActive(false);
        //_market.SetActive(false);
        _enterCityHallButton.SetActive(false);
        _exitCityHallButton.SetActive(true);
        _enterCityHallOfficeButton.SetActive(true);
        _cityHall.SetActive(true);

        if(TimeManager.Instance.GameHour >= _nightEndTime && TimeManager.Instance.GameHour < _dayEndTime)
        {
            _skyCityHall[(int)ETime.Day].SetActive(true);
            _skyCityHall[(int)ETime.Night].SetActive(false);
        }
        //else if(TimeManager.Instance.GameHour >= _dayEndTime && TimeManager.Instance.GameHour < _eveningEndTime)
        //{
        //    _skyCityHall[(int)ETime.Evening].SetActive(true);
        //    _skyCityHall[(int)ETime.Day].SetActive(false);
        //}
        else
        {
            _skyCityHall[(int)ETime.Night].SetActive(true);
            _skyCityHall[(int)ETime.Day].SetActive(false);
            //_skyCityHall[(int)ETime.Evening].SetActive(false);
        }
    }

    private void ExitCityHall()
    {
        _cameraController.MapSize = _mapSize;
        _cameraController.transform.position = new Vector3(_cameraController.MapCenter.x + 10, _cameraController.MapCenter.y, _cameraController.transform.position.z);

        _backGround.SetActive(true);
        //_market.SetActive(true);
        _enterCityHallButton.SetActive(true);
        _exitCityHallButton.SetActive(false);
        _enterCityHallOfficeButton.SetActive(false);
        _cityHall.SetActive(false);

        TimeManager.Instance.CheckMap();
    }

    private void EnterCityHallOffice()
    {
        _cameraController.MapSize = _cityHallOfficeMapSize;
        _cameraController.transform.position = new Vector3(_cameraController.MapCenter.x, _cameraController.MapCenter.y, _cameraController.transform.position.z);

        _cityHall.SetActive(false);
        _exitCityHallButton.SetActive(false);
        _enterCityHallOfficeButton.SetActive(false);
        _exitCityHallOfficeButton.SetActive(true);
        _cityHallOffice.SetActive(true);

        if (TimeManager.Instance.GameHour >= _nightEndTime && TimeManager.Instance.GameHour < _dayEndTime)
        {
            _skyOffice[(int)ETime.Day].SetActive(true);
            _skyOffice[(int)ETime.Night].SetActive(false);
        }
        //else if (TimeManager.Instance.GameHour >= _dayEndTime && TimeManager.Instance.GameHour < _eveningEndTime)
        //{
        //    _skyOffice[(int)ETime.Evening].SetActive(true);
        //    _skyOffice[(int)ETime.Day].SetActive(false);
        //}
        else
        {
            _skyOffice[(int)ETime.Night].SetActive(true);
            _skyOffice[(int)ETime.Day].SetActive(false);
            //_skyOffice[(int)ETime.Evening].SetActive(false);
        }
    }

    private void ExitCityHallOffice()
    {
        _cameraController.MapSize = _cityHallMapSize;
        _cameraController.transform.position = new Vector3(_cameraController.MapCenter.x, _cameraController.MapCenter.y, _cameraController.transform.position.z);

        _cityHall.SetActive(true);
        _exitCityHallButton.SetActive(true);
        _enterCityHallOfficeButton.SetActive(true);
        _exitCityHallOfficeButton.SetActive(false);
        _cityHallOffice.SetActive(false);
    }
}
