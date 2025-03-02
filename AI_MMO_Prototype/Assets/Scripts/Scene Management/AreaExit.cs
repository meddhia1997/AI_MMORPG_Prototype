using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    // Optional: Use these to help with scene transitions.
    [SerializeField] private string sceneTransitionName;
    [SerializeField] private float waitToLoadTime = 1f; // seconds

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            string typeOfMonster = PlayerPrefs.GetString("ActiveMonster", "");
            Debug.Log("ActiveMonster: " + typeOfMonster);

            if (typeOfMonster == "slimes")
            {
                SceneManagement.Instance.SetTransitionName("East_Entrance");
                UIFade.Instance.FadeToBlack();
                StartCoroutine(LoadSceneRoutine("Scene_1"));
            }
            else if (typeOfMonster == "ghosts")
            {
                SceneManagement.Instance.SetTransitionName("South_Entrance");
                UIFade.Instance.FadeToBlack();
                StartCoroutine(LoadSceneRoutine("Scene_2"));
            }
            else
            {
                SceneManagement.Instance.SetTransitionName("North_Entrance");
                UIFade.Instance.FadeToBlack();
                PlayerDataFetcher.Instance.StartFetchingPlayerData();

                StartCoroutine(LoadSceneRoutine("Town"));
            }
        }
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        // Use a local wait timer so that waitToLoadTime is not modified globally.
        float timeLeft = waitToLoadTime;
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("Loading scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
