using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using System.Xml.Serialization;
using System.Linq;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using UnityEngine.SceneManagement;

// remove goals list
// Actvate goal
// make sure goals get activated

public class OOF : MonoBehaviour
{
    [SerializeField] public AK.Wwise.Event akOOFEvent;
    [SerializeField] public AK.Wwise.Event akGoalSuccess;
    [SerializeField] public AK.Wwise.Event akLevelSuccess;
    [SerializeField] public AK.Wwise.Event akGoalError;
    [SerializeField] public AK.Wwise.Event akThud;
    [SerializeField] public AK.Wwise.Event stopMakingNoise;


    private List<GoalBehavior> goals;
    private int goalCounter;
    private Rigidbody rb;
    public string[] tutorialLevels = { "Level1", "Level2", "Level3" };
    public string[] levelNames = { "Level4", "Level5", "Level6", "Level7", "Level8", "Level9", "Level10", "Level11" };
    public List<int> selectedLevels;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        goals = GameObject.FindObjectsOfType<GoalBehavior>().ToList<GoalBehavior>();
        goalCounter = GameObject.FindObjectsOfType<GoalBehavior>().Length;
        selectedLevels = new List<int>(); // Initialize selectedLevels to an empty list

        // Load the selectedLevels list from PlayerPrefs
        if (PlayerPrefs.HasKey("SelectedLevels"))
        {
            string levelsString = PlayerPrefs.GetString("SelectedLevels");
            selectedLevels = new List<int>(System.Array.ConvertAll(levelsString.Split(','), int.Parse));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        akThud.Post(gameObject);
        ActivateGoal(other);
   
        if (selectedLevels.Count < 24)
        {
            Debug.Log(selectedLevels.Count);
            // ActivateGoal();
        }
        else
        {
            Debug.Log("Game Over");
            SceneManager.LoadScene("Success");
            resetLevel();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Pillar" || collision.gameObject.tag == "Ball")
        {
            akOOFEvent.Post(gameObject);
        }
        if (collision.gameObject.tag == "Wall")
        {
            akThud.Post(gameObject);
        }
    }
    

    private void Update()
    {
        // this probably could stand to have the input thingy removed but we do not have time
        // maybe even remove this method but that's messing with other people's code and I dont have time to deal with if it gets messed up
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (selectedLevels.Count < 24)
            {
                Debug.Log(selectedLevels.Count);
                // ActivateGoal();
            }
            else
            {
                Debug.Log("Game Over");
                SceneManager.LoadScene("Success");
                resetLevel();
            }

        }
    }
    private void ActivateGoal(Collider other)
    {

        GoalBehavior thing;
        if (other.TryGetComponent<GoalBehavior>(out thing))
        {
            thing.Deactivate();
            goals.Remove(thing);
            akGoalSuccess.Post(gameObject);
        }

        if (goals.Count == 0)
        {
            // check current level
            Scene currentScene = SceneManager.GetActiveScene();
            Debug.Log(currentScene.name);
            // If the current level is tutorial scene
            if (tutorialLevels.Contains(currentScene.name))
            {
                int nextSceneIndex = System.Array.IndexOf(tutorialLevels, currentScene.name) + 1;
                if (nextSceneIndex < tutorialLevels.Length)
                {
                    // go to next tutorial level
                    SceneManager.LoadScene(tutorialLevels[nextSceneIndex]);
                }
                else
                {
                    selectRandomLevel();
                }
            }
            else
            {
                selectRandomLevel();
            }
        }
    }
    // Randomly generate a level, and store level name into a list
    private void selectRandomLevel()
    {
        // load a random level
        int nextLevel;
        if (selectedLevels.Count < levelNames.Length * 3)
        {
            do
            {
                nextLevel = Random.Range(0, levelNames.Length);
            } while (selectedLevels.Count(level => level == nextLevel) >= 3);

            selectedLevels.Add(nextLevel);
            // save selectedLevels to PlayerPrefs
            PlayerPrefs.SetString("SelectedLevels", string.Join(",", selectedLevels));
            PlayerPrefs.Save();
            SceneManager.LoadScene(levelNames[nextLevel]);
        }
    }
    private void resetLevel()
    {
        selectedLevels = new List<int>(); // Initialize selectedLevels to an empty list
        PlayerPrefs.SetString("SelectedLevels", string.Join(",", selectedLevels));
        PlayerPrefs.Save();
    }
    // When the player exit the play mode using the toolbar button, reset everything
    private void OnApplicationQuit()
    {
        Debug.Log("Player has exited Play Mode");
        resetLevel();
    }

}

