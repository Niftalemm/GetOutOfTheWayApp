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
    [SerializeField] public AK.Wwise.Event youDidGood;
    private Rigidbody rb;
    private bool active = true;
    public float activationDistance = 20f;

    
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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

    public void Deactivate()
    {
        // this method, get this, deactivates the goal.
        Destroy(GetComponent<MeshRenderer>());
        
        active = false;
    }

    public void Activate()
    {
        GetComponent<Material>().color = new Color(0, 255, 0);
        active = true;
    }

    public Vector3 GetPosition()
    {
        return rb.position;
    }

    public bool WithinActivationRange(Vector3 otherPosition)
    {
        return (Mathf.Sqrt(Mathf.Pow(otherPosition.x - rb.position.x,2) + Mathf.Pow(otherPosition.z - rb.position.z,2)) <= activationDistance);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(GoalBehavior)), InitializeOnLoadAttribute]
public class GoalBehaviorEditor : Editor
{
    GoalBehavior gb;
    SerializedObject SerGB;

    private void OnEnable()
    {
        gb = (GoalBehavior) target;
        SerGB = new SerializedObject(gb);
    }

    public override void OnInspectorGUI()
    {
        SerGB.Update();
        GUILayout.Label("Goal Behavior Parameter", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 });
        EditorGUILayout.Space();

        // create a slider for the activation distance
        gb.activationDistance = EditorGUILayout.Slider(new GUIContent("Activation Distance", "How far away the player have to be to complete the goal"), gb.activationDistance, 5, 100, GUILayout.Height(25));

        // update the values of the object
        if (GUI.changed)
        {
            EditorUtility.SetDirty(gb);
            Undo.RecordObject(gb, "BallInitialVelocity Change");
            SerGB.ApplyModifiedProperties();
        }
    }
}
#endif


