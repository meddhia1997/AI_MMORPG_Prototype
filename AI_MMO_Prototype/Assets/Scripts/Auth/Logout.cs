using UnityEngine;
using UnityEngine.UI;

public class Logout : MonoBehaviour
{
    public Button deleteButton;  // Drag the button into this field in the Unity Editor

    void Start()
    {
        if (deleteButton != null)
        {
            deleteButton.onClick.AddListener(DeleteAuthTokenAndExitApp);
        }
    }

    void DeleteAuthTokenAndExitApp()
    {
        // Delete the PlayerPrefs variable "authToken"
        PlayerPrefs.DeleteKey("authToken");

        PlayerPrefs.DeleteKey("playerRole");

        // Optionally, save PlayerPrefs to ensure the deletion is permanent
        PlayerPrefs.Save();

        // Exit the application
        Application.Quit();

        // If in the editor, stop play mode (useful for testing)
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
