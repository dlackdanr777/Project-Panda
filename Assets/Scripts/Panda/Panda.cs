using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// NPC, 스타터 판다의 공통 기능을 모아놓은 클래스
/// 고유 성격, 친밀도를 추상 함수로 설정하여 NPC, 스타터 판다 각자 실체화할 수 있도록 해야함
/// </summary>
public abstract class Panda : MonoBehaviour
{
    public string Nature { get; set; } // 성격
    public string State; // 상태
    public float Intimacy { get; set; } // 친밀도
    public Sprite Image;



    public abstract void AddIntimacy();
    public abstract void SubIntimacy();
}
