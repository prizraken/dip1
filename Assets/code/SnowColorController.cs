using UnityEngine;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class SnowColorController : MonoBehaviour
{
    public Material winterMaterial;
    public Color winterColor = Color.white;
    public Color defaultColor = Color.green;
    public Coordinate coordinateScript;
    private string apiUrl;

    private void Start()
    {
        apiUrl = $"https://api.openweathermap.org/data/2.5/weather?lat={coordinateScript.latitude}&lon={coordinateScript.longitude}&appid={coordinateScript.apiKey}&units=metric";

        UpdateWeatherAndChangeColor();
    }

    public void UpdateWeatherOnClick()
    {
        apiUrl = $"https://api.openweathermap.org/data/2.5/weather?lat={coordinateScript.latitude}&lon={coordinateScript.longitude}&appid={coordinateScript.apiKey}&units=metric";

        UpdateWeatherAndChangeColor();
    }

    private async void UpdateWeatherAndChangeColor()
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                //Debug.Log(responseBody);
                WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(responseBody);

                if (weatherData != null && weatherData.weather.Length > 0 && winterMaterial != null)
                {
                    foreach (WeatherCondition condition in weatherData.weather)
                    {
                        if (condition.main.ToLower() == "snow")
                        {
                            Debug.Log("Снегопад");
                            ChangeMaterialColor(winterColor);
                            return;
                        }
                    }
                    Debug.Log("Снега нет");
                    RestoreDefaultColor();
                }
                else
                {
                    Debug.Log("Данные о погоде недоступны");
                }
            }
            catch (HttpRequestException)
            {
                //Debug.LogError($"Ошибка HTTP-запроса: {e.Message}");
            }
        }
    }

    private void ChangeMaterialColor(Color color)
    {
        winterMaterial.color = color;
    }

    private void RestoreDefaultColor()
    {
        GetComponent<Renderer>().material.color = defaultColor;
    }

    [Serializable]
    public class WeatherData
    {
        public WeatherCondition[] weather;
    }

    [Serializable]
    public class WeatherCondition
    {
        public string main;
    }
}
