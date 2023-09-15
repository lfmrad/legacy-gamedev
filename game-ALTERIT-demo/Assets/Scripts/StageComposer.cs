using UnityEngine;
using System.Collections;

public class StageComposer : MonoBehaviour {

    //[HideInInspector]
    public bool triggered;
    public float speed;

    void Update() {

        if (GC.gameStatus == GC.gameState.restart) {

            // Debug.Log("triggered set to false");
            triggered = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {

        // Debug.Log("Tricubed entered the zone!");

        triggered = true;
    }
}
