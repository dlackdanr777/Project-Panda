using Muks.DataBind;
using UnityEngine;
using UnityEngine.UI;
using Muks;
using Muks.Tween;

public class UIPanda : MonoBehaviour
{
    private Button _stateButton;
    private Button _giftButton;
    private Button _messageButton;

    [SerializeField]
    private Image _stateImage;
    [SerializeField]
    public Sprite[] _stateSprite = new Sprite[4]; //상태 이미지

    [SerializeField]
    private StarterPanda _starterPanda;

    private void Awake()
    {
        _stateButton = transform.GetChild(0).gameObject.GetComponent<Button>();
        _giftButton = transform.GetChild(1).gameObject.GetComponent<Button>();
        _messageButton = transform.GetChild(2).gameObject.GetComponent<Button>();
    }
    private void OnEnable()
    {
        _starterPanda.stateData.StateHandler += StateData_StateHandler;

        _stateButton.onClick.AddListener(OnClickStateButton);
        _giftButton.onClick.AddListener(OnClickGiftButton);
        _messageButton.onClick.AddListener(OnClickMessageButton);
    }

    private void OnDisable()
    {
        _starterPanda.stateData.StateHandler -= StateData_StateHandler;
    }

    // 상태 이미지 변경
    private void StateData_StateHandler(int currentPandaState)
    {
        OnChangeStateImage(currentPandaState);
    }

    private void OnClickStateButton()
    {
        // 상태창 표시 추가
        Debug.Log("상태창 표시");
    }
    private void OnClickGiftButton()
    {
        // 인벤토리창 열기 추가
        Debug.Log("인벤토리 열기");
    }
    private void OnClickMessageButton()
    {
        // 채팅 앱 열기 추가
        Debug.Log("채팅 앱 열기");
    }
    private void OnChangeStateImage(int currentPandaState)
    {
        //(수정) DataID 바꾸기
        DataBind.SetSpriteValue("941", _stateSprite[currentPandaState]);
    }

}
