using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    public GameObject PausePanel;
    public bool isPaused = false;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (isPaused)
        {
            PauseGame(true);
        }
        else
        {
            PauseGame(false);
        }

        if (Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("Start"))
        {
            SwitchPause();
        }
    }

    public void PauseGame(bool state)
    {
        if (state)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
        PausePanel.SetActive(state);
    }


    public void SwitchPause()
    {
        if (isPaused)
        {
            isPaused = false;
        }
        else
        {
            isPaused = true;
        }
    }
}
