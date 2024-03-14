using UnityEngine;
using UnityEngine.UI;
using Muks.Tween;
using Muks.DataBind;
using static UnityEditor.PlayerSettings;

public class MapButton : MonoBehaviour
{
    [Tooltip("���� ī�޶� ����")]
    [SerializeField] private CameraController _cameraController;

    [SerializeField] private Transform[] _targetTransform;// = new Transform[DatabaseManager.Instance.GetMapDic().Count];
    private float _fadeTime = 1f;
    private int _currentMap;
    public int CurrentMap {
        get { return _currentMap; }
        set
        {
            _currentMap = value;
            TimeManager.Instance.CurrentMap = "MN" + (_currentMap + 1 >= 10 ? (_currentMap + 1) : "0" + (_currentMap + 1));
        } }
    //private Vector2 _forestMapSize; // �ʸ��� ũ�Ⱑ �ٸ��ٸ� ����
    private Vector2 _mapSize;
    private Camera _camera;
    private float _height;
    private float _width;
    private bool _isLeft;

    private void Awake()
    {
        _currentMap = 0;

        //_forestMapSize = new Vector2(30, _cameraController.MapSize.y);
        _mapSize = _cameraController.MapSize;
        _camera = _cameraController.GetComponent<Camera>();

        _height = _camera.orthographicSize;
        _width = _height * Screen.width / Screen.height;

        DataBind.SetButtonValue("WishTreeButton", MoveWishTree);
        DataBind.SetButtonValue("FishingGroundButton", MoveFishingGround);
        DataBind.SetButtonValue("ForestEntranceButton", MoveForestEntrance);
        DataBind.SetButtonValue("ForestButton", MoveForest);
        DataBind.SetButtonValue("VillageButton", MoveVillage);
        DataBind.SetButtonValue("MarketButton", MoveMarket);
        DataBind.SetButtonValue("CatWorldButton", MoveCatWorld);
        DataBind.SetButtonValue("MermaidForestButton", MoveMermaidForest);
        DataBind.SetButtonValue("OtherWorldlyForestButton", MoveOtherWorldlyForest);
        DataBind.SetButtonValue("OtherWorldlyForestEntranceButton", MoveOtherWorldlyForestEntrance);
        DataBind.SetButtonValue("VillageHouseButton", MoveVillageHouse);
        DataBind.SetButtonValue("CityHallButton", MoveCityHall);
        DataBind.SetButtonValue("CityHallOfficeButton", MoveCityHallOffice);
        DataBind.SetButtonValue("CatCastleButton", MoveCatCastle);
    }

    /// <summary>
    /// ���� �̵��ϴ� ���� Ȯ��</summary>
    private void CheckDirection()
    {
        if (Camera.main.transform.position.x < _targetTransform[CurrentMap].position.x)
        {
            _isLeft = true;
        }
        else
        {
            _isLeft = false;
        }
    }


    public void MoveWishTree()
    {
        CheckDirection();

        CurrentMap = 0;
        _cameraController.MapSize = _mapSize;

        MoveField(_isLeft);
    }
    private void MoveFishingGround()
    {
        CheckDirection();

        CurrentMap = 1;
        _cameraController.MapSize = _mapSize;

        MoveField(_isLeft);
    }

    private void MoveForestEntrance()
    {
        CheckDirection();

        CurrentMap = 2;
        _cameraController.MapSize = _mapSize;

        MoveField(_isLeft);
    }
    private void MoveForest()
    {
        if(CurrentMap == 8)
        {
            CurrentMap = 3;
            MoveOtherWorld();
        }
        else
        {
            CheckDirection();

            CurrentMap = 3;
            _cameraController.MapSize = _mapSize;

            MoveField(_isLeft);
        }
    }
    private void MoveVillage()
    {
        _cameraController.MapSize = _mapSize;
        if (CurrentMap == 10)
        {
            CurrentMap = 4;
            MoveCenter();
        }
        else
        {
            CheckDirection();

            CurrentMap = 4;
            MoveField(_isLeft);
        }

    }
    private void MoveMarket()
    {
        _cameraController.MapSize = new Vector2(59.5f, _cameraController.MapSize.y);

        if (CurrentMap == 11)
        {
            CurrentMap = 5;
            MoveCenter();
        }
        else
        {
            CheckDirection();

            CurrentMap = 5;
            MoveField(_isLeft);
        }

    }

    private void MoveCatWorld()
    {
        _cameraController.MapSize = _mapSize;
        if (CurrentMap == 13)
        {
            CurrentMap = 6;
            MoveCenter();
        } 
        else
        {
            CheckDirection();

            CurrentMap = 6;
            MoveField(_isLeft);
        }
        
    }

    private void MoveMermaidForest()
    {
        CheckDirection();

        CurrentMap = 7;
        _cameraController.MapSize = new Vector2(31f, _cameraController.MapSize.y);

        MoveField(_isLeft);
    }

    private void MoveOtherWorldlyForest()
    {
        if(CurrentMap == 3)
        {
            CurrentMap = 8;
            MoveOtherWorld();
        }
        else
        {
            CheckDirection();

            CurrentMap = 8;
            _cameraController.MapSize = _mapSize;

            MoveField(_isLeft);
        }
    }

    private void MoveOtherWorldlyForestEntrance()
    {
        CheckDirection();

        CurrentMap = 9;
        _cameraController.MapSize = _mapSize;

        MoveField(_isLeft);
    }

    private void MoveVillageHouse()
    {
        CurrentMap = 10;
        _cameraController.MapSize = new Vector2(30f, _cameraController.MapSize.y);

        MoveCenter();
    }

    private void MoveCityHall()
    {
        CurrentMap = 11;
        _cameraController.MapSize = new Vector2(28.5f, _cameraController.MapSize.y);

        MoveCenter();
    }

    private void MoveCityHallOffice()
    {
        CurrentMap = 12;
        _cameraController.MapSize = new Vector2(45, _cameraController.MapSize.y);

        MoveCenter();
    }

    private void MoveCatCastle()
    {
        CurrentMap = 13;
        _cameraController.MapSize = new Vector2(15f, _cameraController.MapSize.y);

        MoveCenter();
    }

    // ������ �̵�
    private void MoveField(bool isLeft)
    {
        Vector3 targetPos = new Vector3(_targetTransform[CurrentMap].position.x, _targetTransform[CurrentMap].position.y, Camera.main.transform.position.z);

        FadeInOutManager.Instance.FadeIn(_fadeTime, (System.Action)(() =>
        {
            TimeManager.Instance.CheckMap();

            _cameraController.MapCenter = _targetTransform[(int)this.CurrentMap].position;
            //Camera.main.transform.position = targetPos;

            // �ٸ� ������ �̵��� �� ���� �������� �̵��ϴ� ��ó�� ���̰� ��
            _width = _height * Screen.width / Screen.height;
            float lx = _cameraController.MapSize.x - _width;
            //float ly = _cameraController.MapSize.y - _height;

            if (isLeft) // ���� �������� ���� +lx
            {
                Camera.main.transform.position = targetPos + new Vector3(lx, 0, 0);
            }
            else // ������ �������� ���� -lx 
            {
                Camera.main.transform.position = targetPos + new Vector3(-lx, 0, 0);
            }

            FadeInOutManager.Instance.FadeOut(_fadeTime);
        }));
    }

    // ���� ��ġ�� �̵�
    private void MoveOtherWorld()
    {
        Vector3 targetPos = new Vector3(_targetTransform[CurrentMap].position.x, _targetTransform[CurrentMap].position.y, Camera.main.transform.position.z);
        FadeInOutManager.Instance.FadeIn(_fadeTime, (System.Action)(() =>
        {
            TimeManager.Instance.CheckMap();

            // �ڵ����� �̵� �� ���� ��ġ�� ī�޶� �̵�
            float lx = _cameraController.MapCenter.x - Camera.main.transform.position.x;

            _cameraController.MapCenter = _targetTransform[(int)this.CurrentMap].position;

            Camera.main.transform.position = targetPos - new Vector3(lx, 0, 0);

            FadeInOutManager.Instance.FadeOut(_fadeTime);
        }));
    }

    // �߽����� �̵�
    private void MoveCenter()
    {
        Vector3 targetPos = new Vector3(_targetTransform[CurrentMap].position.x, _targetTransform[CurrentMap].position.y, Camera.main.transform.position.z);
        FadeInOutManager.Instance.FadeIn(_fadeTime, (System.Action)(() =>
        {
            TimeManager.Instance.CheckMap();

            _cameraController.MapCenter = _targetTransform[(int)this.CurrentMap].position;

            Camera.main.transform.position = targetPos;

            FadeInOutManager.Instance.FadeOut(_fadeTime);
        }));
    }

}
