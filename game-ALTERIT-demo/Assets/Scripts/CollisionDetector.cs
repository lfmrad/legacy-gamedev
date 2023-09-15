using UnityEngine;
using System.Collections;

public class CollisionDetector : GC {
	
	void OnTriggerEnter2D (Collider2D other) {

        if (other.tag == "Wall") {

            GC.gameStatus = GC.gameState.failed;
        } 
  	}
}
