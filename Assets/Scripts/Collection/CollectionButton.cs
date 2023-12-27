using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muks.Tween;
using BT;
using System;

public class CollectionButton : MonoBehaviour, IInteraction
{
    private Animator _collectionAnim;
    [SerializeField] private GameObject _speechBubble;

    public Action<float> OnCollectionButtonClicked;
    public bool IsCollection = false; // ä�� ���ΰ�?
    private float _fadeTime = 1f; // ȭ�� ��ο� �ð�
    private Vector3 _targetPos;

    private Vector3 CollectionPosition = new Vector3(-3.4f, -14f, 0);

    private void Start()
    {
        _collectionAnim = DatabaseManager.Instance.StartPandaInfo.StarterPanda.GetComponent<Animator>();
    }

    public void StartInteraction()
    {
        // ȭ�� FadeOut
        OnCollectionButtonClicked?.Invoke(_fadeTime);
        Invoke("ClickCollectionButton", _fadeTime);
    }

    public void UpdateInteraction()
    {

    }

    public void ExitInteraction()
    {

    }

    private void ClickCollectionButton()
    {
        StarterPanda starterPanda = DatabaseManager.Instance.StartPandaInfo.StarterPanda;

        // ĳ���Ͱ� ä�� ����Ʈ�� �̵�
        starterPanda.gameObject.transform.position = CollectionPosition;

        // ī�޶� ĳ���Ͱ� �߾����� �����ǵ��� �̵�
        _targetPos = new Vector3(starterPanda.transform.position.x, starterPanda.transform.position.y, Camera.main.transform.position.z);
        Camera.main.gameObject.transform.position = _targetPos;

        IsCollection = true;
        GetComponent<SpriteRenderer>().enabled = false;

        // ȭ�� ������ �ð��� ���߾� ä�� ����
        Invoke("StartCollection", _fadeTime);

        
    }

    /// <summary>
    /// ī�޶� �������� ���ϰ� ���� </summary>
    public void CameraLock()
    {
        Camera.main.gameObject.transform.position = _targetPos;
    }

    /// <summary>
    /// ä�� ������ �� ���� </summary>
    private void StartCollection()
    {
        Debug.Log("ä�������Դϴ� ~~");
        // ä�� �ִϸ��̼� �Ǵٿ� ��ǳ�� ����
        _collectionAnim.enabled = true;
        _collectionAnim.SetBool("IsCollecting", true);
        _speechBubble.GetComponent<Animator>().enabled = true;
        // ��ǳ�� ...�� ������ ����ǥ ǥ��
        // ����ǥ ��ġ�ϸ� ä�� ���� ���� ���� �˷���
        // �ִϸ��̼� ���� + �ؽ�Ʈ �����
        // �κ��丮�� �������� ���� ����

        // ä�� ����
        //IsCollection = false;
    }
}
