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

        // 이미지의 텍스처 가져오기
        Texture2D texture = spriteRenderer.sprite.texture;

        // 텍스처 크기 얻기
        Vector2 imageSize = new Vector2(texture.width, texture.height) / spriteRenderer.sprite.pixelsPerUnit;

        // BoxCollider2D 컴포넌트 가져오기
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();

        if (boxCollider == null)
        {
            Debug.LogError("BoxCollider2D component not found!");
            return;
        }

        // 콜라이더의 크기 설정
        boxCollider.size = imageSize;
    }
}