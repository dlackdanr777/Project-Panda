using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotObject : MonoBehaviour
{

    [Tooltip("�����ͺ��̽��� ����� id�� �Է�")]
    [SerializeField] private int _id;

    [Tooltip("������ �̸�")]
    [SerializeField] private string _name;

    [Tooltip("������� ��/��")]
    [SerializeField] private bool _isRegistered;
}
