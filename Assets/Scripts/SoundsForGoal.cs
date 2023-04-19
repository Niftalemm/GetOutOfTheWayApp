using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsForGoal : MonoBehaviour
{
    [SerializeField] public AK.Wwise.Event startMakingNoise;
    [SerializeField] public AK.Wwise.Event stopMakingNoise;
    [SerializeField] public AK.Wwise.Event youDidGood;
    // Start is called before the first frame update
    void Start()
    {
        startMakingNoise.Post(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player Trigger Found");
            stopMakingNoise.Post(gameObject);
            youDidGood.Post(gameObject);
            this.Destroy();

        }
    }

    

    // Update is called once per frame
    void Update()
    {

    }
}
