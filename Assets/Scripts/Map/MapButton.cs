using UnityEngine;
using UnityEngine.UI;
using Muks.Tween;
using Muks.DataBind;

public class MapButton : MonoBehaviour
{
    [Tooltip("메인 카메라 연결")]
    [SerializeField] private CameraController _cameraController;

    [SerializeField] private GameObject _fadeInOut;
    [SerializeField] private Transform[] _targetTransform;// = new Transform[DatabaseManager.Instance.GetMapDic().Count];
    private float _fadeTime = 1f;
    private int _currentMap;

    private void Awake()
    {
        DataBind.SetButtonValue("WishTreeButton", MoveWishTree);
        DataBind.SetButtonValue("FishingGroundButton", MoveFishingGround);
        DataBind.SetButtonValue("ForestEntranceButton", MoveForestEntrance);
        DataBind.SetButtonValue("ForestButton", MoveForest);
        DataBind.SetButtonValue("VillageButton", MoveVillage);
        DataBind.SetButtonValue("MarketButton", MoveMarket);
    }

    public void MoveWishTree()
    {
        _currentMap = 0;

        MoveField();
    }
    private void MoveFishingGround()
    {
        _currentMap = 1;

        MoveField();
    }

    private void MoveForestEntrance()
    {
        _currentMap = 2;

        MoveField();
    }
    private void MoveForest()
    {
        _currentMap = 3;

        MoveField();
    }
    private void MoveVillage()
    {
        _currentMap = 4;

        MoveField();
    }
    private void MoveMarket()
    {
        _currentMap = 5;

        MoveField();
    }


    private void MoveField()
    {
        Vector3 targetPos = new Vector3(_targetTransform[_currentMap].position.x, _targetTransform[_currentMap].position.y, Camera.main.transform.position.z); 

        _fadeInOut.gameObject.SetActive(true);
        Tween.IamgeAlpha(_fadeInOut.gameObject, 1, _fadeTime, TweenMode.Constant, () =>
        {
            TimeManager.Instance.CheckTime();
            _cameraController.MapCenter = _targetTransform[_currentMap].position;
            //Camera.main.transform.position = targetPos;
            Camera.main.transform.position = targetPos + new Vector3(_cameraController.MapSize.x, 0, 0);
            Tween.IamgeAlpha(_fadeInOut.gameObject, 0, _fadeTime, TweenMode.Quadratic, () =>
            {
                _fadeInOut.gameObject.SetActive(false);
            });
        });
    }

}
