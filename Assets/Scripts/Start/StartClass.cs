using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StartClass : MonoBehaviour
{
    public abstract void Init(StartClassController startClass); 
    public abstract void UIStart();
    public abstract void UIUpdate();
    public abstract void UIEnd();
}
