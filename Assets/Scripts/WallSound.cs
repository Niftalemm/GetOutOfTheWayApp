using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSound : MonoBehaviour
{
    [SerializeField] public AK.Wwise.Event wallSound;
    // Start is called before the first frame update
    void Start()
    {
        wallSound.Post(gameObject);
    }

}
