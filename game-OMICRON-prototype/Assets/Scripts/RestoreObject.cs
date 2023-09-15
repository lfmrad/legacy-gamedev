using UnityEngine;
using System.Collections;

public class RestoreObject : MonoBehaviour {

	public Transform spawnCube;

	void OnTriggerExit2D (Collider2D other) {
		other.gameObject.rigidbody2D.velocity = new Vector2 (0, 0); 
		other.gameObject.rigidbody2D.angularVelocity = 0;
		other.gameObject.transform.position = new Vector2 (spawnCube.transform.position.x, spawnCube.transform.position.y + 1);
	}
}
