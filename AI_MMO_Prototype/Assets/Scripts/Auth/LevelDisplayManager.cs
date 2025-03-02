using UnityEngine;
using TMPro;

public class LevelDisplayManager : MonoBehaviour
{
    // Singleton instance
    public static LevelDisplayManager Instance;

    public TextMeshProUGUI levelText;  // Reference to TMP component that will display the level

    private void Awake()
    {
        // Ensure there's only one instance of LevelDisplayManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Optionally, update the level on start (if TMP component is set)
        if (levelText != null)
        {
            UpdateLevelDisplay();
        }
    }

    // Update the level display (called from other scripts)
    public void UpdateLevelDisplay()
    {
        int playerLevel = PlayerPrefs.GetInt("level", 1); // Get the level from PlayerPrefs (default to 1 if not found)
        levelText.text = playerLevel.ToString();  // Assign level to the TMP text component
    }
}
