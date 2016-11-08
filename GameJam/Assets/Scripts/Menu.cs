using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
      
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void MainMenu()
    {
        SceneManager.LoadScene("Main");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("EricBuilding");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Tutorial()
    {
        Debug.Log("YOU ENTERED TUTORIAL");
        SceneManager.LoadScene("VanBuilding");
    }
}
