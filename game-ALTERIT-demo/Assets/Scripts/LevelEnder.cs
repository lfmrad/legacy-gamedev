using UnityEngine;
using System.Collections;

public class LevelEnder : MonoBehaviour {

    public static bool levelEnderHasFinishedLevelComplete = false;
    public static bool levelEnderHasRestarted = false;

    public float tricubeTrifectaModule = 1.0f;
    public float ledsTrifectaModule = 1.0f;

    void Awake() {

        GetLevelDefaults();
        SetLevelDefaults();
    }
		
	void Update() {

        if (GC.gameStatus == GC.gameState.started) {

            RotateHoldAnchor();
            RotateLeds();
            MakeLedsBounceWithTheRhythmOfTheStreet();
            ChangeLedsColors();
            levelEnderHasRestarted = false;

        } else if (GC.gameStatus == GC.gameState.complete) {

            if (
                positionTricubeCheck && positionLedsCheck && 
                restoreDefaultHoldAnchorRotationCheck && restoreDefaultLedRotationsCheck
                ) 
            {
                SetTricubeBonesAsChildsOfLeds();
                RotateHoldAnchor();
                RotateLeds();
                MakeLedsBounceWithTheRhythmOfTheStreet();
                ChangeLedsColors();

                if (WaitFor(3.0f)) {

                    levelEnderHasFinishedLevelComplete = true;
                }
 
            } else {

                GetInitialTricubeBonesPositions();
                PositionTricube();
                PositionLeds();
                RestoreDefaultHoldAnchorRotation();
                RestoreDefaultLedRotations();
            }

        } else if (GC.gameStatus == GC.gameState.restart) {

            SetLevelDefaults();
            levelEnderHasRestarted = true;
        }
	}

    void OnTriggerEnter2D(Collider2D other) {

        if (other.tag == "Player") {

            GC.gameStatus = GC.gameState.complete;
        }
    }

    void GetLevelDefaults() {

        holdArchorInitialRot = holdAnchor.rotation;
        ledsInitialRot = ledAlpha.rotation;
    }

    void SetLevelDefaults() {

        levelEnderHasFinishedLevelComplete = false;

        SetTricubeTargetPos();
        SetLedsDefaultPos();
        SetTricubeBonesDefaultParent();

        holdAnchor.rotation = holdArchorInitialRot;
        ledAlpha.rotation = ledBeta.rotation = ledGamma.rotation = ledsInitialRot;

        positionTricubeCheck = positionLedsCheck = false;
        restoreDefaultHoldAnchorRotationCheck = restoreDefaultLedRotationsCheck = false;

        tricubeInitialPosSet = ledCurrentPosSet = currentRotSet = false;

        triStep = rotAnchorStep = ledStep = rotLedsStep = 0.0f;
    }

    // ----------- This timer should be on GC, but Time doesn't work well there... O.o Waiting For Fix.

    float timer = 0.0f;

    bool WaitFor(float secsToWait) {

        if (timer <= secsToWait) {

            timer += Time.deltaTime;

        } else {
            timer = 0.0f;
            return true;           
        }
        return false;
    }

    // ----------- Tricube And Bones

    void SetTricubeBonesAsChildsOfLeds() {

        tricubeHead.transform.parent = ledAlpha.transform;
        tricubeNexus.transform.parent = ledBeta.transform;
        tricubeTail.transform.parent = ledGamma.transform;
    }

    void SetTricubeBonesDefaultParent() {

        tricubeHead.transform.parent = tricube.transform;
        tricubeNexus.transform.parent = tricube.transform;
        tricubeTail.transform.parent = tricube.transform;
    }

    public Transform tricube, tricubeHead, tricubeNexus, tricubeTail;
    
    Vector2 tricubeInitialPos;

    bool tricubeInitialPosSet;

    Vector2 tricubeHeadTargetPos, tricubeNexusTargetPos, tricubeTailTargetPos;

    void SetTricubeTargetPos() {

        SetTrifectaScale(tricubeTrifectaModule);
        tricubeHeadTargetPos = new Vector2(0.0f, -tricubeTrifectaModule);
        tricubeNexusTargetPos = new Vector2(-scaledXTrif, scaledYTrif);
        tricubeTailTargetPos = new Vector2(scaledXTrif, scaledYTrif);
    }

    Vector2 tricubeHeadInitialPos, tricubeNexusInitialPos, tricubeTailInitialPos;

    void GetInitialTricubeBonesPositions() {

        if (!tricubeInitialPosSet) {

            tricubeHeadInitialPos = tricubeHead.localPosition;
            tricubeNexusInitialPos = tricubeNexus.localPosition;
            tricubeTailInitialPos = tricubeTail.localPosition;
            tricubeInitialPos = tricube.position;

            tricubeInitialPosSet = true;
        }
    }

    public float tricubeToAnchorSpeed;
    float triStep;
    bool positionTricubeCheck;

    void PositionTricube() {

        if (triStep <= 1) {

            triStep += Time.deltaTime * tricubeToAnchorSpeed;

            tricubeHead.localPosition = Vector2.Lerp(tricubeHeadInitialPos, tricubeHeadTargetPos, triStep);
            tricubeNexus.localPosition = Vector2.Lerp(tricubeNexusInitialPos, tricubeNexusTargetPos, triStep);
            tricubeTail.localPosition = Vector2.Lerp(tricubeTailInitialPos, tricubeTailTargetPos, triStep);
            tricube.position = Vector2.Lerp(tricubeInitialPos, holdAnchor.position, triStep);

        } else {

            positionTricubeCheck = true;
        }
    }

    // ----------- Anchor

    public Transform holdAnchor;
    public float anchorRotationSpeed;

    void RotateHoldAnchor() {

        holdAnchor.Rotate(new Vector3(0, 0, anchorRotationSpeed * Time.deltaTime));

    }

    public float anchorRestoreRotSpeed;
    bool currentRotSet;
    Quaternion holdArchorInitialRot, holdAnchorCurrentRot;
    float rotAnchorStep;
    bool restoreDefaultHoldAnchorRotationCheck;

    void RestoreDefaultHoldAnchorRotation() {

        if (!currentRotSet) {

            holdAnchorCurrentRot = holdAnchor.rotation;
            currentRotSet = true;
        }

        if (rotAnchorStep <= 1) {

            rotAnchorStep += Time.deltaTime * anchorRestoreRotSpeed;
            holdAnchor.rotation = Quaternion.Lerp(holdAnchorCurrentRot, holdArchorInitialRot, rotAnchorStep);

        } else {

            restoreDefaultHoldAnchorRotationCheck = true;
        }
    }

    // ----------- Leds

    FXController fxController = new FXController();

    public SpriteRenderer ledAlphaRendererMain, ledAlphaRendererSub;
    public SpriteRenderer ledBetaRendererMain, ledBetaRendererSub;
    public SpriteRenderer ledGammaRendererMain, ledGammaRendererSub;

    public Color main01, main02, sub01, sub02, main11, main12, sub11, sub12;
    public float ledColorChangeFactor;

    void ChangeLedsColors () {

        Color mainLedColor, subLedColor;

        if (GC.gameStatus == GC.gameState.started) {

            mainLedColor = fxController.VFXColorOsc(main01, main02, ledColorChangeFactor);
            subLedColor = fxController.VFXColorOsc(sub01, sub02, ledColorChangeFactor);


            ledAlphaRendererMain.color = ledBetaRendererMain.color = ledGammaRendererMain.color = mainLedColor;
            ledAlphaRendererSub.color = ledBetaRendererSub.color = ledGammaRendererSub.color = subLedColor;

        } else if (GC.gameStatus == GC.gameState.complete) {

            mainLedColor = fxController.VFXColorOsc(main11, main12, ledColorChangeFactor);
            subLedColor = fxController.VFXColorOsc(sub11, sub12, ledColorChangeFactor);

            ledAlphaRendererMain.color = ledBetaRendererMain.color = ledGammaRendererMain.color = mainLedColor;
            ledAlphaRendererSub.color = ledBetaRendererSub.color = ledGammaRendererSub.color = subLedColor;
        }
    }

    public Transform ledAlpha, ledBeta, ledGamma;
    public float ledsRotationSpeed;

    void RotateLeds() {

        ledAlpha.Rotate(new Vector3(0, 0, ledsRotationSpeed * Time.deltaTime));
        ledBeta.Rotate(new Vector3(0, 0, ledsRotationSpeed * Time.deltaTime));
        ledGamma.Rotate(new Vector3(0, 0, ledsRotationSpeed * Time.deltaTime));
    }

    public float ledsToAnchorSpeed;
    
    bool ledCurrentPosSet;
    Vector2 ledAlphaCurrentPos, ledBetaCurrentPos, ledGammaCurrentPos;
    float ledStep;
    bool positionLedsCheck;

    void PositionLeds() {

        if (ledStep <= 1) {

            ledStep += Time.deltaTime * ledsToAnchorSpeed;

            if (!ledCurrentPosSet) {

                ledAlphaCurrentPos = ledAlpha.localPosition;
                ledBetaCurrentPos = ledBeta.localPosition;
                ledGammaCurrentPos = ledGamma.localPosition;
                ledCurrentPosSet = true;
            }

            ledAlpha.localPosition = Vector2.Lerp(ledAlphaCurrentPos, tricubeHeadTargetPos, ledStep);
            ledBeta.localPosition = Vector2.Lerp(ledBetaCurrentPos, tricubeNexusTargetPos, ledStep);
            ledGamma.localPosition = Vector2.Lerp(ledGammaCurrentPos, tricubeTailTargetPos, ledStep);

        } else {

            positionLedsCheck = true;
        }
    }
    
    public float ledsRestoreRotSpeed;

    bool currentLedRotSet;
    Quaternion ledsInitialRot, ledsCurrentRot;
    float rotLedsStep;
    bool restoreDefaultLedRotationsCheck;

    void RestoreDefaultLedRotations() {

        if (!currentLedRotSet) {

            ledsCurrentRot = ledAlpha.rotation;
            currentLedRotSet = true;
        }

        if (rotLedsStep <= 1) {

            rotLedsStep += Time.deltaTime * ledsRestoreRotSpeed;
            ledAlpha.rotation = Quaternion.Lerp(ledsCurrentRot, holdArchorInitialRot, rotLedsStep);
            ledBeta.rotation = Quaternion.Lerp(ledsCurrentRot, holdArchorInitialRot, rotLedsStep);
            ledGamma.rotation = Quaternion.Lerp(ledsCurrentRot, holdArchorInitialRot, rotLedsStep);

        } else {

            restoreDefaultLedRotationsCheck = true;
        }
    }

    Vector2 ledAlphaInitialPos, ledBetaInitialPos, ledGammaInitialPos;

    void SetLedsDefaultPos() {

        SetTrifectaScale(ledsTrifectaModule);
        ledAlphaInitialPos = ledAlpha.localPosition = new Vector2(0.0f, -ledsTrifectaModule);
        ledBetaInitialPos = ledBeta.localPosition = new Vector2(-scaledXTrif, scaledYTrif);
        ledGammaInitialPos = ledGamma.localPosition = new Vector2(scaledXTrif, scaledYTrif);
    }

    public float ledsBouncingFactor, ledsBouncingPercentage, finalRotDampingDuration;
    public bool ledsBouncing;

    void MakeLedsBounceWithTheRhythmOfTheStreet() {

        if (ledsBouncing && FXController.musicSystemEnabled) {

            float oscValue = fxController.MusicOSC(ledsBouncingFactor);

            if (GC.gameStatus == GC.gameState.started) {

                ledAlpha.localPosition = ledAlphaInitialPos * (1 + ledsBouncingPercentage * oscValue);
                ledBeta.localPosition = ledBetaInitialPos * (1 + ledsBouncingPercentage * oscValue);
                ledGamma.localPosition = ledGammaInitialPos * (1 + ledsBouncingPercentage * oscValue);

                fxController.SetDamperDefaults();

            } else if (GC.gameStatus == GC.gameState.complete) {

                oscValue *= fxController.Damper(finalRotDampingDuration);

                ledAlpha.localPosition = tricubeHeadTargetPos * (1 + ledsBouncingPercentage * oscValue);
                ledBeta.localPosition = tricubeNexusTargetPos * (1 + ledsBouncingPercentage * oscValue);
                ledGamma.localPosition = tricubeTailTargetPos * (1 + ledsBouncingPercentage * oscValue);
            }   
        }
    }

    // ----------- Others

    float scaledXTrif, scaledYTrif;
    float xTrif = Mathf.Cos(Mathf.PI / 6.0f);
    float yTrif = Mathf.Sin(Mathf.PI / 6.0f);

    void SetTrifectaScale(float trifectaModule) {

        scaledXTrif = xTrif * trifectaModule;
        scaledYTrif = yTrif * trifectaModule;
    } 
}
