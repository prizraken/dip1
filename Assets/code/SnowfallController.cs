using UnityEngine;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class SnowfallController : MonoBehaviour
{
    public ParticleSystem snowParticleSystem;
    public Coordinate coordinateScript;
    private string apiUrl;

    private void Start()
    {
        apiUrl = $"https://api.openweathermap.org/data/2.5/weather?lat={coordinateScript.latitude}&lon={coordinateScript.longitude}&appid={coordinateScript.apiKey}&units=metric";

        UpdateWeatherAndControlParticleSystem();
    }
    public void UpdateWeatherOnClick()
    {
        apiUrl = $"https://api.openweathermap.org/data/2.5/weather?lat={coordinateScript.latitude}&lon={coordinateScript.longitude}&appid={coordinateScript.apiKey}&units=metric";

        UpdateWeatherAndControlParticleSystem();
    }

    private async void UpdateWeatherAndControlParticleSystem()
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

                if (weatherData != null && weatherData.weather.Length > 0 && snowParticleSystem != null)
                {
                    bool snowing = false;
                    foreach (WeatherCondition condition in weatherData.weather)
                    {
                        if (condition.main.ToLower() == "snow")
                        {
                            Debug.Log("Снегопад");
                            snowing = true;
                            break;
                        }
                    }

                    if (snowing)
                    {
                        snowParticleSystem.Play();
                    }
                    else
                    {
                        Debug.Log("Снега нет");
                        snowParticleSystem.Stop();
                    }
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
