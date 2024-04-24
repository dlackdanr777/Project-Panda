using UnityEngine;
using Muks.DataBind;

public class VillageHouse : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;

    [SerializeField] private GameObject _backGround;
    //[SerializeField] private GameObject _village;
    [SerializeField] private GameObject _villageHouse;

    [Header("Button")]
    [SerializeField] private GameObject _enterVillageHouseButton;
    [SerializeField] private GameObject _exitVillageHouseButton;

    [Header("Sky")]
    [SerializeField] private GameObject _day;
    //[SerializeField] private GameObject _evening;
    [SerializeField] private GameObject _night;

    private Vector2 _mapSize;
    private Vector2 _villageHouseMapSize;

    private float _fadeTime = 1f;

    private int _dayEndTime;
    private int _eveningEndTime;
    private int _nightEndTime;

    void Start()
    {
        _mapSize = _cameraController.MapSize;
        _villageHouseMapSize = new Vector2(30f, _cameraController.MapSize.y);

        _dayEndTime = TimeManager.Instance.DayEndTime;
        //_eveningEndTime = TimeManager.Instance.EveningEndTime;
        _nightEndTime = TimeManager.Instance.NightEndTime;

        DataBind.SetUnityActionValue("EnterVillageHouseButton", () => {

            FadeInOutManager.Instance.FadeIn(_fadeTime, () => {
                EnterVillageHouse();
                FadeInOutManager.Instance.FadeOut(_fadeTime);
            });
        });

        DataBind.SetUnityActionValue("ExitVillageHouseButton", () => {

            FadeInOutManager.Instance.FadeIn(_fadeTime, () => {
                ExitVillageHouse();
                FadeInOutManager.Instance.FadeOut(_fadeTime);
            });
        });

    }

    private void EnterVillageHouse()
    {
        _cameraController.MapSize = _villageHouseMapSize;

        _backGround.SetActive(false);
        //_village.SetActive(false);
        _enterVillageHouseButton.SetActive(false);
        _exitVillageHouseButton.SetActive(true);
        _villageHouse.SetActive(true);

        if (TimeManager.Instance.GameHour >= _nightEndTime && TimeManager.Instance.GameHour < _dayEndTime)
        {
            _day.SetActive(true);
            _night.SetActive(false);
        }
        //else if(TimeManager.Instance.GameHour >= _dayEndTime && TimeManager.Instance.GameHour < _eveningEndTime)
        //{
        //    _evening.SetActive(true);
        //    _day.SetActive(false);
        //}
        else
        {
            _night.SetActive(true);
            _day.SetActive(false);
            //_evening.SetActive(false);
        }
    }

    private void ExitVillageHouse()
    {
        _cameraController.MapSize = _mapSize;
        _cameraController.transform.position = new Vector3(_cameraController.MapCenter.x + 10, _cameraController.MapCenter.y, _cameraController.transform.position.z);

        _backGround.SetActive(true);
        //_village.SetActive(true);
        _enterVillageHouseButton.SetActive(true);
        _exitVillageHouseButton.SetActive(false);
        _villageHouse.SetActive(false);

        TimeManager.Instance.CheckMap();
    }
}
