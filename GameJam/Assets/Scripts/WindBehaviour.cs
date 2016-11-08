using UnityEngine;
using System.Collections;

public class WindBehaviour : MonoBehaviour
{

    public Wind.Direction Direction { get; set; }
    [SerializeField]
    float windStrength = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ball")
        {
            other.GetComponent<BallBehaviour>().IsAffectedByFan = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ball")
        {
            other.GetComponent<BallBehaviour>().IsAffectedByFan = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        switch (Direction)
        {
            case Wind.Direction.VERTICAL:
                if (other.tag == "Ball")
                {
                    if (other.GetComponent<BallBehaviour>().IsAffectedByFan)
                    {
                        other.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, -10.0f));
                    }
                }
                else other.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, -10.0f));
                break;
            case Wind.Direction.HORIZONTAL:
                if (other.tag == "Ball") 
                {
                    if (other.GetComponent<Rigidbody2D>().gravityScale == 0.0f)
                    {
                        other.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
                    }
                    if (transform.position.x < 0)
                    {
                        other.GetComponent<Rigidbody2D>().AddForce(Vector2.right * windStrength);
                    }
                    else
                    {
                        other.GetComponent<Rigidbody2D>().AddForce(Vector2.left * windStrength);
                    }
                }
                    
                break;
            default:
                break;
        }

    }
}
