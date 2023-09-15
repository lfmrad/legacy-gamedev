using UnityEngine;
using System.Collections;

public class Properties : Nature {

	public Charge charge;
	public bool chargeColor;

	void Start() {
		
		if (chargeColor) {
			renderer.material.color = charge == Charge.alpha ? alphaChargeColor : betaChargeColor;
		}
	}
}
