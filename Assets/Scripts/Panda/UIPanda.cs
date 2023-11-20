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
    public Sprite[] _stateSprite = new Sprite[4]; //상태 이미지

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
        OnChangeStateImage(0); // 행복 상태로 시작

        _stateButton.onClick.AddListener(OnClickStateButton);
        _cameraButton.onClick.AddListener(OnClickCameraButton);
    }

    private void OnDisable()
    {
        _starterPanda.stateData.StateHandler -= StateData_StateHandler;
    }

    // 상태 이미지 변경
    private void StateData_StateHandler(int currentPandaState)
    {
        Debug.Log("상태 이미지 변경");
        OnChangeStateImage(currentPandaState);
    }

    private void OnClickStateButton()
    {
        // 상태창 표시 추가
        Debug.Log("상태창 표시");
    }
    private void OnClickCameraButton()
    {
        // 카메라와 연동
        Debug.Log("카메라 실행");
    } 

    private void OnChangeStateImage(int currentPandaState)
    {
        //(수정) DataID 바꾸기
        DataBind.SetSpriteValue("941", _stateSprite[currentPandaState]);
    }

}
