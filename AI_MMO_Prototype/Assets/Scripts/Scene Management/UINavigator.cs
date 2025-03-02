using UnityEngine;
using UnityEngine.UI;

public class UINavigator : MonoBehaviour
{
    [System.Serializable]
    public class NavigationPair
    {
        public Button button;        // Button that triggers the navigation
        public GameObject hideUI;    // UI to deactivate
        public GameObject showUI;    // UI to activate
    }

    public NavigationPair[] navigationPairs; // Array of UI navigation buttons

    void Start()
    {
        // Assign button listeners
        foreach (var pair in navigationPairs)
        {
            if (pair.button != null && pair.hideUI != null && pair.showUI != null)
            {
                pair.button.onClick.AddListener(() => SwitchUI(pair.hideUI, pair.showUI));
            }
        }
    }

    void SwitchUI(GameObject toHide, GameObject toShow)
    {
        if (toHide != null) toHide.SetActive(false);
        if (toShow != null) toShow.SetActive(true);
    }
}
