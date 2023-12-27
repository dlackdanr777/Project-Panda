using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour, IInteraction
{
    // 특정 스폰 지역에서 채집이 가능할 때 시간 조절 가능하게 반짝이를 통해 알려줌
    private float _time;
    private float _spawnTime = 10f; // 채집 가능한 시간 간격 - 10초마다 채집 가능
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

        // 채집 중이면 카메라 잠금
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
        // 채집 가능 아이콘 생성
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
