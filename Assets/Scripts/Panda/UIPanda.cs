using BT;
using Muks.DataBind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanda : MonoBehaviour
{
    private Button _stateButton;
    private Button _cameraButton;

    [SerializeField]
    public Sprite[] _stateSprite = new Sprite[4]; //상태 이미지

    [SerializeField]
    private StarterPanda _starterPanda;

    private void Awake()
    {
        _stateButton = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Button>();
        _cameraButton = transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<Button>();
    }

    private void OnEnable()
    {
        _starterPanda.StateHandler += StateData_StateHandler;

        _stateButton.onClick.AddListener(OnClickStateButton);
        _cameraButton.onClick.AddListener(OnClickCameraButton);
    }
    private void OnDisable()
    {
        _starterPanda.StateHandler -= StateData_StateHandler;
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
