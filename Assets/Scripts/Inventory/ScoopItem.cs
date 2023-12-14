using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScoopItem : MonoBehaviour
{
    public static Action<GameObject> OnScoop = delegate { };

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 위치를 스크린 좌표로 가져옴
            Vector3 mousePosition = Input.mousePosition;

            // 스크린 좌표를 레이로 변환
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            // 레이캐스트 수행
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, transform.forward);

            if(hit.transform != null)
            {
                // 충돌한 오브젝트 처리
                HandleHitObject(hit.collider.gameObject);

            }
            
        }
    }
    void HandleHitObject(GameObject hitObject)
    {
        BoxCollider2D hitCollider = hitObject.GetComponent<BoxCollider2D>();
        // 여기서 충돌한 오브젝트에 대한 처리를 수행
        if(hitCollider.CompareTag("Item"))
        {
            OnScoop?.Invoke(hitObject);
        }
    }

}
