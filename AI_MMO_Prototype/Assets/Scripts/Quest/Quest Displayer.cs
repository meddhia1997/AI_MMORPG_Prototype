using System.Collections;
using TMPro;
using UnityEngine;

public class QuestDisplayer : MonoBehaviour
{
    [Tooltip("Delay between each character (in seconds).")]
    [SerializeField] private float delay = 0.05f;
    public GameObject questDisplay;
    public TMP_Text tmpText;


    /// <summary>
    /// Extracts the instructions from the full quest text and displays them with a typewriter effect.
    /// </summary>
    /// <param name="fullQuestText">The full quest text containing Quest, Objective, and Instructions.</param>
    public void questDisplayer(string fullQuestText)
    {
        string instructions = ExtractInstructions(fullQuestText);
        StopAllCoroutines();
        StartCoroutine(TypeText(instructions));
    }

    /// <summary>
    /// Looks for the "**Instructions:**" marker and returns the text after it.
    /// </summary>
    /// <param name="text">The full quest text.</param>
    /// <returns>The instructions text, or the full text if the marker isn't found.</returns>
private string ExtractInstructions(string text)
{
    string instructionsMarker = "**Quest: Ghostly Helpers**";
    string rewardMarker = "Reward:";
    
    int instructionsIndex = text.IndexOf(instructionsMarker);
    if (instructionsIndex != -1)
    {
        int start = instructionsIndex + instructionsMarker.Length;
        int rewardIndex = text.IndexOf(rewardMarker, start);
        string extracted;
        
        if (rewardIndex != -1)
        {
            extracted = text.Substring(start, rewardIndex - start).Trim();
        }
        else
        {
            extracted = text.Substring(start).Trim();
        }
        return extracted;
    }
    return text; // fallback if marker not found
}


    private IEnumerator TypeText(string fullText)
    {
        tmpText.text = "";
        foreach (char letter in fullText)
        {
            tmpText.text += letter;
            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForSeconds(2);

        questDisplay.gameObject.SetActive(false);
        tmpText.text = "Am thinking ...";

    }
}
