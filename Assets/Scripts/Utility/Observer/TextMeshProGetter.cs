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
            if (string.IsNullOrEmpty(_dataID))
            {
                Debug.LogErrorFormat("Invalid text data ID. {0}", gameObject.name);
                enabled = false;
            }

            _text = GetComponent<TextMeshProUGUI>();
            _data = DataBind.GetTextBindData(_dataID);
            _data.CallBack += UpdateText;

        }


        private void OnEnable()
        {
            _text.text = _data.Item;
        }


        private void UpdateText(string text)
        {
            if (enabled)
                _text.text = text;
        }


        private void OnDestroy()
        {
            _data.CallBack -= UpdateText;
            _data = null;
            _text = null;
        }
    }
}

