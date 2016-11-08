using UnityEngine;
using System.Collections;
using System.Linq;

public class Wind : MonoBehaviour {

	public enum Direction {
		HORIZONTAL,
		VERTICAL
	}

    private GameObject windZone;
	public GameObject windParticles;
	private GameObject particleInstance;

	private void Start () {
		windZone = transform.GetChild(0).gameObject;
		particleInstance = null;
	}

    public void ToggleWindZone(Direction direction)
    {
		windZone.GetComponent<WindBehaviour>().Direction = direction;

		Collider2D collider = windZone.GetComponent<Collider2D>();
        if (collider.enabled)
        {
			if (particleInstance != null)
				Destroy(particleInstance);
			
            collider.enabled = false;
        }
        else
        {
			if (direction == Direction.HORIZONTAL)
			{
                Vector3 position = transform.position;

                if (transform.rotation.z == 1.0f) {
					Quaternion rotation = windParticles.transform.rotation;
					rotation.SetLookRotation(Vector3.left);
					particleInstance = Instantiate(windParticles, position, rotation) as GameObject;
				}
				else {
					particleInstance = Instantiate(windParticles, position, windParticles.transform.rotation) as GameObject;
				}
			}
			else
			{
				particleInstance = Instantiate(windParticles, transform.position, Quaternion.identity) as GameObject;
			}

			collider.enabled = true;
        }
    }
}
