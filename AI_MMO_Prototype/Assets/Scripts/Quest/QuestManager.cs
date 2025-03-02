using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class QuestManager : Singleton<QuestManager>
{
    public List<Quest> activeQuests = new List<Quest>();

    // Stores the last selected monster type to avoid consecutive repeats.
    private string lastMonsterType = "";

    /// <summary>
    /// Returns either "ghost" or "slime", ensuring the same word isn't returned consecutively.
    /// </summary>
    private string GetMonsterType()
    {
        string newType = "";
        if (lastMonsterType == "ghost")
        {
            newType = "slime";
        }
        else if (lastMonsterType == "slime")
        {
            newType = "ghost";
        }
        else
        {
            newType = (Random.value < 0.5f) ? "ghost" : "slime";
        }
        lastMonsterType = newType;
        return newType;
    }

    // Call this method to request a new quest from DeepSeek.
    public void GenerateQuest()
    {
        StartCoroutine(GetQuestFromDeepSeek());
    }

    private IEnumerator GetQuestFromDeepSeek()
    {
        // DeepSeek API endpoint (replace with the actual endpoint if needed)
        string url = "https://api.deepseek.com/v1/chat/completions";

        // Get one of the two monster types using our function.
        string selectedWord = GetMonsterType();

        // Build the prompt using the selected monster type.
        string prompt = "Generate a single, simple, friendly quest for children aged 5-8. " +
                        "Your output must include ONLY one quest instruction with no extra text or titles. " +
                        "The instruction must be a maximum of 50 characters. " +
                        "There are two possible monster types: slime and ghost. " +
                        "Choose " + selectedWord + " and output only its corresponding instruction. " +
                        "For a slime-type quest, produce a phrase that conveys the idea of poping slime samples so they can go back to earth. " +
                        "For a ghost-type quest, produce a phrase that conveys the idea of using a wand to help ghosts finish their business and ascend' " +
                        "Do not output both instructions, only one.";

        // Create the request payload for DeepSeek.
        string jsonPayload = JsonUtility.ToJson(new DeepSeekRequest
        {
            model = "deepseek-chat",
            messages = new List<Message>
            {
                new Message { role = "user", content = prompt }
            }
        });

        // Create a UnityWebRequest.
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + GetApiKey());

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("DeepSeek API Error: " + request.error);
        }
        else
        {
            // Parse the JSON response.
            string jsonResponse = request.downloadHandler.text;
            DeepSeekResponse response = JsonUtility.FromJson<DeepSeekResponse>(jsonResponse);

            if (response != null && response.choices != null && response.choices.Count > 0)
            {
                string questDescription = response.choices[0].message.content;
                Debug.Log("New Quest Generated: " + questDescription);

                // Create a new Quest object with the simplified description.
                Quest newQuest = new Quest(questDescription);
                activeQuests.Add(newQuest);
                Debug.Log("Quest Added: " + newQuest.description);

                // Send the quest description to LMNT for speech synthesis.
                LMNTSpeechManager lmnt = FindObjectOfType<LMNTSpeechManager>();
                if (lmnt != null)
                {
                    lmnt.GenerateSpeech(newQuest.description);
                }
                else
                {
                    Debug.LogError("LMNTSpeechManager not found in the scene.");
                }

                // Display the quest using QuestDisplayer.
                QuestDisplayer questDisplayer = FindObjectOfType<QuestDisplayer>();
                if (questDisplayer != null)
                {
                    questDisplayer.questDisplayer(newQuest.description);
                }
                else
                {
                    Debug.LogError("QuestDisplayer not found in the scene.");
                }
            }
            else
            {
                Debug.LogError("Invalid response from DeepSeek API.");
            }
        }
    }

    private string GetApiKey()
    {
        // Retrieve the API key from a secure location (e.g., environment variable or config file)
        return "sk-8b6c137591c342edb6a2b3d011a4f369"; // Replace with your actual API key
    }

    [System.Serializable]
    private class DeepSeekRequest
    {
        public string model;
        public List<Message> messages;
    }

    [System.Serializable]
    private class Message
    {
        public string role;
        public string content;
    }

    [System.Serializable]
    private class DeepSeekResponse
    {
        public List<Choice> choices;
    }

    [System.Serializable]
    private class Choice
    {
        public Message message;
    }
}
