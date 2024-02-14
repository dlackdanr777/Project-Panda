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

        DataBind.SetButtonValue("WishTreeButton", MoveWishTree);
        DataBind.SetButtonValue("FishingGroundButton", MoveFishingGround);
        DataBind.SetButtonValue("ForestEntranceButton", MoveForestEntrance);
        DataBind.SetButtonValue("ForestButton", MoveForest);
        DataBind.SetButtonValue("VillageButton", MoveVillage);
        DataBind.SetButtonValue("MarketButton", MoveMarket);
        DataBind.SetButtonValue("MermaidForestButton", MoveMermaidForest);
    }

    /// <summary>
    /// ���� �̵��ϴ� ���� Ȯ��</summary>
    private void CheckDirection()
    {
        if (Camera.main.transform.position.x < _targetTransform[_currentMap].position.x)
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

        _currentMap = 0;
        _cameraController.MapSize = _mapSize;

        MoveField(_isLeft);
    }
    private void MoveFishingGround()
    {
        CheckDirection();

        _currentMap = 1;
        _cameraController.MapSize = _mapSize;

        MoveField(_isLeft);
    }

    private void MoveForestEntrance()
    {
        CheckDirection();

        _currentMap = 2;
        _cameraController.MapSize = _mapSize;

        MoveField(_isLeft);
    }
    private void MoveForest()
    {
        CheckDirection();

        _currentMap = 3;
        _cameraController.MapSize = _mapSize;

        MoveField(_isLeft);
    }
    private void MoveVillage()
    {
        CheckDirection();

        _currentMap = 4;
        _cameraController.MapSize = _mapSize;

        MoveField(_isLeft);
    }
    private void MoveMarket()
    {
        CheckDirection();

        _currentMap = 5;
        _cameraController.MapSize = new Vector2(59.5f, _cameraController.MapSize.y);

        MoveField(_isLeft);
    }

    private void MoveMermaidForest()
    {
        CheckDirection();

        _currentMap = 6;
        _cameraController.MapSize = _mapSize;

        MoveField(_isLeft);
    }


    private void MoveField(bool isLeft)
    {
        Vector3 targetPos = new Vector3(_targetTransform[_currentMap].position.x, _targetTransform[_currentMap].position.y, Camera.main.transform.position.z);

        FadeInOutManager.Instance.FadeIn(_fadeTime, () =>
        {
            TimeManager.Instance.CheckMap();

            _cameraController.MapCenter = _targetTransform[_currentMap].position;
            //Camera.main.transform.position = targetPos;

            // �ٸ� ������ �̵��� �� ���� �������� �̵��ϴ� ��ó�� ���̰� ��
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
        });
    }

}
