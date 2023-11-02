using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotObject : MonoBehaviour
{

    [Tooltip("데이터베이스에 저장된 id를 입력")]
    [SerializeField] private int _id;

    [Tooltip("아이템 이름")]
    [SerializeField] private string _name;

    [Tooltip("도감등록 유/무")]
    [SerializeField] private bool _isRegistered;
}
