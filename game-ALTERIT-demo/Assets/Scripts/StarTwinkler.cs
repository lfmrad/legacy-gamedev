using UnityEngine;
using System.Collections;

public class StarTwinkler : MonoBehaviour {

    SpriteRenderer spriteRenderer;
    FXController fxController = new FXController();
    public Color shiningColor, fadedColor;
    public float twinklingFreq;
    public bool twinklingStar = false;
	
	void Start () {

        spriteRenderer = GetComponent<SpriteRenderer>();
        twinklingFreq = Random.Range(0.1f, 0.3f);
	}
	
	void Update () {

        if (twinklingStar) {

            spriteRenderer.color = fxController.VFXColorExhanger(shiningColor, fadedColor, twinklingFreq);
        }   
	}
}
