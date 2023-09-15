using UnityEngine;
using System.Collections;

public class FXProxy : MonoBehaviour {

    SpriteRenderer spriteRenderer;

	void Start () {

        spriteRenderer = GetComponent<SpriteRenderer>();
	
	}
	
	void Update () {

        spriteRenderer.color = FXSeq_PrionLevel.group1;
	}
}
