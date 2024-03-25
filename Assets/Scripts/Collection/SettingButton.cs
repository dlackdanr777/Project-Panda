using UnityEngine;
using Muks.Tween;
using UnityEngine.UI;

public class SettingButton : MonoBehaviour
{
    [Tooltip("UI가 따라다닐 오브젝트")]
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Vector2 _minSize = new Vector2(0, 0);
    [SerializeField] string _targetTransformName; // 비워놓기 가능
    private Image _image;
    private Vector2 _size;
    private bool _isRunningTween;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        _size = transform.localScale;
        _isRunningTween = false;

        // 이름이 null이 아니면 target 찾기
        if(string.IsNullOrEmpty(_targetTransformName) == false)
        {
            _targetTransform = GameObject.Find(_targetTransformName).transform.GetComponent<Transform>();
        }
        if (GetComponent<Image>() != null)
        {
            _image = GetComponent<Image>();
        }
    }

    private void Update()
    {

        if (_targetTransform == null && string.IsNullOrEmpty(_targetTransformName) == false)
        {
            _targetTransform = GameObject.Find(_targetTransformName).transform.GetComponent<Transform>();
        }

        if (_image != null && _targetTransform.gameObject.activeSelf == false)
        {
            _image.enabled = false;
        }
        else if(_image != null)
        {
            _image.enabled = true;
        }

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
