using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTween : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Muks.Tween.Tween.TransformRotate(this.gameObject, new Vector3(0, 360, 0), 5, TweenMode.Smoothstep).Repeat(3);
    }
}
