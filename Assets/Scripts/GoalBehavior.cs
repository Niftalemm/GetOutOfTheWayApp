using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GoalBehavior : MonoBehaviour
{
    [SerializeField] public AK.Wwise.Event startMakingNoise;
    [SerializeField] public AK.Wwise.Event stopMakingNoise;
    private bool active = true;

    
    

    // Start is called before the first frame update
    void Start()
    {
        startMakingNoise.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            // whatever the goal does while active goes here (if that is anything)
        }
        else
        {
            // anything the goal does while deactivated goes here
        }
    }

    public void FixedUpdate()
    {
        // empty lmao
    }

    /// <summary>
    /// Deactivate the goal and destroy the important components
    /// </summary>
    public void Deactivate()
    {
        // this method, get this, deactivates the goal.
        Destroy(GetComponent<MeshRenderer>());
        Destroy(GetComponent<BoxCollider>());
        stopMakingNoise.Post(gameObject);
        active = false;
    }
}


