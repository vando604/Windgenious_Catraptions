using UnityEngine;
using System.Collections;

public class BallBehaviour : MonoBehaviour
{
    private float xScale;
    private float yScale;

    private Rigidbody2D rigidBody;

    [SerializeField]
    private float maxVelocity;

    public bool IsAffectedByFan { get; set; }
	public GameObject poof;
    private void Start()
    {
        xScale = transform.localScale.x;
        yScale = transform.localScale.y;

        rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        controls();

		// clamp velocity of ball to maximum speed
        float currentVelocity = rigidBody.velocity.magnitude;
        rigidBody.velocity = rigidBody.velocity.normalized * Mathf.Clamp(currentVelocity, 0.0f, maxVelocity);
    }
		

    private void controls()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            float ranX = Random.Range(-100.0f, 100.0f);
            float ranY = Random.Range(10.0f, 100.0f);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(ranX, ranY));
        }
    }

    private IEnumerator unSquishRoutine()
    {
        while(transform.localScale.y < yScale)
        {
            returnToOriginalScale();
            yield return null;
        }
    }

    private void returnToOriginalScale()
    {
        float YScale = Mathf.MoveTowards(transform.localScale.y, yScale, 0.3f/10.0f);

        this.transform.localScale = new Vector2(xScale, YScale);
    }

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("LeftTrack"))
		{
			Instantiate (poof, transform.position, Quaternion.identity);
			Destroy (gameObject);
		}
		else if (other.gameObject.CompareTag("RightTrack"))
		{
			GameObject blow = Instantiate (poof, transform.position, Quaternion.identity) as GameObject;
			blow.transform.localScale = new Vector3 (2, 2, 0);
			Destroy (gameObject);
		}

		GameObject soundEngine = GameObject.FindGameObjectWithTag("SoundEngine");
		if (soundEngine != null)
		{
			Debug.Log("collision sound");
			soundEngine.GetComponent<SoundEngine>().playYarnBounce();
		}
	}
}
