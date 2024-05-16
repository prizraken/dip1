using UnityEngine;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections;

public class CloudController : MonoBehaviour
{
    [SerializeField] public GameObject cloudPrefab;
    public int cloudDensityThreshold = 5;
    public int maxClouds = 8;
    public float minDistance = 10f;
    public float cloudSize = 2.5f;
    public float maxSpeed = 1f;
    public Coordinate coordinateScript;

    private string apiUrl;

    private void Start()
    {

        apiUrl = $"https://api.openweathermap.org/data/2.5/weather?lat={coordinateScript.latitude}&lon={coordinateScript.longitude}&appid={coordinateScript.apiKey}&units=metric";
        UpdateWeather();
        StartCoroutine(SpawnCloudsPeriodically());
    }


    public void UpdateWeatherOnClick()
    {

        apiUrl = $"https://api.openweathermap.org/data/2.5/weather?lat={coordinateScript.latitude}&lon={coordinateScript.longitude}&appid={coordinateScript.apiKey}&units=metric";

        UpdateWeather();
    }
    private IEnumerator SpawnCloudsPeriodically()
    {
        while (true)
        {
            UpdateWeather();
            yield return new WaitForSeconds(6f); 
        }
    }

    private async void UpdateWeather()
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Debug.Log(responseBody);
                WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(responseBody);

                if (weatherData != null && weatherData.clouds != null)
                {
                    Debug.Log($"Облачность - {weatherData.clouds.cloudiness}%");

                    int cloudCount = 0;
                    int cloudiness = weatherData.clouds.cloudiness;
                    if (cloudiness <= 30)
                    {
                        cloudCount = 3;
                    }
                    else if (cloudiness <= 60)
                    {
                        cloudCount = 5;
                    }
                    else if (cloudiness <= 100)
                    {
                        cloudCount = 7;
                    }

                    if (cloudCount > 0)
                    {
                        SpawnClouds(cloudCount);
                    }
                    else
                    {
                        Debug.Log("ккк");
                    }
                }
                else
                {
                    Debug.Log("Данные о погоде не найдены.");
                }
            }
            catch (HttpRequestException e)
            {
                Debug.LogError($"Ошибка HTTP-запроса: {e.Message}");
            }
        }
    }

    private void SpawnClouds(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            GameObject cloudObject = Instantiate(cloudPrefab, randomPosition, Quaternion.identity);
            Cloud cloud = cloudObject.AddComponent<Cloud>();
            cloud.DestroyAfterDistance(30f);
            Rigidbody2D rb = cloudObject.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(maxSpeed, 0f);
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 randomPosition = new Vector3(UnityEngine.Random.Range(-35f, -20f), UnityEngine.Random.Range(10f, -10f), 0f);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(randomPosition, new Vector2(cloudSize, cloudSize), 0f);
        if (colliders.Length > 0)
        {
            return GetRandomPosition();
        }
        return randomPosition;
    }

    public class Cloud : MonoBehaviour
    {
        private Vector3 initialPosition;

        private void Start()
        {
            initialPosition = transform.position;
        }

        public void DestroyAfterDistance(float distance)
        {
            StartCoroutine(DestroyAfterDistanceCoroutine(distance));
        }

        private IEnumerator DestroyAfterDistanceCoroutine(float distance)
        {
            float initialX = initialPosition.x;
            while (transform.position.x < initialX + distance)
            {
                yield return null;
            }
            Destroy(gameObject);
        }
    }

    [Serializable]
    public class WeatherData
    {
        public CloudData clouds;
    }

    [Serializable]
    public class CloudData
    {
        [JsonProperty("all")]
        public int cloudiness;
    }
}
