using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]
public class DynamicColliderFromImage : MonoBehaviour
{
    void Awake()
    {
        // 이미지의 크기에 맞는 Collider 생성
        CreateColliderFromImage();
    }

    void CreateColliderFromImage()
    {
        // SpriteRenderer 컴포넌트 가져오기
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found!");
            return;
        }

        // 이미지의 크기 얻기
        Vector2 imageSize = spriteRenderer.bounds.size;

        // BoxCollider 컴포넌트 추가
        BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();

        // 콜라이더의 크기 설정
        boxCollider.size = imageSize;
    }
}