using UnityEngine;
using System.Collections;

public class VerticalTurret : MonoBehaviour
{
    //inspector variables
    [SerializeField]
    private float moveSpeed = 5.0f;
    [SerializeField]
    private int playerNumber;
    [SerializeField]
    private GameObject upperLimit;
    [SerializeField]
    private GameObject lowerLimit;

	public bool inputLock;

    //private variables
    private Rigidbody2D turretRigidbody;
    private Vector2 movementInput;
    // Use this for initialization
    void Start()
	{
        turretRigidbody = gameObject.GetComponent<Rigidbody2D>();
		inputLock = false;
	}

    // Update is called once per frame
    void Update()
    {

        //clamp gun positions within limits
        if (transform.position.y < lowerLimit.transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, lowerLimit.transform.position.y, transform.position.z);
        }
        if (transform.position.y > upperLimit.transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, upperLimit.transform.position.y, transform.position.z);
        }

        collectInput();

        if (movementInput.y > 0)
        {
            turretRigidbody.velocity = new Vector2(turretRigidbody.velocity.x, -moveSpeed);
        }
        else if (movementInput.y < 0)
        {
            turretRigidbody.velocity = new Vector2(turretRigidbody.velocity.x, moveSpeed);
        }
        else
        {
            turretRigidbody.velocity = new Vector2(turretRigidbody.velocity.x, 0.0f);
        }

    }

	private void collectInput()
	{
		if (!inputLock)
		{
			movementInput.x = Input.GetAxis("LeftHorizontal" + playerNumber);
			movementInput.y = Input.GetAxis("LeftVertical" + playerNumber);
		}
    }

    public int getPlayerNumber()
    {
        return playerNumber;
    }
}
