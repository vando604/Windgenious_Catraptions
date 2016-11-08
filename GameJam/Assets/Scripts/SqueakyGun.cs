using UnityEngine;
using System.Collections;

public class SqueakyGun : MonoBehaviour {

    //inspector variable
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private GameObject leftLimit;
    [SerializeField]
    private GameObject rightLimit;
    public GameObject bottomSmoke;
    public float shotDistance = 4f;
    public bool isLeft;
    public float shotStrength = 15f;
    [SerializeField]
    private float cooldown;
    [SerializeField]
	private float disabledCooldown = 5.0f;
	[SerializeField]
	private Animator catHead;
	[SerializeField]
	private Animator catTail;
	[SerializeField]
	private Animator catPaw;
    private bool canMove;

	public bool inputLock;

    //private variables
    private Rigidbody2D gunRigidBody;


    //data variables
    public int playerNumber;
    Vector2 movementInput;
    float triggerInput;
    float cooldownTimer;

    // Use this for initialization
    void Start ()
    {
        gunRigidBody = gameObject.GetComponent<Rigidbody2D>();
        canMove = true;
		inputLock = false;
 	}
	
	// Update is called once per frame
	void Update ()
    {
        collectInput();

        if (canMove)
        {
            //moving right
            if (movementInput.x > 0)
            {
				gunRigidBody.velocity = new Vector2(moveSpeed, gunRigidBody.velocity.y);
				if (catTail != null)
				{
					catTail.SetBool("isWaggingTail", true);
				}
            }
            //moving left
            else if (movementInput.x < 0)
            {
				gunRigidBody.velocity = new Vector2(-moveSpeed, gunRigidBody.velocity.y);
				if (catTail != null)
				{
					catTail.SetBool("isWaggingTail", true);
				}
            }
            //idle
            else
            {
                gunRigidBody.velocity = new Vector2(0.0f, gunRigidBody.velocity.y);
				if (catTail != null)
				{
					catTail.SetBool("isWaggingTail", false);
				}
            }
        }
        else
        {
            gunRigidBody.velocity = new Vector2(0.0f, gunRigidBody.velocity.y);
        }

        // clamp gun position within limits
        if (transform.position.x < leftLimit.transform.position.x)
        {
            transform.position = new Vector3(leftLimit.transform.position.x, transform.position.y, transform.position.z);
            gunRigidBody.velocity = new Vector2(0.0f, gunRigidBody.velocity.y);
        }
        if (transform.position.x > rightLimit.transform.position.x)
        {
            transform.position = new Vector3(rightLimit.transform.position.x, transform.position.y, transform.position.z);
            gunRigidBody.velocity = new Vector2(0.0f, gunRigidBody.velocity.y);
        }

        
        // fire piston (smoke must be given velocity of turret before velocity is adjusted kinematically)
        if ((triggerInput > 0) && cooldownTimer <= 0.0f && canMove)
        {
            GameObject bs = Instantiate(bottomSmoke, new Vector3(this.transform.position.x, this.transform.position.y + 1.85f, 0), Quaternion.identity) as GameObject;
            bs.GetComponent<Rigidbody2D>().velocity = new Vector2(gunRigidBody.velocity.x * 0.15f, bs.GetComponent<Rigidbody2D>().velocity.y);
            cooldownTimer = cooldown;
            shoot();
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
			movementInput.x = Input.GetAxis( ((isLeft) ? "LeftHorizontal" : "RightHorizontal") + playerNumber);
			movementInput.y = Input.GetAxis(((isLeft) ? "LeftHorizontal" : "RightHorizontal") + playerNumber);
			triggerInput = Input.GetAxis(((isLeft) ? "LeftTrigger" : "RightTrigger") + playerNumber);
		}
    }


    private void shoot()
    {
        int hitLayers = LayerMask.GetMask("Ball");
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.4f, Vector2.up, shotDistance, hitLayers);
       
        if (hit.collider != null)
        {
            if(hit.transform.gameObject.tag == "Ball" || hit.transform.gameObject.tag == "PowerUp")
            {
                if (hit.transform.gameObject.tag == "Ball" &&
                    hit.transform.gameObject.GetComponent<Rigidbody2D>().gravityScale == 0.0f) 
                {
                    hit.transform.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
                }

                Vector2 oppositeNormal = hit.normal * -1;
                hit.transform.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                hit.transform.gameObject.GetComponent<Rigidbody2D>().AddForce((Vector2.up + oppositeNormal).normalized * shotStrength, ForceMode2D.Impulse);
            }
            
        }
		if (GetComponent<Animator>() != null)
		{
			GetComponent<Animator>().SetTrigger("Shoot");
		}
		if (catHead != null)
		{
			catHead.SetTrigger("BobHead");
		}
		if (catPaw != null)
		{
			catPaw.SetTrigger("PushButton");
		}
			
		GameObject soundEngine = GameObject.FindGameObjectWithTag("SoundEngine");
		if (soundEngine != null)
		{
			soundEngine.GetComponent<SoundEngine>().playSqueak();
		}
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ball" && canMove)
        {
            StartCoroutine(freeze(disabledCooldown));
        }
    }

    private IEnumerator freeze(float time)
    {
        canMove = false;
        //StartCoroutine(flicker(time));
        StartCoroutine(flicker(time));
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    private IEnumerator flicker(float time)
    {
        int nTimes = (int)time * 2;
        float timeOn = 0.25f;
        while(nTimes > 0)
        {
            StartCoroutine(fadeTo(0.0f, timeOn));
            yield return new WaitForSeconds(timeOn);
            StartCoroutine(fadeTo(1.0f, timeOn));
            yield return new WaitForSeconds(timeOn);
            nTimes--;
        }
    }

    private IEnumerator fadeTo(float aValue, float aTime)
    {
        float alpha = GetComponent<Renderer>().material.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1.0f, 0.25f, 0.25f, Mathf.Lerp(alpha, aValue, t));
            GetComponent<Renderer>().material.color = newColor;
            yield return null;
        }
        Color tempColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        GetComponent<Renderer>().material.color = tempColor;
    }

    
}
