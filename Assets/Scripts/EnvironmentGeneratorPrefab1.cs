using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using AK.Wwise;
using UnityEditor.Experimental.GraphView;

public class EnvironmentGeneratorPrefab1 : MonoBehaviour
{
   



    // Start is called before the first frame update
    void Start()
    {

        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        // Adding a Tag
        string[] s = { "Pillar", "Ball", "Wall", "Goal" };

        for (int i = 0; i < s.Length; i++)
        {
           
            tagsProp.InsertArrayElementAtIndex(0);
            SerializedProperty n = tagsProp.GetArrayElementAtIndex(0);
            n.stringValue = s[i];
            tagManager.ApplyModifiedPropertiesWithoutUndo();
        }
        


        /* Hardcoded pillar coordinates */
        Vector3[] PillarCords = { new Vector3(76, 7, 77), new Vector3(-40, 7, 61), new Vector3(3, 7, -58), new Vector3(50, 7, 17), new Vector3(-76, 7, -26),
        new Vector3(54, 7, -61), new Vector3(-9, 7, -3), new Vector3(10, 7, 58)};

        /* Hardcoded wall coordinates */
        Vector3[] WallCords = { new Vector3(0, 15, -100), new Vector3(100, 15, 0), new Vector3(-100, 15, 0), new Vector3(0, 15, 100) };

        /* Hardcoded wall orientation */
        Vector3[] WallOrient = { new Vector3(90, 0, 0), new Vector3(90, 0, 90), new Vector3(90, 0, 90), new Vector3(90, 0, 0) };

        Vector3 WallScale = new Vector3(20, 1, 3);
        Vector3 PillarScale = new Vector3(5, 15, 5);

        GameObject[] Walls = new GameObject[WallCords.Length];
        GameObject[] Pillars = new GameObject[PillarCords.Length];
        
        /* Generates a new pillar, using each set of coordinates from the PillarCords array. Requires Prefab */
        for (int i = 0; i < PillarCords.Length; i++)
        {
            GameObject pillar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pillar.AddComponent<Rigidbody>();
            pillar.GetComponent<Rigidbody>().useGravity = false;
            pillar.GetComponent<Rigidbody>().isKinematic = true;
            pillar.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
            pillar.AddComponent<AkGameObj>();
            pillar.AddComponent<AkAmbient>();
            pillar.tag = "Pillar";
            AkSoundEngine.PostEvent("Play_The_Creature_1", pillar);
            pillar.transform.localScale = PillarScale;
            pillar.transform.localPosition = PillarCords[i];
            Pillars[i] = pillar;
        }


        
        /* Generates a new wall, using each set of coordinates from the WallCords array, and rotation from the WallOrient array */
        for (int i = 0; i < WallCords.Length; i++)
        {
            GameObject Wall = GameObject.CreatePrimitive(PrimitiveType.Plane);
            Wall.AddComponent<Rigidbody>();
            Wall.GetComponent<Rigidbody>().useGravity = false;
            Wall.GetComponent<Rigidbody>().isKinematic = true;
            Wall.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
            Wall.AddComponent<AkGameObj>();
            Wall.AddComponent<AkAmbient>();
            Wall.tag = "Wall";
            AkSoundEngine.PostEvent("Play_The_Creature_1", Wall);
            Wall.transform.localScale = WallScale;
            Wall.transform.localPosition = WallCords[i];
            Wall.transform.localRotation = Quaternion.Euler(WallOrient[i]);
            Walls[i] = Wall;
        }

        GameObject plane1 = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane1.transform.localScale = new Vector3(20, 1, 20);

        GameObject goal = GameObject.CreatePrimitive(PrimitiveType.Cube);
        goal.AddComponent<Rigidbody>();
        goal.GetComponent<Rigidbody>().useGravity = false;
        goal.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        goal.AddComponent<AkGameObj>();
        goal.AddComponent<AkAmbient>();
        AkSoundEngine.PostEvent("Play_MUSCChim_InsJ_Hand_Bell_01_16_High_C", goal);
        goal.transform.localScale = new Vector3(8, 10, 8);
        goal.transform.localPosition = new Vector3(-5, 5, 79);
        goal.GetComponent<Renderer>().material.color = new Color(34, 139, 34);
        goal.tag = "Goal";
    }
    // Update is called once per frame
    void Update()
    {

    }
}