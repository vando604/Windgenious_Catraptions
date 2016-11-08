using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetButtonDown("Submit") || Input.GetKeyDown(KeyCode.A))
        {
            SceneManager.LoadScene("EricBuilding");
        }

	}
}
