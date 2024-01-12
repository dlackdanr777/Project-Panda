using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class UICutoutMask : Image
{

    protected override void Start()
    {
        base.Start();
        StartCoroutine(Fix());
    }

    private IEnumerator Fix()
    {
        yield return null;
        maskable = false;
        maskable = true;
    }

    public override Material materialForRendering
    {
        get
        {
            Material material = new Material(base.materialForRendering);
            material.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            return material;
        }
    }
}
 