using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GoalBehavior : MonoBehaviour
{
    public Transform playerPosition;
    public float activationDistance = 20f;
    private bool active = true;
    private Material mat;
    public Material completedMaterial;
    
    

    // Start is called before the first frame update
    void Start()
    {
        // define playerPosition value
        // define thegameObject
        mat = GetComponent<Material>();
        
    }

    // Update is called once per frame
    void Update()
    {

        // none of this shit is tested (evidently it doesn't work either)
        RaycastHit hit;

        if (active)
        {
            // whatever the goal does goes here
            
            active = !(Physics.Raycast(playerPosition.position, playerPosition.forward, out hit, activationDistance) && Input.GetKeyDown(KeyCode.E)); // deactivate when player is looking at it and presses E
            if (!active)
            {
                mat = completedMaterial;
                Debug.Log("Goal has been pressed.");
            }
        }
        if (Physics.Raycast(playerPosition.position, playerPosition.forward, out hit, activationDistance))
        {
            Debug.Log("Goal is being looked at.");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("The E button has been pressed.");
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(GoalBehavior)), InitializeOnLoadAttribute]
public class GoalBehaviorEditor : Editor
{
    GoalBehavior gb;
    SerializedObject SerGB;
    Material tempMat;
    Object player;

    private void OnEnable()
    {
        gb = (GoalBehavior) target;
        SerGB = new SerializedObject(gb);
    }

    public override void OnInspectorGUI()
    {
        SerGB.Update();
        GUILayout.Label("Goal Behavior Parameters", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 });
        EditorGUILayout.Space();

        // create a slider for the activation distance
        gb.activationDistance = EditorGUILayout.Slider(new GUIContent("Activation Distance", "How far away the player have to be to complete the goal"), gb.activationDistance, 5, 100, GUILayout.Height(25));

        // create a ObjectField for the material
        GUILayout.Label("Goal Completion Material", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontSize = 12});
        tempMat = (Material) EditorGUILayout.ObjectField(tempMat, typeof(Material), false, GUILayout.Height(25));
        

        // create an ObjectField for the player so the position of the player can be accessed
        GUILayout.Label("Player Object", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontSize = 12});
        player =  EditorGUILayout.ObjectField(player, typeof(Object), false, GUILayout.Height(25));

        gb.completedMaterial = tempMat;
        gb.playerPosition = player.GetComponent<Rigidbody>().transform; // this will cause errors


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


