using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System.IO;
using System.Text;



public class CollisionTracker : MonoBehaviour
{


    
    public class collectedData
    {
        public int staticCollisions = 0;
        public int movingCollisions = 0;
        public List<string> position = new List<string>();
        public List<string> rotation = new List<string>();
        public float baseTimer;
        public float timer1 = 0;
        public float timer2 = 0;
        public float timer3 = 0;
    }

    collectedData tracker = new collectedData();
    
    private void WriteToCSV()
    {
        string filePath = Application.dataPath + "/results.csv";
        if (!File.Exists(filePath))
        {
            StreamWriter creativeWriter = new StreamWriter(filePath);
            creativeWriter.WriteLine("Participant,Static Collisions,Moving Collisions,Location,Rotation,TTT1,TTT2,TTT3");
            creativeWriter.Flush();
            creativeWriter.Close();
        }
        
        string[] tempPosition = tracker.position.ToArray();
        string[] tempRotation = tracker.rotation.ToArray();
        string fullPosition = "";
        string fullRotation = "";

        for (int i = 0; i < tempPosition.Length - 2; i++)
        {
            fullPosition += tempPosition[i] + ", ";
        }
        fullPosition += tempPosition[tempPosition.Length - 1];

        for (int i = 0; i < tempRotation.Length - 2; i++)
        {
            fullRotation += tempRotation[i] + ", ";
        }
        fullRotation += tempRotation[tempRotation.Length - 1];
        StreamWriter writer = File.AppendText(filePath);
        writer.WriteLine("Goober McGee" + "," + tracker.staticCollisions + "," + tracker.movingCollisions + "," + "\"" + fullPosition + "\"" + "," + "\"" + fullRotation + "\"" + 
            "," + tracker.timer1 + "," + tracker.timer2 + "," + tracker.timer3);
        writer.Flush();
        writer.Close();
    }

    private void Start()
    {
        tracker.baseTimer = 0;
    }

    private void Update()
    {
       
        tracker.baseTimer += Time.deltaTime;

    }

    private void FixedUpdate()
    {
        // The FPC object does a terrible job of recording the rotation, and I found the reticle tracks all three coordinates for it, so I went with that
        GameObject followMe = GameObject.Find("FirstPersonController");
        GameObject rotationRef = GameObject.Find("CrosshairAndStamina");
        // Convert Vector3 into a string resembling an array element to later convert into an array
        string convertedPosition = ("[" + followMe.transform.position.x + "," + followMe.transform.position.y + "," + followMe.transform.position.z + "]");
        string convertedRotation = ("[" + rotationRef.transform.rotation.x + "," + rotationRef.transform.rotation.y + "," + rotationRef.transform.rotation.z + "]");
        tracker.position.Add(convertedPosition);
        tracker.rotation.Add(convertedRotation);
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Pillar")
        {
            tracker.staticCollisions += 1;
            Debug.Log("Pillar hit " + tracker.staticCollisions);
        }
        else if (collision.gameObject.tag == "Wall")
        {
            tracker.staticCollisions += 1;
            Debug.Log("Wall hit " + tracker.staticCollisions);
        }
        else if (collision.gameObject.tag == "Ball")
        {
            tracker.movingCollisions += 1;
            Debug.Log("Ball hit " + tracker.movingCollisions);
        }
        else if (collision.gameObject.tag == "Goal1")
        {
            tracker.timer1 = tracker.baseTimer;
            Debug.Log("Goal At " + tracker.timer1);      
        }
        else if(collision.gameObject.tag == "Goal2")
        {
            tracker.timer2 = tracker.baseTimer;
            Debug.Log("Goal At " + tracker.timer2);
        }
        else if (collision.gameObject.tag == "Goal2")
        {
            tracker.timer3 = tracker.baseTimer;
            Debug.Log("Goal At " + tracker.timer3);
        }
    }

}
