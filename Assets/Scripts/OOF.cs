using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using System.Xml.Serialization;
using System.Linq;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class OOF : MonoBehaviour
{
    [SerializeField] public AK.Wwise.Event akOOFEvent;
    [SerializeField] public AK.Wwise.Event akGoalSuccess1;
    [SerializeField] public AK.Wwise.Event akGoalSuccess2;
    [SerializeField] public AK.Wwise.Event akGoalError;
    private List<GoalBehavior> goals;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        goals = GameObject.FindObjectsOfType<GoalBehavior>().ToList<GoalBehavior>();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Pillar" || collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ball")
        {
            akOOFEvent.Post(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("h*ck");
            ActivateGoal();
        }
    }
    private void ActivateGoal()
    {
        int index = 0;
        bool flag = false;

        while (!flag && index < goals.Count)
        {
            if (goals[index].WithinActivationRange(rb.transform.position))
            {
                goals[index].GetComponentInParent<AkAmbient>().Stop(1);
                goals[index].GetComponentInParent<AkAmbient>().Stop(1);
                Destroy(goals[index].GetComponentInParent<MeshRenderer>());
                goals.RemoveAt(index);
                akGoalSuccess1.Post(gameObject);
                flag = true;
            }
            index++;
        }
        if (!flag && index == goals.Count)
        {
            Debug.Log("There are no goal objects in range");
            akGoalError.Post(gameObject);
        }
    }
}
