using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
using System.Net;
#endif

public class BallInitialVelocity : MonoBehaviour
{
    public Vector3 intitialVelocity = new Vector3(1, 0, 1);

    // Start is called before the first frame update
    void Start()
    {
        // change initial velocity here
        this.GetComponent<Rigidbody>().velocity = new Vector3(100,0,100);

        Debug.Log(intitialVelocity.x);
        Debug.Log(intitialVelocity.z);
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Rigidbody>().velocity = intitialVelocity;
    }
}

// Custom Editor
// Pretty much all of this code was stolen from the FirstPersonController.cs script
// but hey I figured out how to make an in-inspector editor so a win is a win
#if UNITY_EDITOR
[CustomEditor(typeof(BallInitialVelocity)), InitializeOnLoadAttribute]
public class SphereVelocityEditor : Editor
{
    BallInitialVelocity biv;
    SerializedObject SerBIV;
    float direction;
    float speed;

    private void OnEnable()
    {
        biv = (BallInitialVelocity)target;
        SerBIV = new SerializedObject(biv);
    }

    public override void OnInspectorGUI()
    {
        SerBIV.Update();
        GUILayout.Label("Ball Initial Movement", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 });
        EditorGUILayout.Space();

        // create sliders for speed and direction
        direction = EditorGUILayout.Slider(new GUIContent("Ball Movement Direction", "Determines the direction the ball travels in degrees. 0 degrees is in the positive X direction"),
            (float) direction,
            0,
            360,
            GUILayout.Height(25));
        speed = EditorGUILayout.Slider(new GUIContent("Ball Movement Speed", "how fast it go :O"),
            (float) speed,
            0,
            100,
             GUILayout.Height(25));

        // convert the direction to a Vector3
        float constant = Mathf.PI / 180;
        biv.intitialVelocity = new Vector3(speed * Mathf.Sin(constant * direction), 0, speed * Mathf.Cos(constant * direction));
        Debug.Log(speed * Mathf.Sin(constant * direction));
        Debug.Log(speed * Mathf.Cos(constant * direction));
    }
}
#endif
