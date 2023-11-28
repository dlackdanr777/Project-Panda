using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandaManager : SingletonHandler<PandaManager>
{
    /// <summary>
    /// ��� �Ǵ� ������
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
    /// �Ǵ� ID�� �Ǵ� ã�� PandaData ��ȯ
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
    /// ��Ÿ�� �Ǵ� mbti ����
    /// </summary>
    public void SetStarterMBTI(string mbti)
    {
        _pandaDatas[0].MBTI = mbti;
    }
}
