using UnityEngine;
using Muks.Tween;
using UnityEngine.UI;
using Unity.VisualScripting;

public class SettingButton : MonoBehaviour
{
    [Tooltip("UI가 따라다닐 오브젝트")]
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Vector2 _minSize = new Vector2(0, 0);
    [SerializeField] string _targetTransformName; // 비워놓기 가능
    private Vector2 _size;
    private Camera _camera;
    private bool _isRunningTween;
    private bool _isDestroy;
    private QuestMarkSetting _questMarkSetting;

    private void Awake()
    {
        _camera = Camera.main;
        _size = transform.localScale;
        _isRunningTween = false;

        // 이름이 null이 아니면 target 찾기
        if (string.IsNullOrEmpty(_targetTransformName) == false)
        {
            _targetTransform = GameObject.Find(_targetTransformName).transform.GetComponent<Transform>();
        }
        if (gameObject.name.StartsWith("NPC"))
        {
            gameObject.SetActive(false);
            _questMarkSetting = _targetTransform.gameObject.GetComponent<QuestMarkSetting>();
            _questMarkSetting.OnEnableQuestMarkHandler += ButtonSetTure;
            _questMarkSetting.OnDisableQuestMarkHandler += ButtonSetFalse;
        }
    }

    private void OnDestroy()
    {
        _isDestroy = true;
        if (gameObject.name.StartsWith("NPC"))
        {
            _questMarkSetting.OnEnableQuestMarkHandler -= ButtonSetTure;
            _questMarkSetting.OnDisableQuestMarkHandler -= ButtonSetFalse;
        }
    }

    private void Update()
    {
        if (_targetTransform == null && string.IsNullOrEmpty(_targetTransformName) == false)
        {
            _targetTransform = GameObject.Find(_targetTransformName).transform.GetComponent<Transform>();
        }
        transform.position = _camera.WorldToScreenPoint(_targetTransform.position);

        // 카메라 시야 안에 있는지 확인
        Vector3 viewportPosition = _camera.WorldToViewportPoint(_targetTransform.position);
        bool isInCameraRange = viewportPosition.x > 0 && viewportPosition.x < 1 && viewportPosition.y > 0 && viewportPosition.y < 1;

        // 카메라 시야 안에 있다면 버튼 원래 크기로 변경
        if (isInCameraRange && !_isRunningTween)
        {
            _isRunningTween = true;
            Tween.TransformScale(gameObject, _size, 0.15f, TweenMode.Constant, () =>
            {
                _isRunningTween = false;
            });
        }
        // 카메라 시야 밖이라면 버튼 크기 줄이기
        else if (!_isRunningTween)
        {
            _isRunningTween = true;
            Tween.TransformScale(gameObject, _minSize, 0.15f, TweenMode.Constant, () =>
            {
                _isRunningTween = false;
            });
        }
    }

    private void ButtonSetTure()
    {
        gameObject.SetActive(true);
    }

    private void ButtonSetFalse()
    {
        if(_isDestroy) return;
        gameObject.SetActive(false);
    }
}
