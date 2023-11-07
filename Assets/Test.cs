using Muks.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        Tween.Move(gameObject, new Vector3(0, 3, 0), 2, TweenMode.EaseInOutBack);
    }
}
