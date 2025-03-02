using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using Newtonsoft.Json;

public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager Instance;  // Singleton instance

    public int currentExp { get; private set; }
    public int currentLevel { get; private set; }
    public int expToNextLevel { get; private set; }

    private string apiUrl = "http://localhost:5000/auth/updatePlayerData"; // API endpoint to update player data

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Load experience and level from PlayerPrefs (if available)
      
    }

    public void GainExp(int amount)
    {
          currentExp = PlayerPrefs.GetInt("experience");
        Debug.Log("experience: " + currentExp); // Default to 0 if not found
        currentLevel = CalculateLevelFromExperience(currentExp);

        // Calculate experience needed for next level based on the current level
        expToNextLevel = CalculateExpForLevel(currentLevel);

        Debug.Log($"Starting Level: {currentLevel}, Experience: {currentExp}/{expToNextLevel}");
        int previousExp = currentExp;  // Store previous experience for comparison
        currentExp += amount;
        Debug.Log($"Gained {amount} EXP. Total: {currentExp}/{expToNextLevel}");

        if (currentExp != previousExp)  // Only call API if experience has been updated
        {
            // Save the updated experience and level to PlayerPrefs
            PlayerPrefs.SetInt("level", currentLevel);
            PlayerPrefs.Save();

            // Send the updated data to the backend server
            StartCoroutine(UpdatePlayerData(currentExp, currentLevel));

        }

        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        // Check if the player has gained enough experience to level up
        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            currentLevel++;
            expToNextLevel = CalculateExpForLevel(currentLevel);
            OnLevelUp();
        }
    }

    private int CalculateExpForLevel(int level)
    {
        // For example, a simple formula; you can customize this
        return level * 100;
    }

    private int CalculateLevelFromExperience(int exp)
    {
        // Calculate the level based on the current experience
        int level = 1; // Default starting level
        while (exp >= CalculateExpForLevel(level))
        {
            level++;
        }
        return level;
    }

    private void OnLevelUp()
    {
        Debug.Log($"Leveled Up! New Level: {currentLevel}");
        // Unlock PvP mode at level 6 (or any other feature)
        if (currentLevel == 6)
        {
            // TODO: Enable PvP matchmaking logic here
            Debug.Log("PvP mode unlocked!");
        }
        // You might also want to trigger a UI update or animation here.
    }

    // Coroutine to send updated player data to the backend server
    private IEnumerator UpdatePlayerData(int experience, int level)
    {
        string token = PlayerPrefs.GetString("authToken"); // Retrieve the token from PlayerPrefs

        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("Token is missing or empty!");
            yield break; // Stop execution if the token is missing
        }

        var playerData = new
        {
            token = token,
            playerdata = new
            {
                experience = experience,
                level = level
            }
        };

        // Convert the player data to JSON
        string json = JsonConvert.SerializeObject(playerData);

        // Create a PUT request to send to the API
        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "PUT"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Wait for the response
            yield return request.SendWebRequest();

            // Handle the response
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
}
