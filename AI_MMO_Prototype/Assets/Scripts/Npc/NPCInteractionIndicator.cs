using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        if (playerNearby && Input.GetKeyDown(KeyCode.T))
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
        if (collision.CompareTag("Player"))
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
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
            if (interactionImage != null)
            {
                interactionImage.SetActive(false);
            }
        }
    }
}
