using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandaManager : SingletonHandler<PandaManager>
{
    /// <summary>
    /// 모든 판다 데이터</summary>
    private Dictionary<int, PandaData> _pandaDic;
    [SerializeField]
    private PandaImage _pandaImage;
    /// <summary>
    /// 모든 판다 이미지 데이터</summary>
    private Dictionary<int, PandaStateImage> _pandaImageDic;
    private DialogueParser _parser = new DialogueParser();

    public override void Awake()
    {
        base.Awake();

        _pandaDic = _parser.PandaParse("Panda");
        _pandaImageDic = new Dictionary<int, PandaStateImage>();
        // 판다 이미지 저장
        for(int i = 0; i < _pandaDic.Count; i++)
        {
            _pandaImageDic.Add(_pandaDic[i].PandaID, _pandaImage.pandaImage[_pandaDic[i].PandaID]);
        }
    }

    /// <summary>
    /// 판다 ID로 판다 찾아 PandaData 반환</summary>
    public PandaData GetPandaData(int pandaID)
    {
        PandaData findPandaData = _pandaDic[pandaID];
        return findPandaData;
    }

    /// <summary>
    /// 판다 ID로 판다 찾아 PandaImage 반환</summary>
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
    /// 스타터 판다 mbti 설정</summary>
    public void SetStarterMBTI(string mbti)
    {
        _pandaDic[0].MBTI = mbti;
    }
}
