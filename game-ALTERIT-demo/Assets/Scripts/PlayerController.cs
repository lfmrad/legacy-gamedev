using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public enum dir { forward, backward, right, left, oblique_1q, oblique_2q, oblique_3q, oblique_4q };
    public static dir currentDir, previousDir;

    // Level Restart Parameters
    public dir levelDefaultDir;
    Vector3 levelDefaultPos;
    public static bool playerControllerHasRestarted = true;

    void Awake() {

        levelDefaultPos = transform.position;
        SetTouchParameters();
        SetLevelDefaults();
    }

    void Update() {

        if (GC.gameStatus == GC.gameState.started) {

            GetDirPc();
            GetDirMobile();
            
            if (!heldByPlayer) {
                AlterIt();
            }

            playerControllerHasRestarted = false;

            CheckIfDirHasChanged();

        } else if (GC.gameStatus == GC.gameState.restart) {

            SetLevelDefaults();
        }
    }

    void SetLevelDefaults() {

        previousDir = currentDir = levelDefaultDir;
        dirHasChanged = false;
        transform.position = levelDefaultPos;

        RestartTouchParameters();

        playerControllerHasRestarted = true;
    }

    // ----------- OnPC Testing Control

    bool heldByPlayer = false;

    dir GetDirPc() {

        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");

        // cross movement
        if ((vAxis == 1 & hAxis == 0) || Input.GetKeyDown(KeyCode.Keypad8)) {
            currentDir = dir.forward;
        } else if ((vAxis == -1 & hAxis == 0) || Input.GetKeyDown(KeyCode.Keypad2)) {
            currentDir = dir.backward;
        } else if ((hAxis == 1 & vAxis == 0) || Input.GetKeyDown(KeyCode.Keypad6)) {
            currentDir = dir.right;
        } else if ((hAxis == -1 & vAxis == 0) || Input.GetKeyDown(KeyCode.Keypad4)) {
            currentDir = dir.left;
        }

        // oblique movement
        if ((vAxis == 1 & hAxis == 1) || Input.GetKeyDown(KeyCode.Keypad9)) {
            currentDir = dir.oblique_1q;
        } else if ((vAxis == 1 & hAxis == -1) || Input.GetKeyDown(KeyCode.Keypad7)) {
            currentDir = dir.oblique_2q;
        } else if ((vAxis == -1 & hAxis == 1) || Input.GetKeyDown(KeyCode.Keypad3)) {
            currentDir = dir.oblique_4q;
        } else if ((vAxis == -1 & hAxis == -1) || Input.GetKeyDown(KeyCode.Keypad1)) {
            currentDir = dir.oblique_3q;
        }

        if (Input.GetKey(KeyCode.Keypad5)) {

            heldByPlayer = true;

        } else {

            heldByPlayer = false;
        }

        return currentDir;
    }

    // ----------- TouchSystem

    public static bool obliqueMode = false;
    bool modeFingerReleased = true, movFingerReleased = true, movTouchIdStored = false;
    int modeTouchId = 0, movTouchId = 0;

    public void GetDirMobile() {

        foreach (Touch touch in Input.touches) {

            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved) {

                if (touch.position.x < Screen.width * 0.4f && modeFingerReleased) {

                    obliqueMode = true;
                    modeFingerReleased = false;
                    modeTouchId = touch.fingerId;

                } else if (touch.position.x > Screen.width / 2 && movFingerReleased) {

                    // Prevents that an additional finger on the screen contributes to trigger the ThresholdMagnitude
                    if (!movTouchIdStored) {
                        movTouchId = touch.fingerId;
                        movTouchIdStored = true;
                    }

                    if (movTouchId == touch.fingerId) {

                        IncreaseDirVector(touch.position);
                    }

                    if (Vector2.SqrMagnitude(dirVector) > squaredTM) {

                        // Forces to lift the finger before moving again
                        movFingerReleased = false;

                        SetTouchDir();
                        RestartTouchDirVector();
                    }
                }

            } else if (touch.phase == TouchPhase.Ended && touch.fingerId == modeTouchId && !modeFingerReleased) {
       
                obliqueMode = false;
                modeFingerReleased = true;

            } else if (touch.phase == TouchPhase.Ended && touch.fingerId == movTouchId) {

                movFingerReleased = true;
                movTouchIdStored = false;
                RestartTouchDirVector();
            }
        }
    }

    float crossAngleHalf, obliqueAngleHalf;

    public void SetTouchDir() {

        float slopeDeg = Mathf.Atan2(dirVector.x, dirVector.y) * 180f / Mathf.PI;

        if (obliqueMode) {

            obliqueAngleHalf = 90.0f / 2.0f;

            if ((slopeDeg >= 45.0f - obliqueAngleHalf) & (slopeDeg <= 45.0f + obliqueAngleHalf)) {
                currentDir = dir.oblique_1q;
            } else if ((slopeDeg <= obliqueAngleHalf - 45.0f) & (slopeDeg >= -45.0f - obliqueAngleHalf)) {
                currentDir = dir.oblique_2q;
            } else if ((slopeDeg >= 135.0f - obliqueAngleHalf) & (slopeDeg <= 135.0f + obliqueAngleHalf)) {
                currentDir = dir.oblique_4q;
            } else if ((slopeDeg <= obliqueAngleHalf - 135.0f) & (slopeDeg >= -135.0f - obliqueAngleHalf)) {
                currentDir = dir.oblique_3q;
            }

        } else {

            crossAngleHalf = 90.0f / 2.0f;

            if ((slopeDeg > -crossAngleHalf) & (slopeDeg < crossAngleHalf)) {
                currentDir = dir.forward;
            } else if ((slopeDeg <= crossAngleHalf - 180.0f) || (slopeDeg >= 180.0f - crossAngleHalf)) {
                currentDir = dir.backward;
            } else if ((slopeDeg >= 90.0f - crossAngleHalf) & (slopeDeg <= 90.0 + crossAngleHalf)) {
                currentDir = dir.right;
            } else if ((slopeDeg <= crossAngleHalf - 90) & (slopeDeg >= -90.0f - crossAngleHalf)) {
                currentDir = dir.left;
            }
        }
    }

    void RestartTouchDirVector() {

            dirVector = Vector2.zero;
            previousPos = Vector2.zero;
            realTouchDeltaPos = Vector2.zero;   
    }

    void RestartTouchParameters() {

        obliqueMode = false;
        modeFingerReleased = true;
        movFingerReleased = true;
        movTouchIdStored = false;
        RestartTouchDirVector();
    }

    Vector2 dirVector = Vector2.zero, realTouchDeltaPos, previousPos = Vector2.zero;

    void IncreaseDirVector(Vector2 currentPos) {

        if ((currentPos != previousPos) & (previousPos != Vector2.zero)) {

            realTouchDeltaPos = new Vector2(
                (currentPos.x - previousPos.x),
                (currentPos.y - previousPos.y)
            );
        }
        previousPos = currentPos;
        dirVector += realTouchDeltaPos;
    }

    Vector2 designScreenSize = new Vector2(896.0f, 538.0f), currentScreenSize, screenAdjRatio;
    float designScrDPI = 224f, currentScrDPI, screenDPIRatio = 1.0f;
    float touchThresholdMagnitude, touchTMFactor = 2.66f, squaredTM, touchDMTinCubes;
    float cubesWidth = 16.0f;

    void SetTouchParameters() {

        currentScreenSize = new Vector2(Screen.width, Screen.height);
        currentScrDPI = Screen.dpi;

        if (currentScrDPI != 0.0f) {
            screenDPIRatio = (designScreenSize.x / designScrDPI) / (currentScreenSize.x / currentScrDPI);
            Debug.Log("DMT has been adjusted because of DPI");
        }
        screenAdjRatio = new Vector2(designScreenSize.x / currentScreenSize.x, designScreenSize.y / currentScreenSize.y);

        touchThresholdMagnitude = screenDPIRatio * screenAdjRatio.x * touchTMFactor * 15.0f;

        // Debug.Log("screenAdjRatio.x = " + screenAdjRatio.x + " | screenDPIRatio = " + screenDPIRatio);
        // Debug.Log("TM = " + touchThresholdMagnitude);

        squaredTM = Mathf.Pow(touchThresholdMagnitude, 2);

        // Approx. DMT in Cubes
        touchDMTinCubes = cubesWidth / designScreenSize.x * touchThresholdMagnitude;
        // Debug.Log("deltaMagThreshold in Cubes = " + controlTouchDMTinCubes);
    }

    // ----------- Mover
    Vector3 deltaPos;
    float diagMovAdj = Mathf.Sin(Mathf.PI / 4f);
    public float CPS;

    void AlterIt() {

        float controlCPF = Time.deltaTime * CPS;

        switch (currentDir) {
            case dir.forward:
                deltaPos = new Vector3(0, 1, 0);
                break;
            case dir.backward:
                deltaPos = new Vector3(0, -1, 0);
                break;
            case dir.right:
                deltaPos = new Vector3(1, 0, 0);
                break;
            case dir.left:
                deltaPos = new Vector3(-1, 0, 0);
                break;
            case dir.oblique_1q:
                deltaPos = new Vector3(diagMovAdj, diagMovAdj, 0);
                break;
            case dir.oblique_2q:
                deltaPos = new Vector3(-diagMovAdj, diagMovAdj, 0);
                break;
            case dir.oblique_4q:
                deltaPos = new Vector3(diagMovAdj, -diagMovAdj, 0);
                break;
            case dir.oblique_3q:
                deltaPos = new Vector3(-diagMovAdj, -diagMovAdj, 0);
                break;
        }

        transform.Translate(deltaPos * controlCPF);
    }

    // ----------- Others

    public static bool dirHasChanged;

    void CheckIfDirHasChanged() {

        if (currentDir != previousDir) {

            dirHasChanged = true;
            previousDir = currentDir;

        } else {

            dirHasChanged = false;
        }

    }
}