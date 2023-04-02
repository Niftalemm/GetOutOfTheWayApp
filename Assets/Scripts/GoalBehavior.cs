using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBehavior : MonoBehaviour
{
    public Transform playerPosition;
    public float activationDistance = 5f;
    bool active = true;
    
    

    // Start is called before the first frame update
    void Start()
    {
        // define playerPosition value
    }

    // Update is called once per frame
    void Update()
    {
        // none of this shit is tested
        RaycastHit hit;

        if (active)
        {
            // whatever the goal does goes here

            active = !(Physics.Raycast(playerPosition.position, playerPosition.forward, out hit, activationDistance) && Input.GetKeyDown(KeyCode.E)); // deactivate when player is looking at it and presses E
        }
    }
}
