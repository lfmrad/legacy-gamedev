using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour 
{

	public GameObject explosion;
	public GameObject playerExplosion;
	public int scoreValue;
	private GameController gameController;

	void Start ()
	{
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) 
		{
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		if (gameControllerObject == null) 
		{
			Debug.Log ("Cannot find 'GameController' script");
		}

	}
		
		void OnTriggerEnter(Collider other)
	{
		// To detect why the asteroid got destroyed: Debug.Log (other.name);
		if (other.tag != "Boundary") 
		{
			Instantiate(explosion,transform.position,transform.rotation);
			if(other.tag == "Player")
			{
				Instantiate(playerExplosion,other.transform.position,other.transform.rotation);
				gameController.GameOver ();
			}
			// The order doesn't matter because they get destroyed at the end of the frame.
			Destroy(other.gameObject);
			Destroy(gameObject);
			gameController.AddScore (scoreValue);
		}
	}
}
