using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestTracker : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Press Q to log active quests to the console.
        
            DebugActiveQuests();
        
    }

    private void DebugActiveQuests()
    { 
        if (SceneManager.GetActiveScene().name == "Scene_1")
            return;
        if (QuestManager.Instance == null)
        {
            Debug.LogError("QuestManager instance not found.");
            return;
        }

        List<Quest> activeQuests = QuestManager.Instance.activeQuests;
        if (activeQuests == null || activeQuests.Count == 0)
        {
            Debug.Log("No active quests.");
        }
        else
        {
            Debug.Log("Active Quests:");
            foreach (Quest quest in activeQuests)
            {
                // Assuming Quest has a public string property called "description"
                Debug.Log("Quest: " + quest.description);
                
                // Check if the quest description contains "slime" (case-insensitive)
                if (quest.description.ToLower().Contains("slime"))
                {
                    PlayerPrefs.SetString("ActiveMonster", "slimes");
                    Debug.Log("PlayerPref 'ActiveMonster' set to 'slimes'");
                }
                 if (quest.description.ToLower().Contains("ghost"))
                {
                    PlayerPrefs.SetString("ActiveMonster", "ghosts");
                    Debug.Log("PlayerPref 'ActiveMonster' set to 'ghosts'");
                }
            }
        }
    }
}
