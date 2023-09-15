using UnityEngine;
using System.Collections;

public class AttachObject : MonoBehaviour {
		
	Transform platformTransform;
	bool attach = false;

	void Start () {
	
		if (transform.tag == "MovingPlatform") { attach = true; }
	}
	
	void OnTriggerEnter2D (Collider2D other) {
		
		if (attach) { other.gameObject.transform.parent = transform; }
	}

	void OnTriggerExit2D (Collider2D other) {
		
		if (attach) { other.gameObject.transform.parent = null; }
	}
	
}
