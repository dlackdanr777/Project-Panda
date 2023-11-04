using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Text))]
public class TextGetter : MonoBehaviour
{
    [SerializeField] private string _dataID;
    private Text _text;
    private TextData _data;

    private void Awake()
    {
        _text = GetComponent<Text>();

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
        _data = DataBinding.GetTextValue(_dataID);
        _text.text = _data.text;
        _data.callback += UpdateText;
    }

    private void Disabled()
    {
        _data.callback -= UpdateText;
    }
}

