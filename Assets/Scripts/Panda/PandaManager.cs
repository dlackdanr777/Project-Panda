using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandaManager : SingletonHandler<PandaManager>
{
    /// <summary>
    /// 모든 판다 데이터
    /// </summary>
    private PandaData[] _pandaDatas;
    private Dictionary<int, PandaData> _pandaDic;

    private DialogueParser _parser = new DialogueParser();

    public override void Awake()
    {
        base.Awake();

        _pandaDic = _parser.PandaParse("Panda");
        Debug.Log("pandaDAta: " + _pandaDatas[0].PandaName);
    }

    /// <summary>
    /// 판다 ID로 판다 찾아 PandaData 반환
    /// </summary>
    public PandaData GetPandaData(string pandaID)
    {
        PandaData findPandaData = _pandaDatas[int.Parse(pandaID)];
        return findPandaData;
    }

    public void UpdatePandaIntimacy(string pandaID, float intimacy)
    {
        _pandaDatas[int.Parse(pandaID)].Intimacy = intimacy;
    }
    public void UpdatePandaHappiness(string pandaID, float happiness)
    {
        _pandaDatas[int.Parse(pandaID)].Happiness = happiness;
    }

    /// <summary>
    /// 스타터 판다 mbti 설정
    /// </summary>
    public void SetStarterMBTI(string mbti)
    {
        _pandaDatas[0].MBTI = mbti;
    }
}
