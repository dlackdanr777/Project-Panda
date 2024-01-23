using UnityEngine;
using UnityEngine.UI;
using Muks.Tween;
using Muks.DataBind;

public class MapButton : MonoBehaviour
{
    [Tooltip("메인 카메라 연결")]
    [SerializeField] private CameraController _cameraController;

    [SerializeField] private GameObject _fadeInOut;
    [SerializeField] private Transform[] _targetTransform;
    private float _fadeTime = 1f;
    private int _currentMap;

    private void Awake()
    {
        DataBind.SetButtonValue("WishTreeButton", MoveWishTree);
        DataBind.SetButtonValue("ForestButton", MoveForest);
        DataBind.SetButtonValue("FishingGroundButton", MoveFishingGround);
    }

    public void MoveWishTree()
    {
        _currentMap = 0;

        MoveField();
    }

    private void MoveForest()
    {
        _currentMap = 1;

        MoveField();
    }

    private void MoveFishingGround()
    {
        _currentMap = 2;

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
            Camera.main.transform.position = targetPos;
            Tween.IamgeAlpha(_fadeInOut.gameObject, 0, _fadeTime, TweenMode.Quadratic, () =>
            {
                _fadeInOut.gameObject.SetActive(false);
            });
        });
    }

}
