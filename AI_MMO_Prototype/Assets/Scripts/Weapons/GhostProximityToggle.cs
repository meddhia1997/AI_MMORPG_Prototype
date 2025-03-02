using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostProximityToggle : MonoBehaviour
{
    [Tooltip("Detection radius to check for ghost objects.")]
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
        // Get all colliders within the detection radius.
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        bool ghostNearby = false;
        
        foreach (Collider2D col in nearbyColliders)
        {
            // Exclude the object itself.
            if (col.gameObject == gameObject)
                continue;

            // Check if the collider belongs to a ghost.
            if (col.CompareTag("ghost"))
            {
                ghostNearby = true;
                break;
            }
        }
        
        // Set this object's collider as trigger if a ghost is nearby; otherwise, set it back to non-trigger.
        myCollider.isTrigger = ghostNearby;
    }

    // Optional: Draw the detection radius in the Scene view for debugging.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
