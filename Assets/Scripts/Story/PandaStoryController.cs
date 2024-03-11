using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class StoryEventData
{
    [Tooltip("몇번 째 대화 창에 이벤트가 들어가는 것인지?")]
    [SerializeField] private int _insertIndex;

    [SerializeField] private StoryEvent _storyEvent;

    public int InsertIndex => _insertIndex;
    public StoryEvent StoryEvent => _storyEvent;

}


public class PandaStoryController : MonoBehaviour
{
    [SerializeField] private string _storyID;

    [SerializeField] private StoryEventData[] _storyEvents;

    [SerializeField] private Sprite _buttonImage;

    [Tooltip("버튼이 현재 클래스 오브젝트에서 얼마만큼 떨어져있어야 하는지")]
    [SerializeField] private Vector3 _buttonPos;

    private FollowButton _followButton;
    public FollowButton FollowButton => _followButton;

    public StoryDialogue StoryDialogue { get; private set; }

    public static event Action<PandaStoryController, StoryDialogue, StoryEventData[]> OnStartInteractionHandler;

    public static  Action<string, PandaStoryController> OnStartHandler;

    public static  Action<PandaStoryController> OnCheckActivateHandler;


    private void Start()
    {
        StoryDialogue = DatabaseManager.Instance.DialogueDatabase.GetStoryDialogue(_storyID);
        OnStartHandler?.Invoke(_storyID, this);

        Transform parent = GameObject.Find("Follow Button Parent").transform;
        FollowButton followButtonPrefab = Resources.Load<FollowButton>("Button/Follow Button");

        _followButton = Instantiate(followButtonPrefab, transform.position + Vector3.up, Quaternion.identity, parent);
        _followButton.Init(transform, _buttonPos, new Vector2(120, 120), _buttonImage, () => OnStartInteractionHandler?.Invoke(this, StoryDialogue, _storyEvents));
        _followButton.gameObject.SetActive(gameObject.activeSelf);

        OnCheckActivateHandler?.Invoke(this);
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
