using UnityEngine;
using System.Collections;

public class VertexSlider : MonoBehaviour {

    public Transform targetTransform;
    Vector2 initialPos, targetPos;
    StageComposer stageComposer;
		static int vertexCount;

    void Start() {

        stageComposer = GetComponentInParent<StageComposer>();
        initialPos = transform.position;
		Debug.Log (initialPos);
        targetPos = targetTransform.position;
				vertexCount = 0;
    }

    float step = 0f;

    public static bool vertexSlidersHaveRestarted;
	
	void Update () {

        if (GC.gameStatus == GC.gameState.restart) {
						Debug.Log ("hi");
			stageComposer.triggered = false;
						transform.position = initialPos;
            step = 0f;
			vertexCount++;
						if (vertexCount == 6) {
								vertexSlidersHaveRestarted = true;
								vertexCount = 0;
						}
        }


        if (GC.gameStatus == GC.gameState.started) {

            if (step <= 1 && stageComposer.triggered) {

                step += Time.deltaTime * stageComposer.speed;
                transform.position = Vector2.Lerp(initialPos, targetPos, step);
                
            } 
            vertexSlidersHaveRestarted = false;
        }

	}
}
