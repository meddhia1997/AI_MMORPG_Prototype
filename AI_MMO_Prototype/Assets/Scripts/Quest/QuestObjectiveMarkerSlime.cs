using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Text;
using Newtonsoft.Json;

public class QuestObjectiveMarkerSlime : MonoBehaviour
{
    [Tooltip("List of GameObjects to activate if ActiveMonster is 'slimes'.")]
    [SerializeField] private List<GameObject> slimeObjects;
    private bool done = false;

    void FixedUpdate()
    {
        // Check if the PlayerPref "ActiveMonster" is set to "slimes".
        if (PlayerPrefs.GetString("ActiveMonster", "") == "slimes")
        {
            Debug.Log("ActiveMonster is slimes. Activating slime objects...");
            foreach (GameObject obj in slimeObjects)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                    Debug.Log("Activated: " + obj.name);
                }
            }
        }
        else
        {
            Debug.Log("ActiveMonster is not slimes. Deactivating slime objects...");
            foreach (GameObject obj in slimeObjects)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                    Debug.Log("Deactivated: " + obj.name);
                }
            }
        }
    }

    void Update()
    {
        // Only perform the check if the current scene is "Scene_1"
        if (SceneManager.GetActiveScene().name != "Scene_1")
            return;

        // Check if all slime objects are destroyed (or null)
        bool allDestroyed = true;
        foreach (GameObject obj in slimeObjects)
        {
            if (obj != null)
            {
                allDestroyed = false;
                break;
            }
        }

        if (allDestroyed)
        {
            if (!done)
            {
                int experience = PlayerPrefs.GetInt("experience", 0);
                PlayerPrefs.SetInt("experience", experience + 100);
                done = true;
                Debug.Log("Experience added: " + experience);

                // Send updated experience to backend
                StartCoroutine(UpdatePlayerData(experience + 100));
            }

            PlayerPrefs.SetString("ActiveMonster", "Town");
            Debug.Log("All slime objects are destroyed in Scene_1. Setting ActiveMonster to 'Town'.");
        }
    }

    // Function to send the updated player data to the backend

 // Use Newtonsoft for better serialization

[System.Serializable]
public class PlayerData
{
    public int experience;
    public int level;
}

[System.Serializable]
public class PlayerRequest
{
    public string token;
    public PlayerData playerdata;
}

public IEnumerator UpdatePlayerData(int newExperience)
{
    string token = PlayerPrefs.GetString("authToken");  // Retrieve the token from PlayerPrefs

    if (string.IsNullOrEmpty(token))
    {
        Debug.LogError("Token is missing or empty!");
        yield break;
    }

    string url = "http://localhost:5000/auth/updatePlayerData";

    PlayerData playerData = new PlayerData
    {
        experience = newExperience,
        level = 2
    };

    PlayerRequest requestData = new PlayerRequest
    {
        token = token,
        playerdata = playerData
    };

    // Convert to JSON using Newtonsoft.Json
    string json = JsonConvert.SerializeObject(requestData);

    Debug.Log("Sending request with JSON: " + json); // Ensure JSON is not empty

    UnityWebRequest request = new UnityWebRequest(url, "PUT");
    byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
    request.downloadHandler = new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");

    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
        Debug.Log("Player data updated successfully: " + request.downloadHandler.text);
    }
    else
    {
        Debug.LogError("Error updating player data: " + request.error);
        Debug.LogError("Response: " + request.downloadHandler.text);
    }
}

}
