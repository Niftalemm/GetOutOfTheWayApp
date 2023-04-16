using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;



public class CollisionTracker : MonoBehaviour
{


    
    public class collectedData
    {
        public int staticCollisions = 0;
        public int movingCollisions = 0;
        public List<double> staticCollisionTimes = new List<double>();
        public List<double> movingCollisionTimes = new List<double>();  
        public List<string> position = new List<string>();
        public List<string> rotation = new List<string>();
        public float baseTimer;
        public float timer1 = 0;
        public float timer2 = 0;
        public float timer3 = 0;
    }

    collectedData tracker = new collectedData();
    
    /// <summary>
    /// This function writes data that has been gathered into a collectedData object to a .csv file. This data should only be written once per scene/level. 
    /// I was not successful in getting the CSVHelper plugin to work. Would recommend looking into it to help with this. 
    /// </summary>
    private void WriteToCSV()
    {
        string filePath = Application.dataPath + "/results.csv";
        
        StreamWriter creativeWriter = new StreamWriter(filePath);
        creativeWriter.WriteLine("Participant,Scene,# Static Collisions,Static Timestamps,# Moving Collisions,Moving Timestamps,Location,Rotation,TTT1,TTT2,TTT3"); // Header
        creativeWriter.Flush();
        creativeWriter.Close();
        
        // List<> is converted to array for easier loops. 
        string[] tempPosition = tracker.position.ToArray();
        string[] tempRotation = tracker.rotation.ToArray();
        double[] tempStatic = tracker.staticCollisionTimes.ToArray();
        double[] tempMoving = tracker.movingCollisionTimes.ToArray();
        string fullStatic = "[";
        string fullMoving = "[";
        string fullPosition = "";
        string fullRotation = "";
        string currentScene = SceneManager.GetActiveScene().name;

        // For every List<> (now an array), iterate through and convert to a single string

        for (int i = 0; i < tempStatic.Length; i++)
        {
            if (i == tempStatic.Length - 1)
            {
                fullStatic += tempStatic[i].ToString();
            }
            else
            {
                fullStatic += tempStatic[i].ToString() + ", ";
            }
        }
        fullStatic += "]";

        for (int i = 0; i < tempMoving.Length; i++)
        {
            if (i == tempMoving.Length - 1)
            {
                fullMoving += tempMoving[i].ToString();
            }
            else
            {
                fullMoving += tempMoving[i].ToString() + ", ";
            }
        }
        fullMoving += "]";

        for (int i = 0; i < tempPosition.Length; i++)
        {
            if (i == tempPosition.Length - 1)
            {
                fullPosition += tempPosition[i];
            }
            else
            {
                fullPosition += tempPosition[i] + ", ";
            }
        }
        

        for (int i = 0; i < tempRotation.Length; i++)
        {
            if (i == tempRotation.Length - 1)
            {
                fullRotation += tempRotation[i];
            }
            else
            {
                fullRotation += tempRotation[i] + ", ";
            }
        }
        
        //This will append to the CSV file, creating a new row, hence why WriteCSV should be called once, at the end of a level
        StreamWriter writer = File.AppendText(filePath);
        writer.WriteLine("Goober McGee" + "," + currentScene + "," + tracker.staticCollisions + "," + "\"" + fullStatic + "\"" + "," + tracker.movingCollisions + "," + "\"" + fullMoving + "\"" + "," + "\"" + fullPosition + "\"" + "," + "\"" + fullRotation + "\"" + 
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
       // Cumulative time between each frame, calculated each frame, for total run time of a level
        tracker.baseTimer += Time.deltaTime;

    }

    private void FixedUpdate()
    {
        // The FPC object does a terrible job of recording the rotation, and I found the reticle tracks all three coordinates for it, so I went with that
        GameObject followMe = GameObject.Find("FirstPersonController");
        GameObject rotationRef = GameObject.Find("CrosshairAndStamina");
        // Convert Vector3 into a string resembling an array element to later convert into an array
        string convertedPosition = ("[" + followMe.transform.position.x + "|" + followMe.transform.position.y + "|" + followMe.transform.position.z + "]");
        string convertedRotation = ("[" + rotationRef.transform.rotation.x + "|" + rotationRef.transform.rotation.y + "|" + rotationRef.transform.rotation.z + "]");
        tracker.position.Add(convertedPosition);
        tracker.rotation.Add(convertedRotation);
    }

    /// <summary>
    /// Checks the object tag of the object collided with and takes action (collects certain data) based on which object was hit
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Pillar" || collision.gameObject.tag == "Wall")
        {
            tracker.staticCollisions += 1;
            tracker.staticCollisionTimes.Add(tracker.baseTimer);
            WriteToCSV();
            Debug.Log("Pillar hit " + tracker.staticCollisions);
        }
        else if (collision.gameObject.tag == "Ball")
        {
            tracker.movingCollisions += 1;
            tracker.movingCollisionTimes.Add(tracker.baseTimer);
            Debug.Log("Ball hit " + tracker.movingCollisions);
        }
        else if (collision.gameObject.tag == "Goal")
        {
            tracker.timer1 = tracker.baseTimer;
            Debug.Log("Goal At " + tracker.timer1);
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
