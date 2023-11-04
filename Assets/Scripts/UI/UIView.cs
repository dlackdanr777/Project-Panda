using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum VisibleState
{
    Disappeared, // 사라짐
    Disappearing, //사라지는 중
    Appeared, //나타남
    Appearing, //나타나는중
}

public abstract class UIView : MonoBehaviour
{


    /// <summary>
    /// 이것이 Appeared일때 Hide실행 가능,
    /// 이것이 Disappeared일때 Show실행 가능
    /// </summary>
    public VisibleState VisibleState;

    /// <summary>
    /// UI를 불러낼때 콜백되는 함수
    /// </summary>
    public abstract void Show();


    /// <summary>
    /// UI를 끌때 콜백되는 함수
    /// </summary>
    public abstract void Hide();
}
