using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MoveTarget : MonoBehaviour
{
    // Start is called before the first frame update
    private AIDestinationSetter aiDestiniationSetter;
    public GameObject targetTransform;
    private AIPath aiPath;

    void Start()
    {
        aiDestiniationSetter = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "TargetTrigger")
        {
            var test = collider.gameObject.GetComponent<TargetParams>(); 
            aiDestiniationSetter.target = collider.gameObject.GetComponent<TargetParams>().NextTarget.transform;
        }
    }
}
