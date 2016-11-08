using UnityEngine;
using System.Collections;

public class MoveBackgrounds : MonoBehaviour
{

    private GameObject[] topBackground;
    private GameObject[] bottomBackground;

    private Vector3 farTop;
    private Vector3 farBot;

    [SerializeField]
    private float movingSpeed = 20.0f;

    private void Start()
    {
        init();
    }

    private void init()
    {
        topBackground = GameObject.FindGameObjectsWithTag("TopMovingBackground");
        bottomBackground = GameObject.FindGameObjectsWithTag("BottomMovingBackground");

        farTop = topBackground[0].transform.position.x < topBackground[1].transform.position.x ?
            topBackground[0].transform.position : topBackground[1].transform.position;

        farBot = bottomBackground[0].transform.position.x > bottomBackground[1].transform.position.x ?
            bottomBackground[0].transform.position : bottomBackground[1].transform.position;
    }

    private void Update()
    {
        Time.timeScale = 1.0f;

        foreach (GameObject go in topBackground)
        {
            if (go.transform.position.x >= 26.7f)
            {
                go.transform.position = farTop;
            }
            go.gameObject.transform.Translate(Vector2.right * Time.deltaTime * movingSpeed);
        }

        foreach(GameObject go in bottomBackground)
        {
            if (go.transform.position.x <= -26.7f)
            {
                go.transform.position = farBot;
            }
            go.gameObject.transform.Translate(Vector2.left * Time.deltaTime * movingSpeed);
        }
    }
}
