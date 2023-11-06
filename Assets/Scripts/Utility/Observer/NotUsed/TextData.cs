using System;

public class TextData
{
    public event Action<string> callback;

    private string _text;
    public string text
    {
        get
        {
            return _text;
        }
        set
        {
            _text = value;
            callback?.Invoke(value);
        }
    }
}