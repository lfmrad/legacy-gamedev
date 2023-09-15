using UnityEngine;
using System.Collections;

public class StageRotator : MonoBehaviour {

    Quaternion initialRot;
    public float rotationFactor;
    public float degrees;


    void Awake() {

        initialRot = transform.rotation;
    }

    bool doRotate = false;

	void OnTriggerEnter2D(Collider2D other) {

        doRotate = true;
	}

    bool alreadyRotated = false;

    void Update() {

        if (doRotate && !alreadyRotated) {

            Rotate();
        }
    }

    float step = 0f;

    void Rotate() {

        step += Time.deltaTime * rotationFactor;

        if (step <= 1.5) {

            transform.rotation = Quaternion.Lerp(initialRot, Quaternion.Euler(new Vector3(0, 0, degrees)), step);

            Debug.Log(step);
        } else {

            step = 0f;
            doRotate = false; alreadyRotated = true;
        }
    }
}
