using UnityEngine;
using System.Collections;

public class FloorBehaviour : MonoBehaviour {

    private Engine engine;

	private void Start () {
        engine = GameObject.FindGameObjectWithTag("Engine").GetComponent<Engine>();
	}

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ball")
        {
            Destroy(other.gameObject);

            switch (gameObject.tag)
            {
                case "LeftTrack":
					engine.IncrementTeamScore(Engine.Side.RIGHT, 1);
                    break;
                case "RightTrack":
					engine.IncrementTeamScore(Engine.Side.LEFT, 1);    
                    break;
                default:
                    break;
            }
        }
    }
}
