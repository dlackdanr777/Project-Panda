using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class OcclusionCulling2D : MonoBehaviour
{
    [System.Serializable]
    public class ObjectSettings
    {
        public SpriteRenderer SpriteRenderer;
        public Vector2 Size;
        public Vector2 Offset;
        public Vector2 Center;
        public Vector2 TopRight;
        public Vector2 TopLeft;
        public Vector2 BottomLeft;
        public Vector2 BottomRight;
        public float Right;
        public float Left;
        public float Top;
        public float Bottom;
    }

    [SerializeField] private float updateRateInSeconds = 0.04f;

    private SpriteRenderer[] _allSpriteRenderers;
    private List<ObjectSettings> _objectSettingList = new List<ObjectSettings>();
    private Camera _camera;
    private float _cameraHalfWidth;
    private float timer;


    void Awake()
    {
        _camera = GetComponent<Camera>();
        _cameraHalfWidth = _camera.orthographicSize * ((float)Screen.width / (float)Screen.height);
    }


    private void Start()
    {
        _allSpriteRenderers = FindObjectsOfType<SpriteRenderer>();

        foreach (SpriteRenderer renderer in _allSpriteRenderers)
        {
            ObjectSettings objSet = new ObjectSettings();
            objSet.SpriteRenderer = renderer;
            Vector3 size = transform.localScale * 0.01f * 1.1f;

            if (objSet.SpriteRenderer.transform.parent != null)
            {
                Transform parent = objSet.SpriteRenderer.transform.parent;
                while (parent != null)
                {
                    size *= parent.transform.localScale.x;
                    parent = parent.parent;
                }
            }

            objSet.Size = renderer.sprite.rect.size * size;
            objSet.Center = renderer.transform.position;

            objSet.TopRight = new Vector2(objSet.Center.x + objSet.Size.x, objSet.Center.y + objSet.Size.y);
            objSet.TopLeft = new Vector2(objSet.Center.x - objSet.Size.x, objSet.Center.y + objSet.Size.y);
            objSet.BottomLeft = new Vector2(objSet.Center.x - objSet.Size.x, objSet.Center.y - objSet.Size.y);
            objSet.BottomRight = new Vector2(objSet.Center.x + objSet.Size.x, objSet.Center.y - objSet.Size.y);

            objSet.Right = objSet.Center.x + objSet.Size.x;
            objSet.Left = objSet.Center.x - objSet.Size.x;
            objSet.Top = objSet.Center.y + objSet.Size.y;
            objSet.Bottom = objSet.Center.y - objSet.Size.y;

            _objectSettingList.Add(objSet);
        }
    }


    void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer > updateRateInSeconds) timer = 0;
        else return;

        float cameraRight = _camera.transform.position.x + _cameraHalfWidth;
        float cameraLeft = _camera.transform.position.x - _cameraHalfWidth;
        float cameraTop = _camera.transform.position.y + _camera.orthographicSize;
        float cameraBottom = _camera.transform.position.y - _camera.orthographicSize;

        foreach (ObjectSettings o in _objectSettingList)
        {
            if (o.SpriteRenderer == null)
                continue;

            bool IsObjectVisibleInCastingCamera = o.Right > cameraLeft & o.Left < cameraRight & // check horizontal
                                                  o.Top > cameraBottom & o.Bottom < cameraTop; // check vertical
            o.SpriteRenderer.enabled = IsObjectVisibleInCastingCamera;
        }
    }
}

