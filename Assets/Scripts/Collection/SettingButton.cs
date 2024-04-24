using UnityEngine;
using Muks.Tween;
using UnityEngine.UI;
using Unity.VisualScripting;

public class SettingButton : MonoBehaviour
{
    [Tooltip("UI�� ����ٴ� ������Ʈ")]
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Vector2 _minSize = new Vector2(0, 0);
    [SerializeField] string _targetTransformName; // ������� ����
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

        // �̸��� null�� �ƴϸ� target ã��
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

        // ī�޶� �þ� �ȿ� �ִ��� Ȯ��
        Vector3 viewportPosition = _camera.WorldToViewportPoint(_targetTransform.position);
        bool isInCameraRange = viewportPosition.x > 0 && viewportPosition.x < 1 && viewportPosition.y > 0 && viewportPosition.y < 1;

        // ī�޶� �þ� �ȿ� �ִٸ� ��ư ���� ũ��� ����
        if (isInCameraRange && !_isRunningTween)
        {
            _isRunningTween = true;
            Tween.TransformScale(gameObject, _size, 0.15f, TweenMode.Constant, () =>
            {
                _isRunningTween = false;
            });
        }
        // ī�޶� �þ� ���̶�� ��ư ũ�� ���̱�
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
