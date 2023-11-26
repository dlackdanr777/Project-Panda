using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandaData
{
    [SerializeField]
    public string PandaID { get; private set; }

    public string PandaName { get; private set; }
    public string MBTI { get; set; }

    [SerializeField]
    public float Intimacy;

    [SerializeField]
    public float Happiness;

    public PandaData(string pandaID, string pandaName, string mbti, float intermacy, float happiness)
    {
        PandaID = pandaID;
        PandaName = pandaName;
        MBTI = mbti;
        Intimacy = intermacy;
        Happiness = happiness;
    }
}
