using UnityEngine;
using System.Collections;

public class PlayerAnimator : MonoBehaviour {

    public static bool playerAnimatorHasRestarted = true;

    void Awake() {

        GetTricubeBonesDefaults();
        GetCtrlFbGridLevelDefaults();
        SetLevelDefaults();
    }

    void Update() {

        if (GC.gameStatus == GC.gameState.started) {

            ReorganizeTricube();
            ReorganizeObliqueFeedback();
            playerAnimatorHasRestarted = false;

        } else if (GC.gameStatus == GC.gameState.restart) {

            // Because Animator needs TricubeBones to become childs of Player again
            // after LevelEnder animations before restoring values or won't do it properly.
            if (LevelEnder.levelEnderHasRestarted) {

                SetLevelDefaults();
                playerAnimatorHasRestarted = true;
            }
   
        }
    }

    void SetLevelDefaults() {

        SetTricubeReorganizerToLevelDefaults();
        SetGetCtrlFbGridToLevelDefaults();
    }

    // ----------- Alterit Bones Reorganizer
    
    Vector2 levelDefaultHeadPos, levelDefaultNexusPos, levelDefaultTailPos;   

    void GetTricubeBonesDefaults() {

        levelDefaultHeadPos = tricubeHead.localPosition;
        levelDefaultNexusPos = tricubeNexus.localPosition;
        levelDefaultTailPos = tricubeTail.localPosition;
    }

    public Transform tricubeHead, tricubeNexus, tricubeTail;

    void SetTricubeReorganizerToLevelDefaults() {

        // Nexus & Quaternions are here to fully restore to default after a gameComplete (because of animations in LeverEnder)
        tricubeHead.localPosition = levelDefaultHeadPos;
        tricubeHead.rotation = Quaternion.identity;
        tricubeNexus.localPosition = levelDefaultNexusPos;
        // Debug.Log("localPos = " + tricubeNexus.localPosition + " defaultPos = " + levelDefaultNexusPos);
        tricubeNexus.rotation = Quaternion.identity;
        tricubeTail.localPosition = levelDefaultTailPos;
        tricubeTail.rotation = Quaternion.identity;

        gameJustStartedForReorganizer = true;
    }

    public float reorganizationFactor = 10.0f;
    Vector2 headCurrentPos, tailCurrentPos;
    Vector2 headTargetPos, tailTargetPos;
    float reorgStep;
    bool gameJustStartedForReorganizer;
    public float tricubeDiagXY, tricubeCrossXY;

    void ReorganizeTricube() {

        if (PlayerController.dirHasChanged || gameJustStartedForReorganizer) {

            reorgStep = 0.0f;
            headCurrentPos = tricubeHead.localPosition;
            tailCurrentPos = tricubeTail.localPosition;

            // This allows to reset the head/tail initial pos. 
            // so that Reorganizer doesn't continues from a past reorg.
            // in case you died during one.
            gameJustStartedForReorganizer = false;
        }

        if (reorgStep < 1.0f) { 

            reorgStep += Time.deltaTime * reorganizationFactor;
            switch (PlayerController.currentDir) {
                case PlayerController.dir.forward:
                    headTargetPos = new Vector2(0, tricubeCrossXY);
                    tailTargetPos = new Vector2(0, -tricubeCrossXY);
                    break;
                case PlayerController.dir.backward:
                    headTargetPos = new Vector2(0, -tricubeCrossXY);
                    tailTargetPos = new Vector2(0, tricubeCrossXY);
                    break;
                case PlayerController.dir.right:
                    headTargetPos = new Vector2(tricubeCrossXY, 0);
                    tailTargetPos = new Vector2(-tricubeCrossXY, 0);
                    break;
                case PlayerController.dir.left:
                    headTargetPos = new Vector2(-tricubeCrossXY, 0);
                    tailTargetPos = new Vector2(tricubeCrossXY, 0);
                    break;
                case PlayerController.dir.oblique_1q:
                    headTargetPos = new Vector2(tricubeDiagXY, tricubeDiagXY);
                    tailTargetPos = new Vector2(-tricubeDiagXY, -tricubeDiagXY);
                    break;
                case PlayerController.dir.oblique_2q:
                    headTargetPos = new Vector2(-tricubeDiagXY, tricubeDiagXY);
                    tailTargetPos = new Vector2(tricubeDiagXY, -tricubeDiagXY);
                    break;
                case PlayerController.dir.oblique_4q:
                    headTargetPos = new Vector2(tricubeDiagXY, -tricubeDiagXY);
                    tailTargetPos = new Vector2(-tricubeDiagXY, tricubeDiagXY);
                    break;
                case PlayerController.dir.oblique_3q:
                    headTargetPos = new Vector2(-tricubeDiagXY, -tricubeDiagXY);
                    tailTargetPos = new Vector2(tricubeDiagXY, tricubeDiagXY);
                    break;
            }
            tricubeHead.localPosition = Vector2.Lerp(headCurrentPos, headTargetPos, reorgStep);
            tricubeTail.localPosition = Vector2.Lerp(tailCurrentPos, tailTargetPos, reorgStep);
        }
    }

    // ----------- Ctrl Feedback Grid

    public Transform ctrlFb1, ctrlFb2, ctrlFb3, ctrlFb4;
        
    bool previousOblique;

    Vector2 levelDefaultCtrlFb1Pos, levelDefaultCtrlFb2Pos, levelDefaultCtrlFb3Pos, levelDefaultCtrlFb4Pos;
    
    void GetCtrlFbGridLevelDefaults() {

        levelDefaultCtrlFb1Pos = ctrlFb1.localPosition;
        levelDefaultCtrlFb2Pos = ctrlFb2.localPosition;
        levelDefaultCtrlFb3Pos = ctrlFb3.localPosition;
        levelDefaultCtrlFb4Pos = ctrlFb4.localPosition;
    }

    void SetGetCtrlFbGridToLevelDefaults() {

        ctrlFb1.localPosition = levelDefaultCtrlFb1Pos;
        ctrlFb2.localPosition = levelDefaultCtrlFb2Pos;
        ctrlFb3.localPosition = levelDefaultCtrlFb3Pos;
        ctrlFb4.localPosition = levelDefaultCtrlFb4Pos;
        // So that it doesn't reorganize anything before the first mode change or direction change.
        gameJustStartedForCtrlFeedbackGrid = true;
    }

    public float ctrlFeedbackGridSpeed;
    Vector2 ctrlFb1CurrentPos, ctrlFb2CurrentPos, ctrlFb3CurrentPos, ctrlFb4CurrentPos;
    Vector2 ctrlFb1TargetPos, ctrlFb2TargetPos, ctrlFb3TargetPos, ctrlFb4TargetPos;
    float ctrlGridStep;
    bool gameJustStartedForCtrlFeedbackGrid;
    
    void ReorganizeObliqueFeedback() {

        if (PlayerController.obliqueMode != previousOblique || 
            PlayerController.dirHasChanged ||
            gameJustStartedForCtrlFeedbackGrid
            ) {

            ctrlGridStep = 0.0f;
            ctrlFb1CurrentPos = ctrlFb1.localPosition;
            ctrlFb2CurrentPos = ctrlFb2.localPosition;
            ctrlFb3CurrentPos = ctrlFb3.localPosition;
            ctrlFb4CurrentPos = ctrlFb4.localPosition;

            previousOblique = PlayerController.obliqueMode;

            gameJustStartedForCtrlFeedbackGrid = false;
        }

        if (ctrlGridStep <= 1.0f) {

            ctrlGridStep += Time.deltaTime * ctrlFeedbackGridSpeed;

            if (PlayerController.obliqueMode) {

                ReorganizeWhenObliqueIsEnabled();

            } else {

                ReorganizeWhenObliqueIsDisabled();
            }

            ctrlFb1.localPosition = Vector2.Lerp(ctrlFb1CurrentPos, ctrlFb1TargetPos, ctrlGridStep);
            ctrlFb2.localPosition = Vector2.Lerp(ctrlFb2CurrentPos, ctrlFb2TargetPos, ctrlGridStep);
            ctrlFb3.localPosition = Vector2.Lerp(ctrlFb3CurrentPos, ctrlFb3TargetPos, ctrlGridStep);
            ctrlFb4.localPosition = Vector2.Lerp(ctrlFb4CurrentPos, ctrlFb4TargetPos, ctrlGridStep);

        } 
    }

    public float gridDiagXY, gridCrossXY;

    void ReorganizeWhenObliqueIsEnabled() {

        if (PlayerController.currentDir == PlayerController.dir.forward || PlayerController.currentDir == PlayerController.dir.backward
            || PlayerController.currentDir == PlayerController.dir.right || PlayerController.currentDir == PlayerController.dir.left) {
            ctrlFb1TargetPos = new Vector2(-gridCrossXY, gridCrossXY);
            ctrlFb2TargetPos = new Vector2(gridCrossXY, gridCrossXY);
            ctrlFb3TargetPos = new Vector2(gridCrossXY, -gridCrossXY);
            ctrlFb4TargetPos = new Vector2(-gridCrossXY, -gridCrossXY);

        } else if (PlayerController.currentDir == PlayerController.dir.oblique_1q || PlayerController.currentDir == PlayerController.dir.oblique_3q) {
            ctrlFb1TargetPos = new Vector2(-gridCrossXY, gridCrossXY);
            ctrlFb2TargetPos = new Vector2(0, 0);
            ctrlFb3TargetPos = new Vector2(gridCrossXY, -gridCrossXY);
            ctrlFb4TargetPos = new Vector2(0, 0);

        } else if (PlayerController.currentDir == PlayerController.dir.oblique_2q || PlayerController.currentDir == PlayerController.dir.oblique_4q) {
            ctrlFb1TargetPos = new Vector2(0, 0);
            ctrlFb2TargetPos = new Vector2(gridCrossXY, gridCrossXY);
            ctrlFb3TargetPos = new Vector2(0, 0);
            ctrlFb4TargetPos = new Vector2(-gridCrossXY, -gridCrossXY);
        }
    }

    void ReorganizeWhenObliqueIsDisabled() {

        if (PlayerController.currentDir == PlayerController.dir.forward || PlayerController.currentDir == PlayerController.dir.backward) {
            ctrlFb1TargetPos = new Vector2(-gridCrossXY, 0);
            ctrlFb2TargetPos = new Vector2(0, 0);
            ctrlFb3TargetPos = new Vector2(gridCrossXY, 0);
            ctrlFb4TargetPos = new Vector2(0, 0);

        } else if (PlayerController.currentDir == PlayerController.dir.right || PlayerController.currentDir == PlayerController.dir.left) {
            ctrlFb1TargetPos = new Vector2(0, 0);
            ctrlFb2TargetPos = new Vector2(0, gridCrossXY);
            ctrlFb3TargetPos = new Vector2(0, 0);
            ctrlFb4TargetPos = new Vector2(0, -gridCrossXY);

        } else if (PlayerController.currentDir == PlayerController.dir.oblique_1q || PlayerController.currentDir == PlayerController.dir.oblique_3q
        || PlayerController.currentDir == PlayerController.dir.oblique_2q || PlayerController.currentDir == PlayerController.dir.oblique_4q) {
            ctrlFb1TargetPos = new Vector2(-gridCrossXY, 0);
            ctrlFb2TargetPos = new Vector2(0, gridCrossXY);
            ctrlFb3TargetPos = new Vector2(gridCrossXY, 0);
            ctrlFb4TargetPos = new Vector2(0, -gridCrossXY);
        }
    }

    // ----------- Not used yet

    int[] chkNo = new int[2];
    bool chkState;

    void ChangeChunkState(int chunkNumber, bool state) {

        switch (chunkNumber) {
            case 1:
                ctrlFb1.gameObject.SetActive(state);
                break;
            case 2:
                ctrlFb2.gameObject.SetActive(state);
                break;
            case 3:
                ctrlFb2.gameObject.SetActive(state);
                break;
            case 4:
                ctrlFb2.gameObject.SetActive(state);
                break;
            default:
                break;
        }
    }
}
