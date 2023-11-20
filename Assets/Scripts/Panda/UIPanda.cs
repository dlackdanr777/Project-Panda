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
    public Sprite[] _stateSprite = new Sprite[4]; //���� �̹���

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
