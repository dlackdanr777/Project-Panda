using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muks.Tween;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Tween.Move(gameObject, new Vector3(0, 5, 0), 3, TweenMode.Back);
/*        Tween.Move(gameObject, new Vector3(0, 5, 0), 3, TweenMode.Spike);
        Tween.Move(gameObject, new Vector3(0, 5, 0), 3, TweenMode.Spike);

        Tween.Scale(gameObject, new Vector3(5, 5, 5), 3, TweenMode.Spike);
        Tween.Scale(gameObject, new Vector3(8, 8, 8), 3, TweenMode.Spike);
        Tween.Scale(gameObject, new Vector3(2, 2, 2), 3, TweenMode.Spike);*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
