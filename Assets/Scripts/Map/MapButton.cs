using UnityEngine;
using UnityEngine.UI;
using Muks.Tween;
using Muks.DataBind;
using UnityEngine.SceneManagement;
using BT;
using TMPro;

public class MapButton : MonoBehaviour
{
    [Tooltip("���� ī�޶� ����")]
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private Transform[] _targetTransform;// = new Transform[DatabaseManager.Instance.GetMapDic().Count];

    [SerializeField] private TextMeshProUGUI _mapNameText;

    [Space]
    [Header("AudioClips")]
    [SerializeField] private AudioClip _treeAudioClip;
    [SerializeField] private AudioClip _mainTreeInsideAudioClip;
    [SerializeField] private AudioClip _fishingGroundAudioClip;
    [SerializeField] private AudioClip _forestAudioClip;
    [SerializeField] private AudioClip _crossRoadAudioClip;
    [SerializeField] private AudioClip _villageAudioClip;
    [SerializeField] private AudioClip _marcketAudioClip;
    [SerializeField] private AudioClip _cityHallLobbyAudioClip;
    [SerializeField] private AudioClip _cityHallBossRoomAudioClip;
    [SerializeField] private AudioClip _catworldAudioClip;
    [SerializeField] private AudioClip _castleNyangAudioClip;
    [SerializeField] private AudioClip _mermaidForestAudioClip;
    [SerializeField] private AudioClip _villageHouseAudioClip;



    private float _fadeTime = 1f;
    private int _currentMap;
    private string _currentMapID;
    public int CurrentMap
    {
        get { return _currentMap; }
        set
        {
            _currentMap = value;
            _currentMapID = "MN" + ((_currentMap + 1 >= 10) ? _currentMap + 1 : "0" + (_currentMap + 1));
            TimeManager.Instance.CurrentMap = _currentMapID;
        }
    }
    //private Vector2 _forestMapSize; // �ʸ��� ũ�Ⱑ �ٸ��ٸ� ����
    private Vector2 _mapSize;
    private Camera _camera;
    private float _height;
    private float _width;
    private bool _isLeft;

    private void Awake()
    {
        //_forestMapSize = new Vector2(30, _cameraController.MapSize.y);
        _mapSize = _cameraController.MapSize;
        _camera = _cameraController.GetComponent<Camera>();

        _height = _camera.orthographicSize;
        _width = _height * Screen.width / Screen.height;

        LoadingSceneManager.OnLoadSceneHandler += OnSceneChanged;

        Vector3 tmpCameraPos = GameManager.Instance.TmpCameraData.TmpPos;
        int tmpCenterIndex = GameManager.Instance.TmpCameraData.TmpCenterIndex;

        if (tmpCenterIndex != -1)
        {
            CurrentMap = tmpCenterIndex;
            _cameraController.MapCenter = _targetTransform[(int)this.CurrentMap].position;
            if(CurrentMap == 5)
            {
                _cameraController.MapSize = new Vector2(59.5f, _cameraController.MapSize.y);
            }
        }

        if(tmpCameraPos != Vector3.zero)
            _cameraController.transform.position = tmpCameraPos;

        DataBind.SetUnityActionValue("WishTreeButton", MoveWishTree);
        DataBind.SetUnityActionValue("FishingGroundButton", MoveFishingGround);
        DataBind.SetUnityActionValue("ForestEntranceButton", MoveForestEntrance);
        DataBind.SetUnityActionValue("ForestButton", MoveForest);
        DataBind.SetUnityActionValue("VillageButton", MoveVillage);
        DataBind.SetUnityActionValue("MarketButton", MoveMarket);
        DataBind.SetUnityActionValue("CatWorldButton", MoveCatWorld);
        DataBind.SetUnityActionValue("MermaidForestButton", MoveMermaidForest);
        DataBind.SetUnityActionValue("OtherWorldlyForestButton", MoveOtherWorldlyForest);
        DataBind.SetUnityActionValue("OtherWorldlyForestEntranceButton", MoveOtherWorldlyForestEntrance);
        DataBind.SetUnityActionValue("VillageHouseButton", MoveVillageHouse);
        DataBind.SetUnityActionValue("CityHallButton", MoveCityHall);
        DataBind.SetUnityActionValue("CityHallOfficeButton", MoveCityHallOffice);
        DataBind.SetUnityActionValue("CatCastleButton", MoveCatCastle);
        DataBind.SetUnityActionValue("InsideWishTreeButton", MoveInsideWishTree);
    }

    private void Start()
    {
        // ó�� ����
        if (_currentMapID == null)
        {
            _currentMapID = "MN01";
            _mapNameText.text = TimeManager.Instance.MapDatabase.GetMapDic()[_currentMapID].Name;
            Tween.TMPAlpha(_mapNameText.gameObject, 0, 2f, TweenMode.Constant, (System.Action)(() =>
            {
                ShowMapName();
            }));
            

        }
        // �� ����� ��
        else
        {
            ShowMapName();
        }
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


    private void MoveWishTree()
    {
        _cameraController.MapSize = _mapSize;
        SoundManager.Instance.PlayBackgroundAudio(_treeAudioClip, 2);
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);

        if (CurrentMap == 14)
        {
            CurrentMap = 0;
            MoveCenter();
        }
        else
        {
            CheckDirection();

            CurrentMap = 0;
            MoveField(_isLeft);
        }
    }

    private void MoveFishingGround()
    {
        CheckDirection();
        SoundManager.Instance.PlayBackgroundAudio(_fishingGroundAudioClip, 2);
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);

        CurrentMap = 1;
        _cameraController.MapSize = _mapSize;

        MoveField(_isLeft);
    }

    private void MoveForestEntrance()
    {
        CheckDirection();
        SoundManager.Instance.PlayBackgroundAudio(_crossRoadAudioClip, 2);
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);

        CurrentMap = 2;
        _cameraController.MapSize = _mapSize;

        MoveField(_isLeft);
    }
    private void MoveForest()
    {
        SoundManager.Instance.PlayBackgroundAudio(_forestAudioClip, 2);
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);

        if (CurrentMap == 8)
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
        SoundManager.Instance.PlayBackgroundAudio(_villageAudioClip, 2);
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);

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
        SoundManager.Instance.PlayBackgroundAudio(_marcketAudioClip, 2);
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);

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
        SoundManager.Instance.PlayBackgroundAudio(_catworldAudioClip, 2);
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);

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
        SoundManager.Instance.PlayBackgroundAudio(_mermaidForestAudioClip, 2);
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);

        CurrentMap = 7;
        _cameraController.MapSize = new Vector2(31f, _cameraController.MapSize.y);

        MoveField(_isLeft);
    }

    private void MoveOtherWorldlyForest()
    {
        SoundManager.Instance.PlayBackgroundAudio(_forestAudioClip, 2);
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);

        if (CurrentMap == 3)
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
        SoundManager.Instance.PlayBackgroundAudio(_crossRoadAudioClip, 2);
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);

        CheckDirection();

        CurrentMap = 9;
        _cameraController.MapSize = _mapSize;

        MoveField(_isLeft);
    }

    private void MoveVillageHouse()
    {
        SoundManager.Instance.PlayBackgroundAudio(_villageHouseAudioClip, 2);
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);

        CurrentMap = 10;
        _cameraController.MapSize = new Vector2(30f, _cameraController.MapSize.y);

        MoveCenter();
    }

    private void MoveCityHall()
    {
        SoundManager.Instance.PlayBackgroundAudio(_cityHallLobbyAudioClip, 2);
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);

        CurrentMap = 11;
        _cameraController.MapSize = new Vector2(28.5f, _cameraController.MapSize.y);

        MoveCenter();
    }

    private void MoveCityHallOffice()
    {
        SoundManager.Instance.PlayBackgroundAudio(_cityHallBossRoomAudioClip, 2);
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);

        CurrentMap = 12;
        _cameraController.MapSize = new Vector2(45, _cameraController.MapSize.y);

        MoveCenter();
    }

    private void MoveCatCastle()
    {
        SoundManager.Instance.PlayBackgroundAudio(_castleNyangAudioClip, 2);
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);

        CurrentMap = 13;
        _cameraController.MapSize = new Vector2(15f, _cameraController.MapSize.y);

        MoveCenter();
    }

    private void MoveInsideWishTree()
    {
        SoundManager.Instance.PlayBackgroundAudio(_mainTreeInsideAudioClip, 2);
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);

        CurrentMap = 14;
        _cameraController.MapSize = new Vector2(22.7f, _cameraController.MapSize.y);

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

            FadeOut();
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

            FadeOut();
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

            FadeOut();
        }));
    }

    private void FadeOut()
    {
        FadeInOutManager.Instance.FadeOut(_fadeTime, 0, (System.Action)(() =>
        {
            ShowMapName();
        }));
    }

    /// <summary>
    /// �� ���� �� �����ð� �� �̸� ��Ÿ���� �Լ�</summary>
    private void ShowMapName()
    {
        _mapNameText.text = TimeManager.Instance.MapDatabase.GetMapDic()[_currentMapID].Name;
        Tween.TMPAlpha(_mapNameText.gameObject, 1, 1f, TweenMode.Constant, (System.Action)(() =>
        {
            Tween.TMPAlpha(_mapNameText.gameObject, 0, 1.5f, TweenMode.Quadratic);
        }));
    }

    /// <summary>���� ������ �ٸ� ������ ����ɶ� ����Ǵ� �Լ�</summary>
    private void OnSceneChanged()
    {
        GameManager.Instance.TmpCameraData.SaveData(_cameraController.transform.position, CurrentMap);
        LoadingSceneManager.OnLoadSceneHandler -= OnSceneChanged;
    }
}
