using UnityEngine;
using System.Collections;

public class GenericRotator : MonoBehaviour {

    FXController fxController = new FXController();

    public bool binaryMusicRotation;
    public float binaryOscFactor;
    public float deltaAnglePerPeak;
    bool alreadyRotated = false;

    public bool genericContinuousRotation;
    public float continuousRotFactor;

    public bool randomRotation;
    public float chanceOfRotation;

	void Update() {

        if (randomRotation && Random.Range(0.0f, 1.0f) < chanceOfRotation) {

            Rotate();

        } else if (!randomRotation) {

            Rotate();
        }
	}

    void Rotate() {

        if (binaryMusicRotation) {

            BinaryMusicRotation();

        } else if (genericContinuousRotation) {

            transform.Rotate(new Vector3(0, 0, deltaAnglePerPeak * Time.deltaTime * continuousRotFactor));
        }
    }

    void BinaryMusicRotation() {

        if (fxController.BinaryMusicOSC(binaryOscFactor) && !alreadyRotated) {

            alreadyRotated = true;

            transform.Rotate(new Vector3(0, 0, deltaAnglePerPeak));

        } else if (!fxController.BinaryMusicOSC(binaryOscFactor)) {

            alreadyRotated = false;
        }
    }
}
