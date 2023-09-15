using UnityEngine;
using System.Collections;

public class DragAndZoom : MonoBehaviour {

public RectTransform image;
float currentScale = 0f;
public float maxScale;
public float minScale;

void Start() {

}

void Update() {
        Zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

void Zoom(float increment) {
        currentScale += increment;
        if (currentScale >= maxScale) {
            currentScale = maxScale;
        } else if (currentScale <= minScale) {
            currentScale = minScale;
        }
        transform.localScale = new Vector2(currentScale, currentScale);
    }
}
