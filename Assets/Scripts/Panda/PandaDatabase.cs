using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandaDatabase : SingletonHandler<PandaDatabase>
{
    public DataList<PandaData> Pandas;

    public override void Awake()
    {
        base.Awake();
        Pandas = new DataList<PandaData>();
    }
}
