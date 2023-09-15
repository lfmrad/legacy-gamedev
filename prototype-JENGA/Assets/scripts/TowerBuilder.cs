using UnityEngine;
using System.Collections;

public class TowerBuilder : MonoBehaviour {

	int blocksNumber;
	public GameObject jengaBlock;

	// Quaternion rotation;
	Vector3 pos;
	public int numberOfFloors;

	bool alongX = true;

	// float jengaBlockScaleY = jengaBlock.transform.localScale.y;
	float currentHeight = 0;
	// 54 / 3 = 15 (regular jenga)

	void Start () {

		blocksNumber = numberOfFloors * 3;

		for (int i = 0; i < numberOfFloors; i++) {

			currentHeight += jengaBlock.transform.localScale.y;

			for (int j = 0; j < 3; j++) {
		
				if (alongX) {
					pos = new Vector3(j, currentHeight, 0);
					Instantiate(jengaBlock, pos, Quaternion.identity);
				} else {
					pos = new Vector3(1.0f, currentHeight, j - 1.0f);
					Instantiate(jengaBlock, pos, Quaternion.Euler(0, 90, 0));
				}
			}
			alongX = !alongX;
			// if (i > 1 && (i % 2 == 0)) {
			// 	alongX = !alongX;
			// }
			// i > 1 && (i % 2 == 0) ? alongX = !alongX;
		}
	}
}
