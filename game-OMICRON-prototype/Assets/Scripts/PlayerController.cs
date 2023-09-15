using UnityEngine;
using System.Collections;

public class PlayerController : Nature {

	bool inField;
	Properties properties;
	Light statusLight;
	Charge defaultCharge;

	void Start () {

		properties = GetComponent<Properties>();
		defaultCharge = properties.charge;
		statusLight = GetComponentInChildren<Light>();
		unloadedColor = defaultCharge == Charge.alpha ? betaChargeColor : alphaChargeColor;
		statusLight.color = loadedColor;

		// ChangeLevel (Level.testLevel, gameObject);
		
		renderer.material.color = Color.black;
	}
	public float maxTimeCharged;
	float elapsedTimeWhileCharged = 0;
	public float maxTimeRecovering;
	float elapsedTimeRecovering = 0;
	bool isRecovered = true;

	float proportionalRecoveringTime;
	float lastElapsedTimeWhileCharged;

	string key;

	void Update () {

		if (Input.GetKey(KeyCode.Space) && isRecovered) { 
			properties.charge = defaultCharge == Charge.alpha ? Charge.beta : Charge.alpha;
			elapsedTimeWhileCharged += Time.deltaTime;
			isRecovered = elapsedTimeWhileCharged >= maxTimeCharged ? false : true; 
			statusLight.color = unloadedColor;
			
			lastElapsedTimeWhileCharged = elapsedTimeWhileCharged;
			
			// Debug.Log ("Charged" + elapsedTimeWhileCharged);
		} else if (Input.GetKeyUp(KeyCode.Space)) {
			isRecovered = false;
		} else {
			properties.charge = defaultCharge;
			proportionalRecoveringTime = maxTimeRecovering * lastElapsedTimeWhileCharged / maxTimeCharged ;
			elapsedTimeWhileCharged = 0;
			if (!isRecovered) {
				if (elapsedTimeRecovering < proportionalRecoveringTime) {
					elapsedTimeRecovering += Time.deltaTime;		
					LightColorChanger(elapsedTimeRecovering/proportionalRecoveringTime,false);
					// Debug.Log ("Recovering: " + elapsedTimeRecovering);	
				} else {
					isRecovered = true;
					elapsedTimeRecovering = 0;
				}
			}
		}	
//		LEVEL CHANGER - DOESN'T WORK YET
//		if (Input.GetKey (KeyCode.Alpha0)) {
//			ChangeLevel((int)Level.testLevel, gameObject);
//			Debug.Log ((int)Level.level1);
//		} else if (Input.GetKey (KeyCode.Alpha1)) {
//			ChangeLevel((int)Level.level1, gameObject);
//		}
	}

	bool onSurface;

	Transform groundTransform;
	GameObject contactedObject;

	void OnCollisionEnter2D (Collision2D other) {

		onSurface = true;
		groundTransform = other.gameObject.transform;
		//Debug.Log("ground");
	}


	void OnCollisionExit2D () {

		onSurface = false;
		//Debug.Log("air");
	}

	public enum ControlMode{fieldDependent, groundAndAir, xForce}; 
	public ControlMode controlMode;

	float moveHorizontal;
	float xVelocity;
	Vector2 velocity;	
	float velocityMagnitude;

	public float forceMagnitude;
	public float topAirXSpeed;
	public float topSurfaceSpeed;
	
	bool xVelocityUnderLimit;
	bool velocityUnderLimit;
	bool directionHasChanged;

	Vector2 forceDirection;
	
	void FixedUpdate () {
		// Debug.Log (fieldTransform.right);

		moveHorizontal = Input.GetAxisRaw ("Horizontal");

		xVelocity = rigidbody2D.velocity[0];
		velocity = rigidbody2D.velocity;
		velocityMagnitude = Mathf.Sqrt(Mathf.Pow(velocity.x, 2) + Mathf.Pow(velocity.y, 2));

		xVelocityUnderLimit = Mathf.Abs(xVelocity) < topAirXSpeed;
		velocityUnderLimit = velocityMagnitude < topSurfaceSpeed;
		directionHasChanged = (xVelocity * moveHorizontal < 0);


		switch (controlMode) {
			case ControlMode.fieldDependent:
			if (onSurface && (velocityUnderLimit || directionHasChanged)) {
				
				forceDirection = new Vector2 (Mathf.Abs(groundTransform.right.x), groundTransform.right.y);
				
				rigidbody2D.AddForce(moveHorizontal * forceMagnitude * forceDirection);
			} else if (xVelocityUnderLimit || directionHasChanged) {
					rigidbody2D.AddForce(Vector2.right * moveHorizontal * forceMagnitude);
			}
			break;
		}
//		Debug.Log ("V = " + velocityMagnitude + " / " + topOnSurfaceSpeed + " - " +
//		           "Vx = " + xVelocity + " / " + topOnAirXSpeed);
	}

// PROPERTIES

	[HideInInspector]	
	public Color unloadedColor;
	public Color loadedColor;

	void LightColorChanger (float time, bool reverse) {
		Color initial, final;
		
		if (!reverse) { 
			initial = unloadedColor; final = loadedColor;
		} else {
			initial = loadedColor; final = unloadedColor;
		}
		
		statusLight.color = Color.Lerp(initial, final, time) ; 
	}
}