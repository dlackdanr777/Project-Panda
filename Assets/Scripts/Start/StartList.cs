using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StartList : MonoBehaviour
{
    public abstract void Init(StartClassController uiStart); 
    public abstract void UIStart();
    public abstract void UIUpdate();
    public abstract void UIEnd();
}
