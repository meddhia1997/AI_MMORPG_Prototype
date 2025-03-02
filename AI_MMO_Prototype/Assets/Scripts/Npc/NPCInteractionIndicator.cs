using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Import this to check the current scene

public class NPCQuestInteraction : MonoBehaviour
{
    public GameObject interactionImage;

    private bool playerNearby = false;
    public GameObject questDisplay;

    private void Start()
    {
        if (interactionImage != null)
        {
            interactionImage.SetActive(false);
        }
    }

    private void Update()
    {
        // Check if the player is nearby, the "T" key is pressed, and the current scene is "Town"
        if (playerNearby && Input.GetKeyDown(KeyCode.T) && SceneManager.GetActiveScene().name == "Town")
        {
            if (interactionImage != null)
            {
                interactionImage.SetActive(false);
                questDisplay.gameObject.SetActive(true);
            }
            QuestManager.Instance.GenerateQuest();
        }
    }

    // When the player enters the NPC's trigger area.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&& SceneManager.GetActiveScene().name == "Town")
        {
            playerNearby = true;
            if (interactionImage != null)
            {
                interactionImage.SetActive(true);
            }
        }
    }

    // When the player exits the NPC's trigger area.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&& SceneManager.GetActiveScene().name == "Town")
        {
            playerNearby = false;
            if (interactionImage != null)
            {
                interactionImage.SetActive(false);
            }
        }
    }
}
