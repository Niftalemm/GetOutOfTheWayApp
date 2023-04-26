/// <summary>
/// This script handles various game mechanics, such as collision detection and level progression. It is attached to the player object (First Person controller).
/// </summary>

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


public class OOF : MonoBehaviour
{
    [SerializeField] public AK.Wwise.Event akOOFEvent;
    [SerializeField] public AK.Wwise.Event akGoalSuccess;
    [SerializeField] public AK.Wwise.Event akLevelSuccess;
    [SerializeField] public AK.Wwise.Event akGoalError;
    [SerializeField] public AK.Wwise.Event akThud;


    private List<GoalBehavior> goals;
    public string[] tutorialLevels = { "Level1", "Level2", "Level3" };
    public string[] levelNames = { "Level4", "Level5", "Level6", "Level7", "Level8", "Level9", "Level10", "Level11" };
    public List<int> selectedLevels; // A list of integers that correspond to the indices of selected levels
    private bool readyForNextLevel; // flag indicating player is ready for next level


    private void Start()
    {
        goals = GameObject.FindObjectsOfType<GoalBehavior>().ToList<GoalBehavior>();
        selectedLevels = new List<int>(); // Initialize selectedLevels to an empty list
        readyForNextLevel = false;

        // Load the selectedLevels list from PlayerPrefs
        if (PlayerPrefs.HasKey("SelectedLevels"))
        {
            string levelsString = PlayerPrefs.GetString("SelectedLevels");
            selectedLevels = new List<int>(System.Array.ConvertAll(levelsString.Split(','), int.Parse));
        }
    }

    /// <summary>
    /// Called when the player collides with a trigger
    /// </summary>
    /// <param name="other">The Collider component of the object that the player collided with</param>
    private void OnTriggerEnter(Collider other)
    {
        
        //If the player does not finished all 24 trials, activate goals
        if (selectedLevels.Count < 24)
        {
            Debug.Log(selectedLevels.Count);
            ActivateGoal(other);

        }
        // If all levels have been completed, go to the success screen and reset the level
        else
        {
            Debug.Log("Game Over");
            SceneManager.LoadScene("Success");
            resetLevel();
        }
       
    }

    /// <summary>
    /// Called when the player collides with an object with a Collider component (Pillar, ball and wall) attached
    /// </summary>
    /// <param name="collision">The Collision object that describes the collision</param>
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

    /// <summary>
    /// Called once per frame
    private void Update()
    {
        // If the game is ready to move on to the next level and the player presses spacebar, call LevelComplete() which goes to next level
        if (readyForNextLevel && Input.GetKeyDown(KeyCode.Space))
        {
            LevelComplete();
            Time.timeScale = 1;// Unfreeze everything in the game
            readyForNextLevel = false;


        }

    }
    /// <summary>
    /// Activate the goal recently collided with
    /// </summary>
    /// <param name="other">The object passed by the OnTriggerEnter</param>
    private void ActivateGoal(Collider other)
    {

        GoalBehavior thing;
        if (other.TryGetComponent<GoalBehavior>(out thing))
        {
            thing.Deactivate();
            goals.Remove(thing);

            // if there are no more goals, move on
            if (goals.Count == 0)
            {
                

                readyForNextLevel = true;
                Time.timeScale = 0; // Freeze everything in the game
                AkSoundEngine.StopAll();
                akLevelSuccess.Post(gameObject);


            }
            else
            {
                akGoalSuccess.Post(gameObject);
            }
        }
    }
    /// <summary>
    /// This method is called when a level is completed successfully. It triggers an AK Event that plays a sound, and loads the next level.
    /// </summary>
    private void LevelComplete()
    {

        Scene currentScene = SceneManager.GetActiveScene();

        // If the current scene is a tutorial level
        if (tutorialLevels.Contains(currentScene.name))
        {
            // Find the index of the next tutorial level in the tutorialLevels array
            int nextSceneIndex = System.Array.IndexOf(tutorialLevels, currentScene.name) + 1;
            
            // If there is a next tutorial level, load it
            if (nextSceneIndex < tutorialLevels.Length)
            {
                SceneManager.LoadScene(tutorialLevels[nextSceneIndex]);
            }
            // If there are no more tutorial levels, select a random level
            else
            {
                SelectRandomLevel();
            }
        }
        // If the current scene is not a tutorial level, select a random level
        else
        {
            SelectRandomLevel();
        }
    }

    /// <summary>
    /// Randomly selects a level from the levelNames array and loads it three times. The selected level is stored in a list to avoid selecting it again.
    /// </summary>
    private void SelectRandomLevel()
    {
        int nextLevel;
        // Loop until a level is found that has not been played three times yet
        if (selectedLevels.Count < levelNames.Length * 3)
        {
            do
            {
                nextLevel = Random.Range(0, levelNames.Length);
            } while (selectedLevels.Count(level => level == nextLevel) >= 3);

            // Add the selected level to the list of played levels
            selectedLevels.Add(nextLevel);

            // save selectedLevels to PlayerPrefs
            PlayerPrefs.SetString("SelectedLevels", string.Join(",", selectedLevels));
            PlayerPrefs.Save();

            // Load the selected level
            SceneManager.LoadScene(levelNames[nextLevel]);
        }
    }

    /// <summary>
    /// Initializes the selectedLevels list to an empty list and saves it to PlayerPrefs.
    /// </summary>
    private void resetLevel()
    {
        selectedLevels = new List<int>(); // Initialize selectedLevels to an empty list
        PlayerPrefs.SetString("SelectedLevels", string.Join(",", selectedLevels));
        PlayerPrefs.Save();
    }


    /// <summary>
    /// When the player exits the play mode using the toolbar button, reset everything
    /// </summary>
    private void OnApplicationQuit()
    {
        Debug.Log("Player has exited Play Mode");
        resetLevel();
    }

}

