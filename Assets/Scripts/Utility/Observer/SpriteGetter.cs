using TMPro;
using Unity.VisualScripting;
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
            if (string.IsNullOrEmpty(_dataID))
            {
                Debug.LogErrorFormat("Invalid text data ID. {0}", gameObject.name);
                enabled = false;
            }

            switch (_type)
            {
                case GetterType.Image:
                    _image = GetComponent<Image>();
                    break;

                case GetterType.SpriteRenderer:
                    _spriteRenderer = GetComponent<SpriteRenderer>();
                    break;
            }

            _data = DataBind.GetSpriteBindData(_dataID);
            _data.CallBack += UpdateSprite;  
        }

        private void OnEnable()
        {
            switch (_type)
            {
                case GetterType.Image:
                    _image.sprite = _data.Item;
                    break;

                case GetterType.SpriteRenderer:
                    _spriteRenderer.sprite = _data.Item;
                    break;
            }

        }


        private void OnDestroy()
        {
            _data.CallBack -= UpdateSprite;
            _spriteRenderer = null;
            _image = null;
            _data = null;
        }


        private void UpdateSprite(Sprite sprite)
        {
            if (!enabled)
                return;

            switch (_type)
            {
                case GetterType.Image:
                    _image.sprite = sprite;
                    break;

                case GetterType.SpriteRenderer:
                    _spriteRenderer.sprite = sprite;
                    break;
            }
        }
    }
}

