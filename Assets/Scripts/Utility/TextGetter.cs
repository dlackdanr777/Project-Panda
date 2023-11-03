using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Text))]
public class TextGetter : MonoBehaviour
{
    [SerializeField] string _dataID;
    Text _text;
    private TextData _data;

    private void Awake()
    {
        _text = GetComponent<Text>();

        if (string.IsNullOrEmpty(_dataID))
        {
            Debug.LogWarningFormat("Invalid text data ID. {0}", gameObject.name);
            _dataID = gameObject.name;
        }

        _data = UIView.GetValue(_dataID);
        _text.text = _data.text;
        _data.callback += UpdateText;
    }
    
    private void OnDisable()
    {
        _data.callback -= UpdateText;
    }

    public void UpdateText(string text)
    {
        _text.text = text;
    }
}

