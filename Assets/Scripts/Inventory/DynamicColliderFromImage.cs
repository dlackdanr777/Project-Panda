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

        // �̹����� ũ�� ���
        Vector2 imageSize = spriteRenderer.bounds.size;

        // BoxCollider ������Ʈ �߰�
        BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();

        // �ݶ��̴��� ũ�� ����
        boxCollider.size = imageSize;
    }
}