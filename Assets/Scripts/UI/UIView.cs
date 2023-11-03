using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIView : MonoBehaviour
{
    public enum VisibleState
    {
        Disappeared, // �����
        Disappearing, //������� ��
        Appeared, //��Ÿ��
        Appearing, //��Ÿ������
    }

    public VisibleState _visibleState;


    public virtual void Show()
    {
        if(_visibleState == VisibleState.Disappeared)
        {
            gameObject.SetActive(true);
            _visibleState = VisibleState.Appeared;
        }
    }

    public virtual void Hide()
    {
        if(_visibleState == VisibleState.Appeared)
        {
            gameObject.SetActive(false);
            Debug.Log(gameObject.activeSelf);
            _visibleState = VisibleState.Disappeared;
        }
            
    }

    

}
