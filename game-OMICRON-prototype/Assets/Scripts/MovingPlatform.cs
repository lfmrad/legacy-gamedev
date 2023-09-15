using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingPlatform : MonoBehaviour {

	float initialPositionX;
	Vector3 forceDirection;
	int currentWayPoint = 1;
	float aux;
	void Start () {

		initialPositionX = transform.position.x;
		// gameObject.AddComponent<Rigidbody2D>();
		// rigidbody2D.isKinematic = true;
		// rigidbody2D.gravityScale = 0;
		//wayPoints[0].position = transform.position;

		// forceDirection = wayPoints[currentWayPoint].position - transform.position;
		//forceDirection = new Vector3 (Mathf.Abs(forceDirection.x), Mathf.Abs(forceDirection.y), Mathf.Abs(forceDirection.z));  
		platformDecceleration = - Mathf.Pow(topPlatformSpeed, 2) / triggerDistance;
		aux = platformAcceleration;
	}

	public bool movementEnabled;
	public float xSpeed;
	public Transform[] wayPoints;	

	bool continueRight = true;
	bool continueLeft = false;

	bool maintainInPlatform;

	GameObject objectOnSurface;
	void OnCollisionEnter2D (Collision2D other) {

		maintainInPlatform = true;
		objectOnSurface = other.gameObject;
	}
	
	void OnCollisionExit2D (Collision2D other) {
		
		maintainInPlatform = false;
	}
	

	public float platformAcceleration;
	public float forceSetoff;
	public float topPlatformSpeed;
	public float waitingTime;
	
	Vector3 platformVelocity;
	float platformVelocityMagnitude;
	bool platformVelocityUnderLimit;

	Vector2 objectVelocity;
	float objectVelocityMagnitude;
	bool objectVelocityUnderLimit;

	bool arrived;
	bool goingToNextWayPoint = true;
	public bool backAndForth;
	bool hasNotEnteredBefore = true;
	public float triggerDistance;
	float platformDecceleration;
	bool deccelerationPhase;

	bool hasNotEnteredPreviously = true;
	bool timerON = false;

	int dir;
	Vector2 forceOnObject;
	Vector2 forceOnPlatform;

	public bool compensationSystem;
	void FixedUpdate () {

		forceDirection = wayPoints[currentWayPoint].position - transform.position;


		if ((Vector3.Magnitude(forceDirection) < triggerDistance) && hasNotEnteredBefore) {	
			hasNotEnteredBefore = false;
			Debug.Log("Close enough!");
			if (goingToNextWayPoint) {
				if (currentWayPoint == wayPoints.Length - 1) {
					arrived = true; 
					Debug.Log(arrived);
					movementEnabled = false;
					// deccelerationPhase = true;
					if (backAndForth) { goingToNextWayPoint = false; timerON = true; }
				} else {
					arrived = false;
					currentWayPoint++;
				}
			} else {
				if (currentWayPoint == 0) {
					Debug.Log ("WHERE I STARTED!");
					arrived = true;  
					goingToNextWayPoint = true;
					movementEnabled = false;
					timerON = true; currentWayPoint++;
				} else {
					Debug.Log ("BACK!"); hasNotEnteredBefore = true;
					arrived = false;
					currentWayPoint--;
				}
			}
		} // hasNotEnteredBefore = true;

		if (timerON) { timer(waitingTime); }

//		if (deccelerationPhase && platformVelocityMagnitude > 0) {
//
//			platformAcceleration = platformDecceleration;
//		} else {
//			deccelerationPhase = false;
//			platformAcceleration = aux;
//		}

		forceOnPlatform = forceDirection * platformAcceleration;

		if (movementEnabled) {
			platformVelocity = transform.parent.rigidbody.velocity;
			platformVelocityMagnitude = Vector3.Magnitude(platformVelocity);
			platformVelocityUnderLimit = platformVelocityMagnitude < topPlatformSpeed;

			if (platformVelocityUnderLimit) {
				Debug.Log ("Force ON" + " current wp: " + currentWayPoint); 
				transform.parent.rigidbody.AddForce(forceOnPlatform, ForceMode.Acceleration);;
			}
		}

		if (!movementEnabled && hasNotEnteredPreviously) {
			Debug.Log ("STOP"); hasNotEnteredPreviously = false;
			transform.parent.rigidbody.velocity = Vector3.zero;
			Debug.Log (platformVelocity = transform.parent.rigidbody.velocity);
			objectOnSurface.rigidbody2D.velocity = Vector2.zero;
		}

		if (maintainInPlatform && (Input.GetKey (KeyCode.DownArrow))) {
			objectVelocity = objectOnSurface.rigidbody2D.velocity;
			objectVelocityMagnitude = Mathf.Sqrt(Vector2.SqrMagnitude (objectVelocity));
			objectVelocityUnderLimit = objectVelocityMagnitude < topPlatformSpeed;

		//	forceSetoff = * 80;

			if (movementEnabled && objectVelocityUnderLimit) {



				dir = !goingToNextWayPoint ? -1 : 1;
				Debug.Log (dir);
				forceOnObject = new Vector2(dir * forceSetoff * platformAcceleration, forceDirection.y);
				objectOnSurface.rigidbody2D.AddForceAtPosition(forceOnObject, objectOnSurface.transform.position);
			}
		}
		// Debug.Log (objectVelocityMagnitude + " - " + platformVelocityMagnitude + " / " + topPlatformSpeed);
		Debug.Log (forceOnObject);
	}
 
	float elapsedTime = 0;

	void timer (float waitingTime) {
		Debug.Log ("Timer ON");
		if (elapsedTime < waitingTime) {
				Debug.Log ("Timer ON");
				elapsedTime += Time.deltaTime;
		} else {
			elapsedTime = 0;
			Debug.Log ("Green!");
			movementEnabled = true; hasNotEnteredBefore = true; hasNotEnteredPreviously = true; timerON = false;
		}
	}
}
