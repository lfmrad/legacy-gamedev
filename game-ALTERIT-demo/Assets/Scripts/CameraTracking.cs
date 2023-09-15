using UnityEngine;
using System.Collections;

public class CameraTracking : MonoBehaviour {

    // float designAspect = 1.60f;
    float levelDefaultCameraSize;

    void Awake() {

        levelDefaultCameraSize = GetComponent<Camera>().orthographicSize;
    }

    void Update() {

        AdjustCamera();

        if (GC.gameStatus == GC.gameState.started) {

            

        } else if (GC.gameStatus == GC.gameState.waiting) {
   
        }
    }

    Vector2 cameraCurrentPos, cameraTargetPos;
    public float cameraAdjustmentSpeed = 1.0f;
    public float cameraOffset = 2.0f;
    float camAdjStep = 0.0f;

    void AdjustCamera() {

        if (PlayerController.dirHasChanged == true || GC.gameStatus == GC.gameState.waiting) {
            cameraCurrentPos = transform.localPosition;
            camAdjStep = 0.0f;
        } 

        camAdjStep += Time.deltaTime * cameraAdjustmentSpeed;

        switch (PlayerController.currentDir) {

            case PlayerController.dir.forward:
                cameraTargetPos = new Vector2(0, 1);
                break;
            case PlayerController.dir.backward:
                cameraTargetPos = new Vector2(0, -1);
                break;
            case PlayerController.dir.right:
                cameraTargetPos = new Vector2(1, 0);
                break;
            case PlayerController.dir.left:
                cameraTargetPos = new Vector2(-1, 0);
                break;
            case PlayerController.dir.oblique_1q:
                cameraTargetPos = new Vector2(1, 1);
                break;
            case PlayerController.dir.oblique_2q:
                cameraTargetPos = new Vector2(-1, 1);
                break;
            case PlayerController.dir.oblique_4q:
                cameraTargetPos = new Vector2(1, -1);
                break;
            case PlayerController.dir.oblique_3q:
                cameraTargetPos = new Vector2(-1, -1);
                break;
        }
        Vector2 newPos = Vector2.Lerp(cameraCurrentPos, cameraTargetPos * cameraOffset, camAdjStep);
        transform.localPosition = new Vector3(newPos.x, newPos.y, transform.localPosition.z);

    }
}

    /* DECLARATIONS
    public static bool cameraIsAffectedByMusic = true;
    public static float cameraSizeMusicBounce = 0.35f;

    public static bool zoomInComplete = false;
    public static bool gameZoomRestored = false;
    public static float cameraZoomSpeed = 4.0f;
    public static float cameraZoomedInSize = 0.1f;
     * */

    /* FUNCTIONS
     * void CenterCameraWhileZoomIn() {

        SetMovementParameters();
        camAdjStep += Time.deltaTime * 10.0f; // needs to be fast enough to finish before DoZoomIn
        cameraTargetPos = Vector3.zero;
        transform.localPosition = Vector3.Lerp(cameraCurrentPos, cameraTargetPos + new Vector3(0, 0, -1), camAdjStep);
    } 

    float step = 0f;

    void DoZoomIn() {

        if (step <= 1f) {
            step += GC.cameraZoomSpeed * Time.deltaTime;
            camera.orthographicSize = Mathf.Lerp(cameraInitialSize, GC.cameraZoomedInSize, step);
            gameZoomRestored = false;
        } else {
            step = 0f;
            zoomInComplete = true;
        }
    }

    void RestoreGameZoom() {

        if (step <= 1f) {
            step += GC.cameraZoomSpeed * Time.deltaTime;
            camera.orthographicSize = Mathf.Lerp(GC.cameraZoomedInSize, cameraInitialSize, step);
            zoomInComplete = false;
        } else {
            step = 0f;
            gameZoomRestored = true;
        }
    } */
