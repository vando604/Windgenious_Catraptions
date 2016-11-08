using UnityEngine;
using System.Collections;

public class VerticalPod : MonoBehaviour
{
	[SerializeField]
	private float rotationSpeed;
	// if the min and max are on opposite sides of 0 degrees in a 0 - 360 scale, then a positive negative scale must be used (0 - 179.99 and 0 - (-180))
	[SerializeField]
	private float clockwiseLimit;
	[SerializeField]
	private float counterclockwiseLimit;
    [SerializeField]
    private float shotStrength = 1f;
    [SerializeField]
    private float shotDistance = 4f;
    [SerializeField]
	private float cooldown;
	[SerializeField]
	private Animator catPaw;
    private int playerNumber;
	private Vector2 input;
    float triggerInput;
    float cooldownTimer;

	public GameObject smoke;
	public bool inputLock;
    // Use this for initialization
    void Start ()
	{
		playerNumber = transform.parent.GetComponent<VerticalTurret>().getPlayerNumber();
		inputLock = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		collectInput();
		if (input.x != 0 || input.y != 0) 
		{
			rotatePod();
        }

        if ((triggerInput > 0) && cooldownTimer <= 0.0f)
        {
			Quaternion parentRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, transform.rotation.eulerAngles.z - 90.0f));
			GameObject bs = Instantiate(smoke, transform.position + transform.right *1.85f, parentRotation) as GameObject;
			//bs.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.parent.GetComponent<Rigidbody2D>().velocity.x*0.15f, bs.GetComponent<Rigidbody2D>().velocity.y);
            shoot();
            cooldownTimer = cooldown;
        }
        if (cooldownTimer > 0.0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

	private void collectInput()
	{
		if (!inputLock)
		{
			input.x = Input.GetAxis("RightHorizontal" + playerNumber);
			input.y = Input.GetAxis("RightVertical" + playerNumber);
			triggerInput = Input.GetAxis("RightTrigger" + playerNumber);
		}
    }

    private void rotatePod()
    {
        float angle = (Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg) - 90.0f;
        // convert angle to 0 - 360 range
        angle = angle + ((angle < 0) ? 360 : 0);
        // Debug.Log(angle);

        // clamp angle between counterclockwiseLimit and clockwiseLimit
        // left turret
        if (counterclockwiseLimit == 50 && clockwiseLimit == 305)
        {
            if (angle > 50 && angle <= 180)
            {
                angle = 50;
            }
            else if (angle > 180 && angle < 305)
            {
                angle = 305;
            }
        }
        else if (counterclockwiseLimit == 235 && clockwiseLimit == 130)
        {
            if (angle >= 0 && angle < 130)
            {
                angle = 130;
            }
            else if (angle > 235 && angle < 360)
            {
                angle = 235;
            }
        }
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void shoot()
    {
        int hitLayers = LayerMask.GetMask("Ball");
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.5f, transform.right, shotDistance, hitLayers);
		if(GetComponentInChildren<Animator>() != null)
		{
			GetComponentInChildren<Animator> ().SetTrigger ("hShoot");
		}
		else
		{
			Debug.Log ("Testicles");
		}
        if (hit.collider != null)
        {

            if (hit.transform.gameObject.tag == "Ball" || hit.transform.gameObject.tag == "PowerUp")
            {
                if(hit.transform.gameObject.tag == "Ball" && 
                    hit.transform.gameObject.GetComponent<Rigidbody2D>().gravityScale == 0.0f)
                {
                    hit.transform.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
                }

                Debug.Log(hit.transform.gameObject.GetComponent<Rigidbody2D>().isKinematic);

                Vector2 oppositeNormal = hit.normal * -1;
                float ballCannonDotProduct = Vector3.Dot(transform.right, hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity);
                float scaledShotStrength = (ballCannonDotProduct >= 0) ? shotStrength : shotStrength * (1 + Mathf.Abs(ballCannonDotProduct));
                Debug.Log((1 + Mathf.Abs(ballCannonDotProduct)));
                Debug.Log((new Vector2(transform.right.x, transform.right.y) + oppositeNormal).normalized * scaledShotStrength);
                hit.transform.gameObject.GetComponent<Rigidbody2D>().AddForce((new Vector2(transform.right.x, transform.right.y) + oppositeNormal).normalized * scaledShotStrength, ForceMode2D.Impulse);
            }
        }
		if (catPaw != null)
		{
			catPaw.SetTrigger("push");
		}

		GameObject soundEngine = GameObject.FindGameObjectWithTag("SoundEngine");
		if (soundEngine != null)
		{
			soundEngine.GetComponent<SoundEngine>().playHorn();
		}
    }
}
