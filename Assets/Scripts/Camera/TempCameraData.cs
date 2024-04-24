using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>���ξ����� �ٸ������� ��ȯ�� ī�޶� �����͸� �����صδ� Ŭ����</summary>
public class TempCameraData
{
    private Vector3 _tmpPos;
    public Vector3 TmpPos => _tmpPos;

    private int _tmpCenterIndex;
    public int TmpCenterIndex => _tmpCenterIndex;

    public TempCameraData()
    {
        _tmpPos = Vector3.zero;
        _tmpCenterIndex = -1;
    }


    public void SaveData(Vector3 pos, int centerIndex)
    {
        _tmpPos = pos;
        _tmpCenterIndex = centerIndex;
    }
}
