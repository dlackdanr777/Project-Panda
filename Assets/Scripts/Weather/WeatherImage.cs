using UnityEngine;

[CreateAssetMenu(fileName = "WeatherImage", menuName = "Scriptable Object/WeatherImage", order = int.MaxValue)]
public class WeatherImage : ScriptableObject
{

    [SerializeField] private Sprite _sunny;
    [SerializeField] private Sprite _cloudy;
    [SerializeField] private Sprite _rainy;
    [SerializeField] private Sprite _lightning;
    [SerializeField] private Sprite _snow;


    /// <summary>���� �̸��� �޾� ���� �̹����� ��ȯ�ϴ� �Լ�</summary>
    public Sprite GetWeatherImage(string weatherName)
    {
        switch(weatherName)
        {
            case "Sunny":
                return _sunny;
            case "Cloudy":
                return _cloudy;
            case "Rainy":
                return _rainy;
            case "Lightning":
                return _lightning;
            case "Snow":
                return _snow;
            default:
                Debug.LogErrorFormat("{0}�̶�� �̸��� ���� ���� �̹����� �������� �ʽ��ϴ�.", weatherName);
                return null;
        }
    }
}

