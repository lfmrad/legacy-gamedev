using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GC : MonoBehaviour {

    // ------------------- MetaControls
    public static string buildVersion = "> v0.4.2-alpha_20140717";

    // ------------------- Screen Properties
    public static Vector2 designScreenSize;
    public static Vector2 gameViewScreenSize;
    public static Vector2 currentScreenSize;
    public static Vector2 screenAdjRatio;
    public static float designScrDPI = 224f;
    public static float currentScrDPI;
    public static float screenDPIRatio = 1.0f;
    float cubesWidth = 16.0f;

    // ------------------- Gameplay Tracking
    public static int reorganizationCounter;
    public enum gameState { warmingUp, started, failed, complete, waiting, ready, restart };     // static here?
    public static gameState gameStatus;
 
    public virtual void Awake() {

        gameStatus = gameState.warmingUp;

        //if (!gameInitialized) {
        //    SetLevelParameters();
        //    gameStatus = gameState.started;
        //}

        //currentLevel = Application.loadedLevel;


    }

    // ------------------- Game States Controller

    FXController fx = new FXController();

    void Update() {

        // to see the threshold
        // Debug.DrawLine(Vector3.zero, Vector3.right * controlTouchDMTinCubes, Color.green);
        // Debug.DrawLine(Vector3.zero, Vector3.down * controlTouchDMTinCubes, Color.green);
        // Debug.Log("touchDeadZone = " + controlTouchDeadZone);
        // Debug.Log("gameStatus = " + gameStatus);

        switch (gameStatus) {

            case gameState.warmingUp:
                //SavePlayerDataStageStarted();
                if (StarLord.starLordHasCreatedTheStars) {

                    gameStatus = gameState.started;
                } 
                break;

            case gameState.started:
                break;

            case gameState.failed:
                //SavePlayerDataCommonProperties();
                //SavePlayerDataStageFailed();
                // GUIController.selectedGUI = GUIController.guiStatus.stageSelect;
                gameStatus = gameState.restart;
                break;

            case gameState.restart:
                                    
                    if(
                        PlayerController.playerControllerHasRestarted && 
                        PlayerAnimator.playerAnimatorHasRestarted &&
                        LevelEnder.levelEnderHasRestarted &&
                        VertexSlider.vertexSlidersHaveRestarted
                        ) 
                    {

                        gameStatus = gameState.started;
                    }
                
                break;

            case gameState.ready:
                //WaitingForPlayer();
                break;

            case gameState.complete:

                if (LevelEnder.levelEnderHasFinishedLevelComplete) {

                    gameStatus = gameState.restart;
                }
                
                //SavePlayerDataCommonProperties();
                //SavePlayerDataStageComplete();
                // GUIController.selectedGUI = GUIController.guiStatus.stageSelect;
                // gameStatus = gameState.waiting;
                break;
        }
    }

    
  

    /*void WaitingForPlayer() {
        if (GUIController.playerPressedAlterit) {
            GUIController.selectedGUI = GUIController.guiStatus.onGame;
            if (!Application.isLoadingLevel) {

                    gameStatus = gameState.warmingUp;
                    GUIController.playerPressedAlterit = false;
            }
        }
    }*/

    // ------------------- Stages Properties
    public static int currentLevel;
    public static bool gameInitialized = false;

    public static Dictionary<int, int> stageAndSector = new Dictionary<int, int>();
    public static Dictionary<int, int> sectorAndCPS = new Dictionary<int, int>();
    public static Dictionary<int, List<int>> stagesNeededForUnlock = new Dictionary<int, List<int>>();

    List<int> sectorTwoSecondSet = new List<int>();
    List<int> sectorThreeSet = new List<int>();

    public void SetLevelParameters() {

        gameInitialized = true;

        sectorAndCPS.Add(1, 7);
        sectorAndCPS.Add(2, 16);
        sectorAndCPS.Add(3, 21);

        stageAndSector.Add(1, 1);
        stageAndSector.Add(2, 1);
        stageAndSector.Add(3, 1);
        stageAndSector.Add(4, 2);
        stageAndSector.Add(5, 2);
        stageAndSector.Add(6, 2);
        stageAndSector.Add(7, 2);
        stageAndSector.Add(8, 2);
        stageAndSector.Add(9, 2);
        stageAndSector.Add(10, 3);
        stageAndSector.Add(11, 3);
        stageAndSector.Add(12, 3);

        sectorTwoSecondSet.Add(4);
        sectorTwoSecondSet.Add(5);
        sectorTwoSecondSet.Add(6);

        // test level lockings
        //PlayerPrefs.SetInt(4 + "_stageStatus", 0);
        //PlayerPrefs.SetInt(5 + "_stageStatus", 0);
        //PlayerPrefs.SetInt(6 + "_stageStatus", 0);
        // end test

        sectorThreeSet.Add(7);
        sectorThreeSet.Add(8);
        sectorThreeSet.Add(9);

        stagesNeededForUnlock.Add(7, sectorTwoSecondSet);
        stagesNeededForUnlock.Add(8, sectorTwoSecondSet);
        stagesNeededForUnlock.Add(9, sectorTwoSecondSet);
        stagesNeededForUnlock.Add(10, sectorThreeSet);
        stagesNeededForUnlock.Add(11, sectorThreeSet);
        stagesNeededForUnlock.Add(12, sectorThreeSet);
    }

    // ------------------- Player Prefs 
    /*
    void SavePlayerDataCommonProperties() {

        PlayerPrefs.SetFloat(currentLevel + "_lastTime", GUIController.gameTime);
        PlayerPrefs.SetFloat(currentLevel + "_stagePlayTime", PlayerPrefs.GetFloat(currentLevel + "_stagePlayTime") + GUIController.gameTime);
        PlayerPrefs.SetFloat("alteritPlayTime", PlayerPrefs.GetFloat("alteritPlayTime") + GUIController.gameTime);
    }
    void SavePlayerDataStageComplete() {

        if (!GUIController.optionsOverrideCPS) {

            if ((PlayerPrefs.GetInt(currentLevel + "_stageStatus") == 0) || (GUIController.gameTime < PlayerPrefs.GetFloat(currentLevel + "_bestTime"))) {

                Debug.Log(PlayerPrefs.GetInt(currentLevel + "_stageStatus"));
                PlayerPrefs.SetFloat(currentLevel + "_bestTime", GUIController.gameTime);
            }

            PlayerPrefs.SetInt(currentLevel + "_completedTimes", PlayerPrefs.GetInt(currentLevel + "_completedTimes") + 1);
            PlayerPrefs.SetInt(currentLevel + "_stageStatus", 1);
        }
    }
    void SavePlayerDataStageFailed() {
        
        PlayerPrefs.SetInt(currentLevel + "_failedTimes", PlayerPrefs.GetInt(currentLevel + "_failedTimes") + 1);
    }
    void SavePlayerDataStageStarted() {

        if (!PlayerPrefs.HasKey(currentLevel + "_stageStatus")) {
            PlayerPrefs.SetInt(currentLevel + "_stageStatus", 0);
        }
    }

    // ------------------- Other Resources
    // seen in: http://answers.unity3d.com/questions/179775/game-window-size-from-editor-window-in-editor-mode.html
    Vector2 GameViewScreenSize() {

        System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
        System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        System.Object Res = GetSizeOfMainGameView.Invoke(null, null);
        return (Vector2)Res;
    }
     * */
}
