using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class StoryEventData
{
    [Tooltip("��� ° ��ȭ â�� �̺�Ʈ�� ���� ������?")]
    [SerializeField] private int _insertIndex;

    [SerializeField] private StoryEvent _storyEvent;

    public int InsertIndex => _insertIndex;
    public StoryEvent StoryEvent => _storyEvent;

}


public class PandaStoryController : MonoBehaviour
{
    [SerializeField] private string _storyID;

    [SerializeField] private StoryEventData[] _storyEvents;

    [SerializeField] private FollowButton _followButtonPrefab;
    private FollowButton _followButton;
    public FollowButton FollowButton => _followButton;

    [SerializeField] private Sprite _buttonImage;

    [Tooltip("��ư�� ���� Ŭ���� ������Ʈ���� �󸶸�ŭ �������־�� �ϴ���")]
    [SerializeField] private Vector3 _buttonPos;

    public StoryDialogue StoryDialogue { get; private set; }

    public static event Action<PandaStoryController, StoryDialogue, StoryEventData[]> OnStartInteractionHandler;

    public static event Action<string, PandaStoryController> OnStartHandler;

    public static event Action<PandaStoryController> OnCheckActivateHandler;


    private void Start()
    {
        StoryDialogue = DatabaseManager.Instance.DialogueDatabase.GetStoryDialogue(_storyID);

        OnStartHandler?.Invoke(_storyID, this);

        OnCheckActivateHandler?.Invoke(this);

        Vector3 targetPos = transform.position + _buttonPos;
        Transform parent = GameObject.Find("Follow Button Parent").transform;
        _followButton = Instantiate(_followButtonPrefab, transform.position + Vector3.up, Quaternion.identity, parent);
        _followButton.Init(gameObject, targetPos, new Vector2(120, 120), _buttonImage, () => OnStartInteractionHandler?.Invoke(this, StoryDialogue, _storyEvents));
        _followButton.gameObject.SetActive(gameObject.activeSelf);
    }
    

    private void OnEnable()
    {
        if (_followButton != null)
            _followButton.gameObject.SetActive(true);
    }


    private void OnDisable()
    {
        if(_followButton != null )
            _followButton.gameObject.SetActive(false);
    }
}
