using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed = 0.01f;
    public Transform LeftBound;
    public Transform RightBound;
    private float _leftBound; // 왼쪽 경계
    private float _rightBound; // 오른쪽 경계

    void Start()
    {
        _leftBound = LeftBound.position.x;
        _rightBound = RightBound.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
        if (transform.position.x < _leftBound)
        {
            transform.position = new Vector3(_rightBound, transform.position.y, transform.position.z);
        }
    }
}
