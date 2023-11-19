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
    public Sprite[] _stateSprite = new Sprite[4]; //���� �̹���

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

    // ���� �̹��� ����
    private void StateData_StateHandler(int currentPandaState)
    {
        OnChangeStateImage(currentPandaState);
    }

    private void OnClickStateButton()
    {
        // ����â ǥ�� �߰�
        Debug.Log("����â ǥ��");
    }
    private void OnClickGiftButton()
    {
        // �κ��丮â ���� �߰�
        Debug.Log("�κ��丮 ����");
    }
    private void OnClickMessageButton()
    {
        // ä�� �� ���� �߰�
        Debug.Log("ä�� �� ����");
    }
    private void OnChangeStateImage(int currentPandaState)
    {
        //(����) DataID �ٲٱ�
        DataBind.SetSpriteValue("941", _stateSprite[currentPandaState]);
    }

}
