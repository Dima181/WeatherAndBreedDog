using Newtonsoft.Json.Linq;
using UnityEngine.Networking;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System;

namespace Assets.CodeBase.Gameplay.Services.BreedDog.Data
{
    public class BreedDogServerDataProvider
    {
        public async UniTask<IReadOnlyList<BreedId>> GetBreedIdsAsync(CancellationToken cancellationToken = default)
        {
            const string url = "https://dogapi.dog/api/v2/breeds";

            using var request = UnityWebRequest.Get(url);
            await request.SendWebRequest().ToUniTask(cancellationToken: cancellationToken);

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error while getting breeds: " + request.error);
                return Array.Empty<BreedId>();
            }

            string json = request.downloadHandler.text;
            JObject parsed = JObject.Parse(json);
            JArray breedsArray = (JArray)parsed["data"];

            if (breedsArray == null || breedsArray.Count == 0)
            {
                Debug.LogWarning("The data array is empty or missing!");
                return Array.Empty<BreedId>();
            }

            var breedIds = new List<BreedId>(breedsArray.Count);

            foreach (JObject breedObj in breedsArray)
            {
                string id = breedObj["id"]?.ToString();
                string name = breedObj["attributes"]?["name"]?.ToString();

                if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(name))
                {
                    breedIds.Add(new BreedId { Id = id, Name = name });
                }
            }

            return breedIds;
        }


        public async UniTask<BreedData> GetBreedDetailsAsync(string breedId, CancellationToken cancellationToken = default)
        {
            string url = $"https://dogapi.dog/api/v2/breeds/{breedId}";

            using var request = UnityWebRequest.Get(url);
            await request.SendWebRequest().ToUniTask(cancellationToken: cancellationToken);

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error retrieving breed information: " + request.error);
                return null;
            }

            string json = request.downloadHandler.text;
            JObject parsed = JObject.Parse(json);
            JObject breedJson = (JObject)parsed["data"];

            return JsonUtility.FromJson<BreedData>(breedJson.ToString());
        }
    }

}
