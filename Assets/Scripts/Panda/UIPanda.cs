using Muks.DataBind;
using UnityEngine;
using UnityEngine.UI;
using Muks;
using Muks.Tween;

public class UIPanda : MonoBehaviour
{
    private Button _stateButton;
    private Button _cameraButton;

    [SerializeField]
    private Image _stateImage;
    [SerializeField]
    public Sprite[] _stateSprite = new Sprite[4]; //���� �̹���

    [SerializeField]
    private GameObject _currentStateImage;

    [SerializeField]
    private StarterPanda _starterPanda;

    private void Awake()
    {
        _stateButton = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Button>();
        _cameraButton = transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<Button>();
    }
    private void OnEnable()
    {
        _starterPanda.stateData.StateHandler += StateData_StateHandler;
        OnChangeStateImage(0); // �ູ ���·� ����

        _stateButton.onClick.AddListener(OnClickStateButton);
        _cameraButton.onClick.AddListener(OnClickCameraButton);
    }

    private void OnDisable()
    {
        _starterPanda.stateData.StateHandler -= StateData_StateHandler;
    }

    // ���� �̹��� ����
    private void StateData_StateHandler(int currentPandaState)
    {
        Debug.Log("���� �̹��� ����");
        OnChangeStateImage(currentPandaState);
    }

    private void OnClickStateButton()
    {
        // ����â ǥ�� �߰�
        Debug.Log("����â ǥ��");
    }
    private void OnClickCameraButton()
    {
        // ī�޶�� ����
        Debug.Log("ī�޶� ����");
    } 

    private void OnChangeStateImage(int currentPandaState)
    {
        //(����) DataID �ٲٱ�
        DataBind.SetSpriteValue("941", _stateSprite[currentPandaState]);
    }

}
