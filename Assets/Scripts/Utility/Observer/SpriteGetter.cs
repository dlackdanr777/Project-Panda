using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Muks.DataBind
{
    public class SpriteGetter : MonoBehaviour
    {
        public enum GetterType
        {
            Image,
            SpriteRenderer
        }

        [SerializeField] private GetterType _type;
        [SerializeField] private string _dataID;

        private Image _image;
        private SpriteRenderer _spriteRenderer;
        private BindData<Sprite> _data;

        private void Awake()
        {
            if(_type == GetterType.Image)
            {
                if (!TryGetComponent(out _image))
                {
                    Debug.LogErrorFormat("{0}에 연결될 컴포넌트가 존재하지 않습니다.", gameObject.name);
                    return;
                }
            }

            else if(_type == GetterType.SpriteRenderer)
            {
                if (!TryGetComponent(out _spriteRenderer))
                {
                    Debug.LogErrorFormat("{0}에 연결될 컴포넌트가 존재하지 않습니다.", gameObject.name);
                    return;
                }
            }

            if (string.IsNullOrEmpty(_dataID))
            {
                Debug.LogWarningFormat("Invalid text data ID. {0}", gameObject.name);
                _dataID = gameObject.name;
            }
        }
        private void OnEnable()
        {
            Invoke("Enabled", 0.02f);
        }

        private void OnDisable()
        {
            Invoke("Disabled", 0.02f);
        }

        public void UpdateImage(Sprite sprite)
        {
            if (_type == GetterType.Image)
                _image.sprite = sprite;
            else if (_type == GetterType.SpriteRenderer)
                _spriteRenderer.sprite = sprite;
        }

        private void Enabled()
        {
            _data = DataBind.GetSpriteValue(_dataID);

            if (_type == GetterType.Image)
                _image.sprite = _data.Item;
            else if(_type == GetterType.SpriteRenderer)
                _spriteRenderer.sprite = _data.Item;

            _data.CallBack += UpdateImage;
        }

        private void Disabled()
        {
            _data.CallBack -= UpdateImage;
        }
    }
}

