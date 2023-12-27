using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour, IInteraction
{
    // Ư�� ���� �������� ä���� ������ �� �ð� ���� �����ϰ� ��¦�̸� ���� �˷���
    private float _time;
    private float _spawnTime = 10f; // ä�� ������ �ð� ���� - 10�ʸ��� ä�� ����
    private bool _isStartCollection = false;

    [SerializeField] private CollectionButton _collectionButton;


    private void Start()
    {
        _time = 10;
    }

    private void Update()
    {
        if (_time < _spawnTime)
        {
            _time += Time.deltaTime;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
        }

        // ä�� ���̸� ī�޶� ���
        if(_collectionButton.enabled && _collectionButton.IsCollection)
        {
            if (!_isStartCollection)
            {
                _isStartCollection = true;
                GetComponent<SpriteRenderer>().enabled = false;
            }
            _collectionButton.CameraLock();
        }
    }

    public void StartInteraction()
    {
        // ä�� ���� ������ ����
        GetComponent<BoxCollider2D>().enabled = false;
        _collectionButton.gameObject.SetActive(true);
    }

    public void UpdateInteraction()
    {

    }

    public void ExitInteraction()
    {

    }
}
