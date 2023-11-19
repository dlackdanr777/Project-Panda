using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// NPC, ��Ÿ�� �Ǵ��� ���� ����� ��Ƴ��� Ŭ����
/// ���� ����, ģ�е��� �߻� �Լ��� �����Ͽ� NPC, ��Ÿ�� �Ǵ� ���� ��üȭ�� �� �ֵ��� �ؾ���
/// </summary>
public abstract class Panda : MonoBehaviour
{
    public string Nature { get; set; } // ����
    public string State; // ����
    public float Intimacy { get; set; } // ģ�е�
    public Sprite Image;



    public abstract void AddIntimacy();
    public abstract void SubIntimacy();
}
