using UnityEngine;
using System.Collections;

public class Nature : MonoBehaviour {

	public static float omicron;

	public enum Charge {alpha, beta};
	public enum FieldDirection {upper, lower};
	public enum Level {testLevel, level1}

	public static Color alphaChargeColor = Color.black;
	public static Color betaChargeColor = Color.yellow;	
	
//	public static Properties[] entitiesProperties; 


	private void Start() {

		omicron = 40f;
		Debug.Log ("Omicron from Nature is " + omicron);
		// entitiesProperties = FindObjectsOfType<Properties>();
	}

	// public Transform[] spawns;

//	public void ChangeLevel (int chosenLevel, GameObject player) {
//
//		Debug.Log ("CL");
//
//		player.transform.position = spawns[chosenLevel].position;
//		player.rigidbody2D.velocity = new Vector2 (0, 0); 
//		player.rigidbody2D.angularVelocity = 0;
//	}
}
