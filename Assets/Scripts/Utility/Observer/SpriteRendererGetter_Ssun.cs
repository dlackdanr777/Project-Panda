using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Muks.DataBind
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteRendererGetter_Ssun : MonoBehaviour
    {
        [SerializeField] private string _dataID;
        private SpriteRenderer _image;
        private BindData<Sprite> _data;

        private void Awake()
        {
            if (!TryGetComponent(out _image))
            {
                Debug.LogErrorFormat("{0}�� ����� ������Ʈ�� �������� �ʽ��ϴ�.", gameObject.name);
                return;
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
            _image.sprite = sprite;
        }

        private void Enabled()
        {
            _data = DataBind.GetImageValue(_dataID);
            _image.sprite = _data.Item;
            _data.CallBack += UpdateImage;
        }

        private void Disabled()
        {
            _data.CallBack -= UpdateImage;
        }
    }
}

