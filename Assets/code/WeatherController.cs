using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class WeatherController : MonoBehaviour
{
    public ParticleSystem rainParticleSystem;
    public Coordinate coordinate;

    private string apiUrl;
    public async void UpdateWeatherOnClick()
    {
        if (coordinate == null)
        {
            Debug.LogError("111");
            return;
        }

        apiUrl = $"https://api.openweathermap.org/data/2.5/weather?lat={coordinate.latitude}&lon={coordinate.longitude}&appid={coordinate.apiKey}&units=metric";
        await CheckWeatherAndControlParticleSystem();
    }

    private async Task CheckWeatherAndControlParticleSystem()
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(responseBody);

                if (weatherData != null && weatherData.rain != null && rainParticleSystem != null)
                {
                    Debug.Log($"Объем дождя за последний час: {weatherData.rain.volume}");
                    if (weatherData.rain.volume > 0.00f)
                    {
                        if (!rainParticleSystem.isPlaying)
                            rainParticleSystem.Play();
                    }
                    else
                    {
                        if (rainParticleSystem.isPlaying)
                            rainParticleSystem.Stop();
                    }
                }
                else
                {
                    Debug.Log("Дождя нет");
                    if (rainParticleSystem.isPlaying)
                        rainParticleSystem.Stop();
                }
            }
            catch (HttpRequestException)
            {
                //Debug.LogError($"Ошибка HTTP-запроса: {e.Message}");
            }
        }
    }

    [System.Serializable]
    public class WeatherData
    {
        public WeatherCondition[] weather;
        public RainData rain;
    }

    [System.Serializable]
    public class WeatherCondition
    {
        public string main;
    }

    [System.Serializable]
    public class RainData
    {
        [JsonProperty("1h")]
        public float volume;
    }
}
