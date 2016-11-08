using UnityEngine;
using System.Collections;

public class PowerupScript : MonoBehaviour
{
	[SerializeField]
	private GameObject poof;
	[SerializeField]
	private float maxVelocity;
    public enum Powerup { Window, Fan };
	public Powerup type;
	public GameObject engine;


    // Use this for initialization
    void Start ()
	{
		
    }
	
	// Update is called once per frame
	void Update ()
	{
		// clamp velocity of ball to maximum speed
		Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
		float currentVelocity = rigidbody.velocity.magnitude;
		rigidbody.velocity = rigidbody.velocity.normalized * Mathf.Clamp(currentVelocity, 0.0f, maxVelocity);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("LeftTrack"))
		{
			if (type == Powerup.Window)
			{
				engine.GetComponent<Engine> ().ToggleWindow (Engine.Side.RIGHT);
			}
			else
			{
				engine.GetComponent<Engine> ().ToggleFan (Engine.Side.LEFT);
			}
			Instantiate (poof, transform.position, Quaternion.identity);
			Destroy (gameObject);
		}
		else if (other.gameObject.CompareTag("RightTrack"))
		{
			if (type == Powerup.Window)
			{
				engine.GetComponent<Engine> ().ToggleWindow (Engine.Side.LEFT);
			}
			else
			{
				engine.GetComponent<Engine> ().ToggleFan (Engine.Side.RIGHT);
			}
			Instantiate (poof, transform.position, Quaternion.identity);
			Destroy (gameObject);
		}
			
		GameObject soundEngine = GameObject.FindGameObjectWithTag("SoundEngine");
		if (soundEngine != null)
		{
			if (type == Powerup.Window)
			{
				soundEngine.GetComponent<SoundEngine>().playBaseballBounce();
			}
			else
			{
				soundEngine.GetComponent<SoundEngine>().playTennisBounce();
			}
		}
	}
}
