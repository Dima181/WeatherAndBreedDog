using System.Collections;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

namespace Assets.CodeBase.Gameplay.Services.Weather.Data
{
    public class WeatherServerDataProvider
    {
        public async UniTask GetWeatherAsync(TextMeshProUGUI description, RawImage weatherIcon, CancellationToken cancellationToken = default)
        {
            string url = "https://api.weather.gov/gridpoints/TOP/31,80/forecast";
            using var request = UnityWebRequest.Get(url);

            await request.SendWebRequest().ToUniTask(cancellationToken: cancellationToken);

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error while getting weather: " + request.error);
                return;
            }

            var json = request.downloadHandler.text;
            JObject parsed = JObject.Parse(json);
            JArray periods = (JArray)parsed["properties"]?["periods"];

            if (periods == null || periods.Count == 0)
            {
                Debug.LogWarning("No forecasts found in API response.");
                return;
            }

            DateTime now = DateTime.UtcNow;
            JObject bestMatch = null;

            foreach (JObject period in periods)
            {
                string startTimeStr = period["startTime"]?.ToString();
                bool isDaytime = period["isDaytime"]?.ToObject<bool>() ?? false;

                if (DateTime.TryParse(startTimeStr, out var startTime))
                {
                    if (startTime > now && isDaytime)
                    {
                        bestMatch = period;
                        break;
                    }
                }
            }

            if (bestMatch == null)
            {
                Debug.LogWarning("No suitable daily forecast found.");
                return;
            }

            string name = bestMatch["name"]?.ToString();
            string temp = bestMatch["temperature"]?.ToString();
            string unit = bestMatch["temperatureUnit"]?.ToString();
            string shortForecast = bestMatch["shortForecast"]?.ToString();
            string iconUrl = bestMatch["icon"]?.ToString();

            description.text = string.Format($"{name} - {temp}{unit}");
            Debug.Log($"Forecast for {name}: {temp}°{unit}, {shortForecast}");

            if (!string.IsNullOrEmpty(iconUrl))
            {
                await DownloadAndSetIconAsync(iconUrl, weatherIcon, cancellationToken);
            }
        }

        private async UniTask DownloadAndSetIconAsync(string url, RawImage weatherIcon, CancellationToken cancellationToken = default)
        {
            using var request = UnityWebRequestTexture.GetTexture(url);

            await request.SendWebRequest().ToUniTask(cancellationToken: cancellationToken);

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error loading icon: " + request.error);
                return;
            }

            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            weatherIcon.texture = texture;
        }
    }
}
