using UnityEngine;

public class RoleAssignor : MonoBehaviour
{
    // Define a class to store role-based game objects and their corresponding roles
    [System.Serializable]
    public class RoleBasedObjects
    {
        public GameObject[] roleBasedObjects;  // Array of game objects that correspond to different roles
        public string role;  // The role to match against
    }

    // Array to hold different sets of game objects corresponding to different roles
    public RoleBasedObjects[] roleBasedObjects;  

    private string playerRole;  // The role to match against

    void Update()
    {
        // Retrieve player role from PlayerPrefs
        playerRole = PlayerPrefs.GetString("playerRole", "");
        
        if (string.IsNullOrEmpty(playerRole))
        {
            Debug.LogError("No player role found in PlayerPrefs!");
            return;
        }

        // Handle the role-based game objects
        HandleRoleBasedObjects();
    }

    void HandleRoleBasedObjects()
    {
        if (roleBasedObjects.Length == 0)
        {
            Debug.LogError("No game objects assigned to roleBasedObjects array!");
            return;
        }

        // Iterate through each role set
        foreach (var roleSet in roleBasedObjects)
        {
            // Check if the current role set's role matches the player's role
            if (roleSet.role == playerRole)
            {
                // Iterate through each game object in this role set
                foreach (var obj in roleSet.roleBasedObjects)
                {
                    if (obj != null)
                    {
                        // If the game object matches the player's role, keep it
                        Debug.Log("Matching role found: " + obj.name);
                    }
                }
            }
            else
            {
                // If the role doesn't match, destroy all objects in this set
                foreach (var obj in roleSet.roleBasedObjects)
                {
                    if (obj != null)
                    {
                        Destroy(obj);
                        Debug.Log("Destroyed object: " + obj.name);
                    }
                }
            }
        }
    }
}
