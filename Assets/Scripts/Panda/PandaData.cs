using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandaData
{
    [SerializeField]
    public int PandaID { get; private set; }

    public string PandaName { get; private set; }
    public string MBTI { get; set; }

    public float Intimacy;
    public float Happiness;
    public Sprite CurrrentImage;


    public PandaData(int pandaID, string pandaName, string mbti, float intermacy, float happiness, Sprite currrentImage = null)
    {
        PandaID = pandaID;
        PandaName = pandaName;
        MBTI = mbti;
        Intimacy = intermacy;
        Happiness = happiness;
        CurrrentImage = currrentImage;
    }
}
