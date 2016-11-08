using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextEffect : MonoBehaviour {

	private Text text;

	private void Start () {
		text = GetComponent<Text>();
        StartCoroutine (starWars ());
	}

	private void scaleIn() {
		text.transform.localScale *= 1.15f;
	}

	private void scaleOut() {
		text.transform.localScale /= 1.15f;
	}

	private void fadeOut() {
		Color color = text.color;
		color.a -= 0.02f;
		text.color = color;
	}

	private void fadeIn() {
		Color color = text.color;
		color.a += 0.02f;
		text.color = color;
	}

	private IEnumerator starWars() {
        Engine engine = GameObject.FindGameObjectWithTag("Engine").GetComponent<Engine>();
        engine.lockAllInput(true);

        InvokeRepeating ("scaleIn", 0.1f, 0.1f);
		InvokeRepeating ("fadeOut", 0.1f, 0.1f);
		yield return new WaitForSeconds (0.7f);
		CancelInvoke ();
		InvokeRepeating ("scaleOut", 0.1f, 0.1f);
		InvokeRepeating ("fadeIn", 0.1f, 0.1f);
		yield return new WaitForSeconds (0.7f);
		CancelInvoke ();
		InvokeRepeating ("scaleIn", 0.1f, 0.1f);
		InvokeRepeating ("fadeOut", 0.1f, 0.1f);
		yield return new WaitForSeconds (0.7f);
		CancelInvoke ();
		InvokeRepeating ("scaleOut", 0.1f, 0.1f);
		InvokeRepeating ("fadeIn", 0.1f, 0.1f);
		yield return new WaitForSeconds (0.7f);
		CancelInvoke ();
		InvokeRepeating ("scaleIn", 0.1f, 0.1f);
		InvokeRepeating ("fadeOut", 0.1f, 0.1f);
		yield return new WaitForSeconds (1.5f);
		CancelInvoke ();
		Destroy (text.gameObject);

        engine.lockAllInput(false);
    }
}
