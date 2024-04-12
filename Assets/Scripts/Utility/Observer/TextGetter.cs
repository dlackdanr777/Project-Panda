using UnityEngine;
using UnityEngine.UI;

namespace Muks.DataBind
{
    [RequireComponent(typeof(Text))]
    public class TextGetter : MonoBehaviour
    {
        [SerializeField] private string _dataID;
        private Text _text;
        private BindData<string> _data;


        private void Awake()
        {
            if (string.IsNullOrEmpty(_dataID))
            {
                Debug.LogErrorFormat("Invalid text data ID. {0}", gameObject.name);
                enabled = false;
            }

            _text = GetComponent<Text>();
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

