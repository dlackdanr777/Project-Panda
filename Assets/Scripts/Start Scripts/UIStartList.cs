using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIStartList : MonoBehaviour
{
    public abstract void Init(UIStart uiStart); 
    public abstract void UIStart();
    public abstract void UIUpdate();
    public abstract void UIEnd();
}
