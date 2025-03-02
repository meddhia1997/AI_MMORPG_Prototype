using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeProximityToggle : MonoBehaviour
{
    [Tooltip("Detection radius to check for slime objects.")]
    [SerializeField] private float detectionRadius = 3f;

    private Collider2D myCollider;

    void Awake()
    {
        myCollider = GetComponent<Collider2D>();
        if (myCollider == null)
        {
            Debug.LogError("No Collider2D found on " + gameObject.name);
        }
    }

    void Update()
    {
        // Check if any colliders within detectionRadius have the tag "slime"
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        bool slimeNearby = false;
        foreach (Collider2D col in nearbyColliders)
        {
            if (col.CompareTag("Slime"))
            {
                slimeNearby = true;
                break;
            }
        }
        
        // Set this object's collider as trigger if a slime is nearby; otherwise, set it back to non-trigger.
        myCollider.isTrigger = slimeNearby;
    }

    // Optional: Draw the detection radius in the Scene view for debugging.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
