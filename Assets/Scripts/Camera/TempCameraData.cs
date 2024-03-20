using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>메인씬에서 다른씬으로 전환시 카메라 데이터를 저장해두는 클래스</summary>
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
