using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using Newtonsoft.Json;

public class PlayerDataFetcher : MonoBehaviour
{
    private string apiUrl = "http://localhost:5000/auth/getPlayerData"; // API endpoint

    public static PlayerDataFetcher Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern to access from other scripts
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartFetchingPlayerData()
    {
        StartCoroutine(GetPlayerData()); // Trigger fetching once login is successful
    }

    IEnumerator GetPlayerData()
    {
        string token = PlayerPrefs.GetString("authToken");  // Retrieve the saved token

        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("Auth Token is missing. Unable to fetch player data.");
            yield break;
        }

        // Correct JSON formatting
        var requestData = new { token = token };
        string json = JsonConvert.SerializeObject(requestData);

        // Log request data
        Debug.Log("Sending request with JSON: " + json);

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Player data received: " + request.downloadHandler.text);
                ProcessPlayerData(request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error fetching player data: " + request.error);
                Debug.LogError("Response: " + request.downloadHandler.text);
            }
        }
    }

    void ProcessPlayerData(string jsonResponse)
    {
        try
        {
            PlayerDataResponse response = JsonConvert.DeserializeObject<PlayerDataResponse>(jsonResponse);

            if (response.success)
            {
                PlayerPrefs.SetInt("level", response.playerdata.level);
                PlayerPrefs.SetInt("experience", response.playerdata.experience);
                PlayerPrefs.Save(); // Save changes
        LevelDisplayManager.Instance.UpdateLevelDisplay();

                Debug.Log("Updated Player Level: " + response.playerdata.level);
                Debug.Log("Updated Player Experience: " + response.playerdata.experience);
            }
            else
            {
                Debug.LogError("Failed to update player data. Response was not successful.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error parsing player data: " + e.Message);
        }
    }

    [System.Serializable]
    private class PlayerDataResponse
    {
        public bool success;
        public PlayerData playerdata;
    }

    [System.Serializable]
    private class PlayerData
    {
        public int level;
        public int experience;
    }
}
