using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {

	public float speed = 3.0f;

	void Start() {
	
	}
	
	// Update is called once per frame
	void Update() {
		transform.Rotate(0, speed, 0, Space.World);
	}
}
