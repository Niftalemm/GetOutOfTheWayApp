using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;

public class CollisionDetect : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        // AkSoundEngine.PostEvent("Stop_Engine_3", gameObject);
        Destroy(gameObject);
    }
}