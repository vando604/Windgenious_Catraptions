using UnityEngine;
using System.Collections;

public class SoundEngine : MonoBehaviour
{
	[SerializeField]
	private AudioClip[] squeaks;
	[SerializeField]
	private AudioClip yarnBounce;
	[SerializeField]
	private AudioClip tennisBounce;
	[SerializeField]
	private AudioClip baseballBounce;
	[SerializeField]
	private AudioClip[] horns;
	[SerializeField]
	private AudioClip[] catMeows;
	[SerializeField]
	private AudioClip menuButton;

	// Use this for initialization
	void Start ()
	{
		InvokeRepeating("tryPlayCat", 0.0f, 1.0f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	private void tryPlayCat() {
		int coin = Random.Range(0, 3);
		if(coin == 0) {
			playCatMeow();
		}
	}

	public void playSqueak()
	{
		int randomSqueakIndex = Random.Range(0, squeaks.Length);
		AudioSource.PlayClipAtPoint(squeaks[randomSqueakIndex], Vector3.zero, 0.75f);
	}

	public void playCatMeow()
	{
		int randomCatMeowIndex = Random.Range(0, catMeows.Length);
		AudioSource.PlayClipAtPoint(catMeows[randomCatMeowIndex], Vector3.zero, 1.0f);
	}

	public void playHorn()
	{
		int randomHornIndex = Random.Range(0, horns.Length);
		AudioSource.PlayClipAtPoint(horns[randomHornIndex], Vector3.zero, 0.75f);
	}

	public void playYarnBounce()
	{
		AudioSource.PlayClipAtPoint(yarnBounce, Vector3.zero, 0.5f);
	}

	public void playTennisBounce()
	{
		AudioSource.PlayClipAtPoint(tennisBounce, Vector3.zero);
	}

	public void playBaseballBounce()
	{
		AudioSource.PlayClipAtPoint(baseballBounce, Vector3.zero);
	}

	public void playMenuButton()
	{
		AudioSource.PlayClipAtPoint(menuButton, Vector3.zero);
	}
}
