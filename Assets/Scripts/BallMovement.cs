using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
using System.Net;
#endif

public class BallMovement : MonoBehaviour
{
    public Vector3 intitialVelocity;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.velocity = intitialVelocity;
    }

    // Update is called once per frame
    void Update()
    {
    }

    // FixedUpdate is called once every 50th of a second
    void FixedUpdate()
    {
        
    }

    // OnCollisionEnter is called on each collision
    private void OnCollisionEnter(Collision collision)
    {
        intitialVelocity *= -1;
        rb.velocity = intitialVelocity;

        // player collisions still affect the ball's trajectory ugh
    }
}



// Custom Editor
// Pretty much all of this code was stolen from the FirstPersonController.cs script
// but hey I figured out how to make an in-inspector editor so a win is a win
#if UNITY_EDITOR
[CustomEditor(typeof(BallMovement)), InitializeOnLoadAttribute]
public class BallMovementEditor : Editor
{
    BallMovement bm;
    SerializedObject SerBM;
    float direction;
    float speed;

    private void OnEnable()
    {
        bm = (BallMovement)target;
        SerBM = new SerializedObject(bm);
    }

    public override void OnInspectorGUI()
    {
        SerBM.Update();
        GUILayout.Label("Ball Initial Movement", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 });
        EditorGUILayout.Space();

        // create sliders for speed and direction
        direction = EditorGUILayout.Slider(new GUIContent("Ball Movement Direction", "Determines the direction the ball travels in degrees. 0 degrees is in the positive X direction"),
            (float) direction,
            0,
            360,
            GUILayout.Height(25));
        speed = EditorGUILayout.Slider(new GUIContent("Ball Movement Speed", "how fast it go :O"), (float) speed, 0, 100, GUILayout.Height(25));

        // convert the direction to a Vector3
        float constant = Mathf.PI / 180;
        bm.intitialVelocity = new Vector3(speed * Mathf.Sin(constant * direction), 0, speed * Mathf.Cos(constant * direction));

        if (GUI.changed)
        {
            EditorUtility.SetDirty(bm);
            Undo.RecordObject(bm, "BallInitialVelocity Change");
            SerBM.ApplyModifiedProperties();
        }
    }
}
#endif
