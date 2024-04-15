using UnityEngine;

[CreateAssetMenu(fileName = "WeatherImage", menuName = "Scriptable Object/WeatherImage", order = int.MaxValue)]
public class WeatherImage : ScriptableObject
{

    [SerializeField] private Sprite _sunny;
    [SerializeField] private Sprite _cloudy;
    [SerializeField] private Sprite _rainy;
    [SerializeField] private Sprite _lightning;
    [SerializeField] private Sprite _snow;


    /// <summary>날씨 이름을 받아 날씨 이미지를 반환하는 함수</summary>
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
                Debug.LogErrorFormat("{0}이라는 이름을 가진 날씨 이미지가 존재하지 않습니다.", weatherName);
                return null;
        }
    }
}

