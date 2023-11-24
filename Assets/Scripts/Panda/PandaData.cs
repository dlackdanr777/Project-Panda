using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PandaData
{
    [SerializeField]
    private string _pandaID;
    public string PandaID => _pandaID;

    [SerializeField]
    private float _intimacy;
    public float Intimacy => _intimacy;

    [SerializeField]
    private float _happiness;
    public float Happiness => _happiness;

    public PandaData(string pandaID, float intermacy, float happiness)
    {
        _pandaID = pandaID;
        _intimacy = intermacy;
        _happiness = happiness;
    }
}
