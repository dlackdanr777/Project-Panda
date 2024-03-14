using UnityEngine;
using Muks.Tween;

public class SettingButton : MonoBehaviour
{
    [Tooltip("UI가 따라다닐 오브젝트")]
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Vector2 _minSize = new Vector2(0, 0);
    private Vector2 _size;
    private bool _isRunningTween;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        _size = transform.localScale;
        _isRunningTween = false;
    }

    private void Update()
    {
        transform.position = _camera.WorldToScreenPoint(_targetTransform.position);

        // 카메라 시야 안에 있는지 확인
        Vector3 viewportPosition = _camera.WorldToViewportPoint(_targetTransform.position);
        bool isInCameraRange = viewportPosition.x > 0 && viewportPosition.x < 1 && viewportPosition.y > 0 && viewportPosition.y < 1;


        if (isInCameraRange && !_isRunningTween)
        {
            _isRunningTween = true;
            Tween.TransformScale(gameObject, _size, 0.15f, TweenMode.Constant, () =>
            {
                _isRunningTween = false;
            });

            //Tween.TransformScale
        }
        else if (!_isRunningTween)
        {
            _isRunningTween = true;
            Tween.TransformScale(gameObject, _minSize, 0.15f, TweenMode.Constant, () =>
            {
                _isRunningTween = false;
            });
        }
    }
}
