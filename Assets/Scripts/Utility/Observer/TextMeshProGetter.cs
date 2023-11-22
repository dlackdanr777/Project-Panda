using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Muks.DataBind
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextMeshProGetter : MonoBehaviour
    {
        [SerializeField] private string _dataID;
        private TextMeshProUGUI _text;
        private BindData<string> _data;

        private void Awake()
        {
            if (!TryGetComponent(out _text))
            {
                Debug.LogErrorFormat("{0}에 연결될 컴포넌트가 존재하지 않습니다.", gameObject.name);
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

        public void UpdateText(string text)
        {
            _text.text = text;
        }

        private void Enabled()
        {
            _data = DataBind.GetTextValue(_dataID);
            _text.text = _data.Item;
            _data.CallBack += UpdateText;
        }

        private void Disabled()
        {
            _data.CallBack -= UpdateText;
        }
    }
}

