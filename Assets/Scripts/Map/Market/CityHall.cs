using UnityEngine;
using Muks.DataBind;

public class CityHall : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;

    [SerializeField] private GameObject _backGround;
    [SerializeField] private GameObject _market;
    [SerializeField] private GameObject _cityHall;
    [SerializeField] private GameObject _cityHallOffice;

    [Header("Button")]
    [SerializeField] private GameObject _wishTreeButtonR;
    [SerializeField] private GameObject _enterCityHallButton;
    [SerializeField] private GameObject _exitCityHallButton;
    [SerializeField] private GameObject _enterCityHallOfficeButton;
    [SerializeField] private GameObject _exitCityHallOfficeButton;

    [Header("Sky")]
    [SerializeField] private GameObject _day;
    [SerializeField] private GameObject _night;
    [SerializeField] private GameObject _dayOffice;
    [SerializeField] private GameObject _nightOffice;

    private Vector2 _mapSize;
    private Vector2 _cityHallMapSize;
    private Vector2 _cityHallOfficeMapSize;

    private float _fadeTime = 1f;

    void Start()
    {
        _mapSize = new Vector2(59.5f, _cameraController.MapSize.y);
        _cityHallMapSize = new Vector2(28.5f, _cameraController.MapSize.y);
        _cityHallOfficeMapSize = new Vector2(45, _cameraController.MapSize.y);

        DataBind.SetButtonValue("EnterCityHallButton", () => {

            FadeInOutManager.Instance.FadeIn(_fadeTime, () => {
                EnterCityHall();
                FadeInOutManager.Instance.FadeOut(_fadeTime);
            });
        });

        DataBind.SetButtonValue("ExitCityHallButton", () => {

            FadeInOutManager.Instance.FadeIn(_fadeTime, () => {
                ExitCityHall();
                FadeInOutManager.Instance.FadeOut(_fadeTime);
            });
        });

        DataBind.SetButtonValue("EnterCityHallOfficeButton", () => {

            FadeInOutManager.Instance.FadeIn(_fadeTime, () => {
                EnterCityHallOffice();
                FadeInOutManager.Instance.FadeOut(_fadeTime);
            });
        });

        DataBind.SetButtonValue("ExitCityHallOfficeButton", () => {

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
        _market.SetActive(false);
        _wishTreeButtonR.SetActive(false);
        _enterCityHallButton.SetActive(false);
        _exitCityHallButton.SetActive(true);
        _enterCityHallOfficeButton.SetActive(true);
        _cityHall.SetActive(true);

        if(TimeManager.Instance.GameHour > 7 && TimeManager.Instance.GameHour < 17)
        {
            _day.SetActive(true);
            _night.SetActive(false);
        }
        else
        {
            _day.SetActive(false);
            _night.SetActive(true);
        }
    }

    private void ExitCityHall()
    {
        _cameraController.MapSize = _mapSize;
        _cameraController.transform.position = new Vector3(_cameraController.MapCenter.x + 10, _cameraController.MapCenter.y, _cameraController.transform.position.z);

        _backGround.SetActive(true);
        _market.SetActive(true);
        _wishTreeButtonR.SetActive(true);
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

        if (TimeManager.Instance.GameHour > 7 && TimeManager.Instance.GameHour < 18)
        {
            _dayOffice.SetActive(true);
            _nightOffice.SetActive(false);
        }
        else
        {
            _dayOffice.SetActive(false);
            _nightOffice.SetActive(true);
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
