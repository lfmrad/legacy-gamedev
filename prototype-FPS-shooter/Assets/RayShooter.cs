using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]

public class RayShooter : MonoBehaviour {
	private Camera _camera;

	public enum ShootingMode {
		SingleFire = 0,
		Automatic = 1,
	}
	public ShootingMode shootingMode = ShootingMode.SingleFire;

	void Start() {
		_camera = GetComponent<Camera>();
	
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		crosshairStyle.fontSize = 30;
		crosshairStyle.normal.textColor = Color.red;
	}

	GUIStyle crosshairStyle = new GUIStyle(); 

	void OnGUI() {
		int size = 12;
		float posX = _camera.pixelWidth/2 - size/2;
		float posY = _camera.pixelHeight/2 - size/2;
		GUI.Label(new Rect(posX, posY, size, size), "*", crosshairStyle);
	}

	void Update() {
		bool pressedKey;

		if (shootingMode == ShootingMode.SingleFire) {
			pressedKey = Input.GetMouseButtonDown(0);
		} else {
			pressedKey = Input.GetMouseButton(0);
		}

		if (pressedKey) {
			Vector3 point = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
			Ray ray = _camera.ScreenPointToRay(point);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				GameObject hitObject = hit.transform.gameObject;
				ReactiveTarget target = hitObject.GetComponent<ReactiveTarget>();
				if (target != null) {
					Debug.Log ("Target hit");
					target.ReactToHit();
				} else {
					Debug.Log ("Hit " + hit.point + ", Distance " + hit.distance);
					StartCoroutine(SphereIndicator(hit.point));
				}
			}
		}
	}

	private IEnumerator SphereIndicator(Vector3 pos) {
		GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		sphere.transform.position = pos;

		yield return new WaitForSeconds(1);

		Destroy(sphere);
	}
}
