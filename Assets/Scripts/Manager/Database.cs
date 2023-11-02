using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Database : SingletonHandler<Database>
{
    public DataList<PhotoData> Photos;

    public override void Awake()
    {
        base.Awake();
        Photos = new DataList<PhotoData>();
    }
}
