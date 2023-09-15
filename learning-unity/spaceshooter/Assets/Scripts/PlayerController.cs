using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
	public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
	public float speed;
	public float tilt;
	public Boundary boundary;
	
	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	
	private float nextFire;

	// BARREL ROLL TEST
	public float rollSeconds; // tiempo que quieres que tarde en hacerlo
	bool rollEnabled = false;
	float delta = 0;
	float angle;
	// ----------------

	void Update ()
	{
		if (Input.GetButton("Fire1") && Time.time > nextFire)
		{
			nextFire = Time.time + fireRate;
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
			audio.Play ();
		}
	}
	
	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rigidbody.velocity = movement * speed;
		
		rigidbody.position = new Vector3 
			(
				Mathf.Clamp (rigidbody.position.x, boundary.xMin, boundary.xMax), 
				0.0f, 
				Mathf.Clamp (rigidbody.position.z, boundary.zMin, boundary.zMax)
				);
		
		rigidbody.rotation = Quaternion.Euler (0.0f, 0.0f, rigidbody.velocity.x * -tilt);	

		// BARREL ROLL ------------------------------
		if(Input.GetKeyDown (KeyCode.Space)) {
			rollEnabled = true;
		}
		
		if(rollEnabled && (delta < 1.0f)) {
			delta += Time.deltaTime / rollSeconds;
			angle = Mathf.Lerp(0, 359, delta);
			Debug.Log (angle);
			rigidbody.rotation = Quaternion.Euler (Vector3.forward * angle);
		} else {
			delta = 0; rollEnabled = false;
		}
		// -----------------------------------------
	}
}