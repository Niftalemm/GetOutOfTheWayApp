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
        public int goalsHit = 0;
    }

    collectedData tracker = new collectedData();
    
    /// <summary>
    /// This function writes data that has been gathered into a collectedData object to a .csv file. This data should only be written once per scene/level. 
    /// I was not successful in getting the CSVHelper plugin to work. Would recommend looking into it to help with this. 
    /// </summary>
    private void WriteToCSV()
    {
        string filePath = Application.dataPath + "/results.csv";

        if (!File.Exists(filePath))
        {
            StreamWriter creativeWriter = new StreamWriter(filePath);
            creativeWriter.WriteLine("Participant,Scene,# Static Collisions,Static Timestamps,# Moving Collisions,Moving Timestamps,Location,Rotation,TTT1,TTT2,TTT3"); // Header
            creativeWriter.Flush();
            creativeWriter.Close();
        }
        
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

        string participantID = Application.dataPath + "/ParticipantID.txt";

        StreamReader getName = new StreamReader(participantID);
        string participant = getName.ReadLine();
        getName.Close();
        Debug.Log(participant);


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
        writer.WriteLine(participant + "," + currentScene + "," + tracker.staticCollisions + "," + "\"" + fullStatic + "\"" + "," + tracker.movingCollisions + "," + "\"" + fullMoving + "\"" + "," + "\"" + fullPosition + "\"" + "," + "\"" + fullRotation + "\"" + 
            "," + tracker.timer1 + "," + tracker.timer2 + "," + tracker.timer3);
        writer.Flush();
        writer.Close();
    }

    private void Start()
    {
        // When the game starts, a .txt file is created to store the ID of the participant in order to write the same ID throughout changing scenes
        string idNumber = Random.Range(100, 100000).ToString();
        if(SceneManager.GetActiveScene().name == "Level1")
        {
            string participantID = Application.dataPath + "/ParticipantID.txt";
            StreamWriter nameStorage = new StreamWriter(participantID);
            nameStorage.WriteLine(idNumber);
            nameStorage.Flush();
            nameStorage.Close();
        }

        tracker.baseTimer = 0; // Initialize game timer
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
            
        }
        else if (collision.gameObject.tag == "Ball")
        {
            tracker.movingCollisions += 1;
            tracker.movingCollisionTimes.Add(tracker.baseTimer);
           
        }
      
    }

    /// <summary>
    /// Goals require trigger detection. CSV is written to when 3 goals have been hit. 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Goal")
        {
            tracker.timer1 = tracker.baseTimer;
            tracker.goalsHit += 1;
            if (tracker.goalsHit == 3)
            {
                Debug.Log("Writing to CSV");
                WriteToCSV();
            }
        }
        else if (other.gameObject.tag == "Goal1")
        {
            tracker.timer2 = tracker.baseTimer;
            tracker.goalsHit += 1;
            if (tracker.goalsHit == 3)
            {
                WriteToCSV();
                Debug.Log("Writing to CSV");
            }
        }
        else if (other.gameObject.tag == "Goal2")
        {
            tracker.timer3 = tracker.baseTimer;
            tracker.goalsHit += 1;
            if (tracker.goalsHit == 3)
            {
                WriteToCSV();
                Debug.Log("Writing to CSV");
            }
        }
        

    }

}
