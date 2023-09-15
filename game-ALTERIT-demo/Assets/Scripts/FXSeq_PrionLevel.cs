using UnityEngine;
using System.Collections;

public class FXSeq_PrionLevel : MonoBehaviour {

    public Color group11, group12;
    public static Color group1;
    FXController fxController = new FXController();
    public float freq;

	void Start () {
	
	}
	
	void Update () {

        group1 = fxController.VFXColorExhanger(group11, group12, freq);
	}
}
