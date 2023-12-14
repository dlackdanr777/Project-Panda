using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]
public class DynamicColliderFromImage : MonoBehaviour
{
    void Awake()
    {
        // �̹����� ũ�⿡ �´� Collider ����
        CreateColliderFromImage();
    }

    void CreateColliderFromImage()
    {
        // SpriteRenderer ������Ʈ ��������
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found!");
            return;
        }

        // �̹����� �ؽ�ó ��������
        Texture2D texture = spriteRenderer.sprite.texture;

        // �ؽ�ó ũ�� ���
        Vector2 imageSize = new Vector2(texture.width, texture.height) / spriteRenderer.sprite.pixelsPerUnit;

        // BoxCollider2D ������Ʈ ��������
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();

        if (boxCollider == null)
        {
            Debug.LogError("BoxCollider2D component not found!");
            return;
        }

        // �ݶ��̴��� ũ�� ����
        boxCollider.size = imageSize;
    }
}