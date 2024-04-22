using System.Collections.Generic;
using UnityEngine;


namespace Muks.OcclusionCulling2D
{
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
        public List<ObjectSettings> _objectSettingList;
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


        public void GenerateCullingList()
        {
            if(_objectSettingList == null)
                _objectSettingList = new List<ObjectSettings>();

            _objectSettingList.Clear();

            _allSpriteRenderers = FindObjectsOfType<SpriteRenderer>();

            foreach (SpriteRenderer renderer in _allSpriteRenderers)
            {
                ObjectSettings objSet = new ObjectSettings();
                objSet.SpriteRenderer = renderer;

                objSet.Size = ((renderer.sprite.rect.size * renderer.transform.lossyScale) / renderer.sprite.pixelsPerUnit) * 1.1f;
                objSet.Center = renderer.transform.position;

                objSet.TopRight = new Vector2(objSet.Center.x + objSet.Size.x  * 0.5f, objSet.Center.y + objSet.Size.y * 0.5f);
                objSet.TopLeft = new Vector2(objSet.Center.x - objSet.Size.x * 0.5f, objSet.Center.y + objSet.Size.y * 0.5f);
                objSet.BottomLeft = new Vector2(objSet.Center.x - objSet.Size.x * 0.5f, objSet.Center.y - objSet.Size.y * 0.5f);
                objSet.BottomRight = new Vector2(objSet.Center.x + objSet.Size.x * 0.5f, objSet.Center.y - objSet.Size.y * 0.5f);

                objSet.Right = objSet.Center.x + objSet.Size.x * 0.5f;
                objSet.Left = objSet.Center.x - objSet.Size.x * 0.5f;
                objSet.Top = objSet.Center.y + objSet.Size.y * 0.5f;
                objSet.Bottom = objSet.Center.y - objSet.Size.y * 0.5f;

                _objectSettingList.Add(objSet);
            }
        }


        public void Clear()
        {
            _objectSettingList.Clear();
        }



        private void OnDrawGizmosSelected()
        {
            if (_objectSettingList == null)
                return;

            Gizmos.color = Color.green;

            foreach (var o in _objectSettingList)
            {
                Gizmos.DrawLine(o.TopRight, o.TopLeft);
                Gizmos.DrawLine(o.TopLeft, o.BottomLeft);
                Gizmos.DrawLine(o.BottomLeft, o.BottomRight);
                Gizmos.DrawLine(o.BottomRight, o.TopRight);
            }
        }
    }
}



