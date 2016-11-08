using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Engine : MonoBehaviour
{

    public enum Side
    {
        LEFT,
        RIGHT
	}

    [SerializeField]
    private int winningScore;
    [SerializeField]
	private float powerUpInterval;
	[SerializeField]
	private Sprite windowSprite;
	[SerializeField]
	private Sprite fanSprite;
	[SerializeField]
	private GameObject powerUpPrefab;
	[SerializeField]
	private float painMover = 1f;

    private Vector3 leftSpawn;
    private Vector3 rightSpawn;

    private GameObject leftFan;
    private GameObject rightFan;
    private GameObject leftWindow;
    private GameObject rightWindow;

    private GameObject basket;

    private Team leftTeam;
    private Team rightTeam;

    private GameObject score;
	public GameObject leftWindowPain;
	public GameObject rightWindowPain;
	float timeSinceStarted1 = 0f;
	float timeSinceStarted2 = 0f; 

    private void Start()
    {
		initialize();
		InvokeRepeating ("generatePowerup", powerUpInterval * 1.5f, powerUpInterval);
    }

    private void Update()
    {
        //Debug
        if(Input.GetKeyDown(KeyCode.Q))
        {
            ToggleFan(Side.LEFT);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            ToggleFan(Side.RIGHT);
        }
        else if(Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(basketDrop());
        }

		if(Input.GetKeyDown(KeyCode.E))
		{
			ToggleWindow(Side.LEFT);
		}
		else if (Input.GetKeyDown(KeyCode.R))
		{
			ToggleWindow(Side.RIGHT);
		}
		else if (Input.GetKeyDown(KeyCode.L)) {
			GameObject newPowerUp = Instantiate (powerUpPrefab, transform.position, Quaternion.identity) as GameObject;
			newPowerUp.GetComponent<PowerupScript> ().type = PowerupScript.Powerup.Fan;
			newPowerUp.GetComponent<PowerupScript> ().engine = gameObject;
		}
    }

	private void generatePowerup()
	{
		StartCoroutine (basketDrop ());
	}

    private void initialize()
    {
        Random.InitState(System.Environment.TickCount);

        GameObject[] spawns = GameObject.FindGameObjectsWithTag("BallSpawn");
        spawns.OrderBy(s => s.transform.position.x);
        leftSpawn = spawns[0].transform.position;
        rightSpawn = spawns[1].transform.position;

        GameObject[] fans = GameObject.FindGameObjectsWithTag("VerticalFan");
        fans.OrderBy(f => f.transform.position.x);
        leftFan = fans[0];
        rightFan = fans[1];

        GameObject[] windows = GameObject.FindGameObjectsWithTag("Window");
        windows.OrderBy(w => w.transform.position.x);
        leftWindow = windows[0];
        rightWindow = windows[1];

        basket = GameObject.FindGameObjectWithTag("Basket");

        leftTeam = new Team();
        rightTeam = new Team();

		score = GameObject.FindGameObjectWithTag("Score");

        int coin = Random.Range(0, 2);
        if (coin == 0) spawnBall(leftSpawn);
        else spawnBall(rightSpawn);
    }

    private void spawnBall(Vector3 spawnPoint)
    {
        GameObject ball = Resources.Load("Prefabs/eric_testicle") as GameObject;
        GameObject instance = Instantiate(ball, spawnPoint, Quaternion.identity) as GameObject;
    }

    public void IncrementTeamScore(Side side, int amount)
    {
        switch (side)
        {
            case Side.LEFT:
                leftTeam.IncrementScore(amount);
                spawnBall(rightSpawn);
                break;
            case Side.RIGHT:
                rightTeam.IncrementScore(amount);
                spawnBall(leftSpawn);
                break;
            default:
                break;
        }

        if (leftTeam.Score == winningScore || rightTeam.Score == winningScore)
        {
            SceneManager.LoadScene("Main");
        }
        

		score.GetComponent<Text>().text = string.Format("{0} : {1}", leftTeam.Score, rightTeam.Score);
    }

    public void ToggleFan(Side side)
    {
        switch (side)
        {
		case Side.LEFT:
			leftFan.GetComponent<Wind> ().ToggleWindZone (Wind.Direction.VERTICAL);
			leftFan.GetComponent<Animator> ().SetBool ("FanOn", true);	

                StartCoroutine(turnOffFan(leftFan));
                break;
		case Side.RIGHT:
			rightFan.GetComponent<Wind> ().ToggleWindZone (Wind.Direction.VERTICAL);
			rightFan.GetComponent<Animator> ().SetBool("FanOn", true);
				
			    StartCoroutine(turnOffFan(rightFan));
                break;
            default:
                break;
        }
    }

    public void ToggleWindow(Side side)
    {
        switch (side)
        {
		case Side.LEFT:
			
			leftWindow.GetComponent<Wind> ().ToggleWindZone (Wind.Direction.HORIZONTAL);
			leftWindowPain.GetComponent<Animator> ().SetTrigger ("Up");
			StartCoroutine (closeWindow (leftWindow,0));
                break;

		case Side.RIGHT:
			
			rightWindow.GetComponent<Wind> ().ToggleWindZone (Wind.Direction.HORIZONTAL);
			rightWindowPain.GetComponent<Animator> ().SetTrigger ("Up");
			StartCoroutine (closeWindow (rightWindow, 1));
            break;

        default:
           break;
        }
    }

	public void lockAllInput(bool lockInput)
	{
		List<GameObject> lockObjects = new List<GameObject>();
		lockObjects.AddRange(GameObject.FindGameObjectsWithTag("BottomGun"));
		lockObjects.AddRange(GameObject.FindGameObjectsWithTag("WallGun"));

		foreach (GameObject lockObject in lockObjects)
		{
			VerticalPod tempVP = lockObject.GetComponent<VerticalPod>();
			if (tempVP != null)
			{
				tempVP.inputLock = lockInput;
			}

			VerticalTurret tempVT = lockObject.GetComponent<VerticalTurret>();
			if (tempVT != null)
			{
				tempVT.inputLock = lockInput;
			}

			SqueakyGun tempSG = lockObject.GetComponent<SqueakyGun>();
			if (tempSG != null)
			{
				tempSG.inputLock = lockInput;
			}
		}
	}

    private IEnumerator turnOffFan(GameObject fan)
    {
        yield return new WaitForSeconds(20.0f);
        fan.GetComponent<Wind>().ToggleWindZone(Wind.Direction.VERTICAL);
		fan.GetComponent<Animator> ().SetBool ("FanOn", false);	
    }

	private IEnumerator closeWindow(GameObject window, int num)
    {
        yield return new WaitForSeconds(20.0f);
        window.GetComponent<Wind>().ToggleWindZone(Wind.Direction.HORIZONTAL);
    }

    private IEnumerator basketDrop()
    {
        float distanceRemaining = 2.0f;
        while(distanceRemaining >= 0.0f)
        {
            basket.transform.Translate(new Vector3(0.0f, -0.05f, 0.0f));
            distanceRemaining -= 0.05f;
            yield return new WaitForSeconds(0.025f);
        }

        StartCoroutine(basketToggle());

        yield return new WaitForSeconds(1.0f);

        while (distanceRemaining <= 2.0f)
        {
            basket.transform.Translate(new Vector3(0.0f, 0.05f, 0.0f));
            distanceRemaining += 0.05f;
            yield return new WaitForSeconds(0.025f);
        }
    }

    private IEnumerator basketToggle()
    {
        Transform[] pivots = basket.GetComponentsInChildren<Transform>();
        GameObject leftPivot = pivots.First(x => x.tag == "LeftBasketPivot").gameObject;
        GameObject rightPivot = pivots.First(x => x.tag == "RightBasketPivot").gameObject;

        float angleRemaining = 90.0f;

        while(angleRemaining >= 0.0f)
        {
            leftPivot.transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f), -2.0f);
            rightPivot.transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f), 2.0f);
            angleRemaining -= 2.0f;
            yield return new WaitForSeconds(0.01f);
        }
			
		float randomFloat = Random.Range(0f, 1f);
		PowerupScript.Powerup powerupType = (randomFloat >= 0.5f) ? PowerupScript.Powerup.Window : PowerupScript.Powerup.Fan;
		GameObject newPowerUp = Instantiate (powerUpPrefab, transform.position, Quaternion.identity) as GameObject;
		newPowerUp.GetComponentInChildren<SpriteRenderer>().sprite = (powerupType == PowerupScript.Powerup.Window) ? windowSprite : fanSprite;
		newPowerUp.GetComponent<PowerupScript> ().type = powerupType;
		newPowerUp.GetComponent<PowerupScript> ().engine = gameObject;

        yield return new WaitForSeconds(1.0f);

        while (angleRemaining <= 90.0f)
        {
            leftPivot.transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f), 2.0f);
            rightPivot.transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f), -2.0f);
            angleRemaining += 2.0f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    internal class Team
    {
        private int score;
        public int Score { get { return score; } }

        public Team()
        {
            score = 0;
        }

        public void IncrementScore(int amount)
        {
            score += amount;
        }
    }
}
