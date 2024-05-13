using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIScrollbarEventTrigger : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Scrollbar _scrollbar;
    [SerializeField] private EventTrigger _eventTrigger;

    [Header("Option")]
    [SerializeField] private bool _isUpButton;
    [SerializeField][Range(0, 10)] private float _sensitivity;
    [SerializeField][Range(0.1f, 10)] private float _smoothStopTime;


    private bool _isButtonClicked;
    private float _moveSpeed;

    private void Start()
    {
        EventTrigger.Entry buttonDownEvent = new EventTrigger.Entry();
        buttonDownEvent.eventID = EventTriggerType.PointerDown;
        buttonDownEvent.callback.AddListener((eventData) => OnButtonDown());
        _eventTrigger.triggers.Add(buttonDownEvent);

        EventTrigger.Entry buttonUpEvent = new EventTrigger.Entry();
        buttonUpEvent.eventID = EventTriggerType.PointerUp;
        buttonUpEvent.callback.AddListener((eventData) => OnButtonUp());
        _eventTrigger.triggers.Add(buttonUpEvent);
    }


    private void Update()
    {
        if(!_isButtonClicked)
            _moveSpeed = Mathf.Clamp(_moveSpeed - _sensitivity * Time.deltaTime / _smoothStopTime, 0, _sensitivity);
        else
            _scrollbar.value = Mathf.Clamp(_scrollbar.value, 0, 1);

        _scrollbar.value = _isUpButton ? _scrollbar.value + _moveSpeed * Time.deltaTime : _scrollbar.value - _moveSpeed * Time.deltaTime;


    }


    private void OnButtonDown()
    {
        _isButtonClicked = true;
        _moveSpeed = _sensitivity;
    }

    private void OnButtonUp()
    {
        _isButtonClicked = false;
    }
}
