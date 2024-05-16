using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;

public class WeatherInfoDisplay : MonoBehaviour
{
    public TextMeshProUGUI infoText;

    private string apiUrl;
    private double latitude;
    private double longitude;
    private string apiKey;
    public Coordinate coordinateScript;

    private async void Start()
    {
        latitude = coordinateScript.latitude;
        longitude = coordinateScript.longitude;
        apiKey = coordinateScript.apiKey;
        apiUrl = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={apiKey}&units=metric";
        await UpdateWeatherInfo();
    }

    public async void OnButtonClick()
    {
        latitude = coordinateScript.latitude;
        longitude = coordinateScript.longitude;
        apiKey = coordinateScript.apiKey;
        apiUrl = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={apiKey}&units=metric";
        await UpdateWeatherInfo();
    }

    private async Task UpdateWeatherInfo()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        var operation = request.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Delay(100);
        }

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseBody = request.downloadHandler.text;
            Debug.Log("����� �� �������: " + responseBody);

            WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(responseBody);

            if (weatherData != null)
            {
                float temperature = weatherData.main.temp;
                Debug.Log("����������� ��������: " + temperature);

                infoText.text = $"�����������: {temperature} �C";
            }
            else
            {
                Debug.Log("������ � ������ ����������.");
            }
        }
        else
        {
            Debug.LogError($"������ HTTP-�������: {request.error}");
        }

        request.Dispose();
    }

    [Serializable]
    public class WeatherData
    {
        public MainData main;
    }

    [Serializable]
    public class MainData
    {
        public float temp;
    }
}
