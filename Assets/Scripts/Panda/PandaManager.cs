using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandaManager
{
    /// <summary>
    /// ��� �Ǵ� ������</summary>
    private Dictionary<int, PandaData> _pandaDic;


    /// <summary>
    /// ��� �Ǵ� �̹��� ������</summary>
    private Dictionary<int, PandaStateImage> _pandaImageDic;
    private Parser _parser = new Parser();

    public void Register()
    {
        _pandaDic = _parser.PandaParse("Panda");
        _pandaImageDic = new Dictionary<int, PandaStateImage>();

        // �Ǵ� �̹��� ����
        for(int i = 0; i < _pandaDic.Count; i++)
        {
            _pandaImageDic.Add(_pandaDic[i].PandaID, DatabaseManager.Instance.PandaImage.PandaImages[_pandaDic[i].PandaID]);
            _pandaDic[i].CurrrentImage = _pandaImageDic[i].NomalImage; // �Ǵ� ó�� �̹����� �Ϲ� ���·� ����
        }
    }

    /// <summary>
    /// �Ǵ� ID�� �Ǵ� ã�� PandaData ��ȯ</summary>
    public PandaData GetPandaData(int pandaID)
    {
        PandaData findPandaData = _pandaDic[pandaID];
        return findPandaData;
    }

    /// <summary>
    /// �Ǵ� ID�� �Ǵ� ã�� PandaImage ��ȯ</summary>
    public PandaStateImage GetPandaImage(int pandaID)
    {
        PandaStateImage findPandaImage = _pandaImageDic[pandaID];
        return findPandaImage;
    }

    public void UpdatePandaIntimacy(int pandaID, float intimacy)
    {
        _pandaDic[pandaID].Intimacy = intimacy;
    }
    public void UpdatePandaHappiness(int pandaID, float happiness)
    {
        _pandaDic[pandaID].Happiness = happiness;
    }

    /// <summary>
    /// ��Ÿ�� �Ǵ� mbti ����</summary>
    public void SetStarterMBTI(string mbti)
    {
        _pandaDic[0].MBTI = mbti;
    }
}
