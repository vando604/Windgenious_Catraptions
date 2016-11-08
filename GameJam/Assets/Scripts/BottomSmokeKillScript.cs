using UnityEngine;
using System.Collections;

public class BottomSmokeKillScript : MonoBehaviour
{



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Object.Destroy(gameObject, 0.7f);
    }
}

