using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldGenerator : Nature {
	
	public FieldDirection fieldDirection;
	public float fieldHeight;
	public float fieldFactor;
	public float fieldWidthFactor;
	public bool fieldEnabled;

	Charge charge;

	Vector2 objectInFieldXY; Vector2 fieldXY;

	bool isAttachedToAMovingPlatform;

	void Start () {
		if (fieldEnabled) {	
			transform.localScale = new Vector2 (transform.localScale.x * fieldWidthFactor, transform.localScale.y * fieldHeight);
			
			if (fieldDirection == FieldDirection.lower) {
				transform.Rotate(Vector3.forward * 180f);
				fieldHeight *= -1;
			}
	
			transform.localPosition = new Vector2 (transform.localPosition.x, transform.localPosition.y + fieldHeight/2);

		 	charge = transform.parent.GetComponent<Properties>().charge;
	
			fieldXY = transform.position;

			// objectsInField = new List<GameObject>();

//			if (transform.parent.transform.parent.tag == "MovingPlatformWithField") {
//				isAttachedToAMovingPlatform = true;
//			}
		}
	}

	List<GameObject> objectsInField = new List<GameObject>();
	GameObject objectInField;	

	void OnTriggerEnter2D (Collider2D other) {

		objectInField = other.gameObject;

//		if (isAttachedToAMovingPlatform) {
//			objectInField.transform.parent = transform.parent.transform.parent;		
//		}

		if (fieldEnabled) {
			if(!objectsInField.Contains (objectInField)) { objectsInField.Add (objectInField); }
			Debug.Log("Something In Field");
		}
	}


	void OnTriggerExit2D (Collider2D other) {

		objectInField = other.gameObject;

//		if (isAttachedToAMovingPlatform) {
//			objectInField.transform.parent = null;	
//		}

		if (fieldEnabled) {	
			objectsInField.Remove (objectInField);
		}
		Debug.Log("OUT");
	}
	
	public bool rotate; public float rotation;
	
	void Update() {
		if (rotate) {
			transform.parent.RotateAround(Vector3.zero, Vector3.forward, rotation * Time.deltaTime);
			// Debug.Log(transform.parent.eulerAngles[2]);
		}
	}

	Vector2 forceMagnitude;
	float distanceFactor;
	public bool distanceAffectsForce;
	int dir;

	void FixedUpdate () {

		if (fieldEnabled) {		
			for (int i = 0; i < objectsInField.Count; i++) {
	
				distanceFactor = distanceAffectsForce ? Mathf.Pow ((1/Vector2.Distance(objectInFieldXY,fieldXY)),2) : 1;
				dir = charge == objectsInField[i].GetComponent<Properties>().charge ? 1 : -1;
	
				forceMagnitude = (transform.up * dir * fieldFactor * distanceFactor * Nature.omicron);
				objectsInField[i].rigidbody2D.AddForce (forceMagnitude); 
				// Debug.Log (forceMagnitude);
			}
		}
	}
}