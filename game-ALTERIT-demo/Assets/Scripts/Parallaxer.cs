using UnityEngine;
using System.Collections;

public class Parallaxer : MonoBehaviour {

    public Transform[] backgroundsToMove;
    public float[] playerRatio;
    public Transform player;
	
	void Update () {

        for (int i = 0; i < backgroundsToMove.Length; i++) {

            backgroundsToMove[i].position = player.position * playerRatio[i];
        }
	}
}
