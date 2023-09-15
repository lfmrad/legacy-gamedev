using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CO2LightSystem))]

public class CO2LightSystem : MonoBehaviour {

	public bool debugLightSystem;

	public Light dirAlpha;
	public Light dirBeta;
	public GameObject earthGlow;

	public GameController gameController;

	Renderer earthGlowRenderer;

	public Color[] dirAlphaColor;
	public Color[] dirBetaColor;
	public Color[] glowColor;
	
	Color currentDirAlphaColor;
	public Color targetDirAlphaColor;
	Color currentDirBetaColor;
	public Color targetDirBetaColor;
	Color currentGlowColor;
	public Color targetGlowColor;

	bool glowEnabled = true;
	bool inProgress = true;	

	int step;
	
	void Start () {

		#if UNITY_ANDROID
		earthGlow.SetActive(false);
		glowEnabled = false;
 		#endif

		if (glowEnabled) {
			earthGlowRenderer = earthGlow.GetComponent<Renderer>();
		}

		// Testing
		// dirAlpha.color = Color.red;
		// earthGlowRenderer.material.SetColor("_TintColor", Color.red * 0.25f);
		// lxmn, rvyunvnwcjcrxw jwm jac kh udrb onawjwmx vjacrwni jwmand
	}

	float transitionTimeFrame = 1.1f;

	void Update () {
		if (gameController.signalSentToC2LightSystem | inProgress) {
			if (AnimationClock(clock.count) == 0) {
				inProgress = true;
				step = (int)GetStep().x;

				currentDirAlphaColor = dirAlpha.color;
				targetDirAlphaColor = GetIntermediateColor(GetDirAlphaColor(step-1), GetDirAlphaColor(step), step);

				currentDirBetaColor = dirBeta.color;
				targetDirBetaColor = GetIntermediateColor(GetDirBetaColor(step-1), GetDirBetaColor(step), step);

				if (glowEnabled) {
					currentGlowColor = earthGlowRenderer.material.GetColor("_TintColor");
					targetGlowColor = GetIntermediateColor(GetGlowColor(step-1), GetGlowColor(step), step);
				}

				if (debugLightSystem) { Debug.Log("Light Transition Initialized / Current Step: " + step); }

			} else if (AnimationClock(clock.count) > 0 & AnimationClock(clock.count) <= transitionTimeFrame) {

				dirAlpha.color = ColorLerper(currentDirAlphaColor, targetDirAlphaColor, 1f, 0f);
				dirBeta.color = ColorLerper(currentDirBetaColor, targetDirBetaColor, 1f, 0f);

				if (glowEnabled) {
					earthGlowRenderer.material.SetColor("_TintColor", ColorLerper(currentGlowColor, targetGlowColor, 1f, 0f));
				}
				
			} else if (AnimationClock(clock.count) > transitionTimeFrame) {
				if (debugLightSystem) { Debug.Log("Light Transition Completed"); }
				AnimationClock(clock.restart);
				inProgress = false;
			}
		}
	}

	Color ColorLerper(Color initialColor, Color targetColor, float timeToTarget, float delayToTarget) {
		if (AnimationClock(clock.count) > timeToTarget + delayToTarget) {
			return targetColor;
		} else if (AnimationClock(clock.count) >= delayToTarget) {
			return Color.Lerp(initialColor, targetColor, (AnimationClock(clock.count) - delayToTarget) / timeToTarget);	
		} else {
			return initialColor;
		}
	}

	Color GetIntermediateColor(Color lowerColor, Color upperColor, int step) {
		float stepWidth;
		float ratio;

		if (step <= 5) {
			stepWidth = 38f;
		} else {
			stepWidth = 42.5f;
		}

		if (debugLightSystem) { 
			Debug.Log("Score-StepLowerBound = " + (gameController.CO2Score - (int)GetStep().y));
		 }

		ratio = ((gameController.CO2Score - GetStep().y) / stepWidth) / 1f;
		return Color.Lerp(lowerColor, upperColor, ratio);
	}

	Color GetDirAlphaColor(int step) {
		return dirAlphaColor[step];
	}

	Color GetDirBetaColor(int step) {
		return dirBetaColor[step];
	}

	Color GetGlowColor(int step) {
		return glowColor[step];
	}


	Vector2 GetStep() {
		int CO2 = gameController.CO2Score;

		if (-190 <= CO2 & CO2 < -152) {
			return new Vector2(1, -190f);

		} else if (-152 <= CO2 & CO2 < -114) {
			return new Vector2(2, -152f);

		} else if (-114 <= CO2 & CO2 < -76) {
			return new Vector2(3, -114f);

		} else if (-76 <= CO2 & CO2 < -38) {
			return new Vector2(4, -76f);

		} else if (-38 <= CO2 & CO2 < 0) {
			return new Vector2(5, -38f);

		} else if (0 <= CO2 & CO2 < 42.5) {
			return new Vector2(6, 0f);

		} else if (42.5 <= CO2 & CO2 < 85) {
			return new Vector2(7, 42.5f);

		} else if (85 <= CO2 & CO2 < 127.5) {
			return new Vector2(8, 85f);

		} else if (127.5 <= CO2 & CO2 <= 170) {
			return new Vector2(9, 127.5f);
		}

		return Vector2.zero;
	}

	enum clock {restart = 1, count = 2, pause = 3}
	bool justStarted = true, restarted = false;
	float animationClock = 0f;
	float lastStopped = 0f, startedAt = 0f;

	float AnimationClock(clock statusRecieved) {
		if(statusRecieved == clock.restart & !restarted) {
			animationClock = 0f;
			lastStopped = Time.time;
			restarted = true;
			justStarted = true;
		} else if (statusRecieved == clock.count) {
			if(justStarted) {
				justStarted = false;
				startedAt = Time.time;
			}
			animationClock = Time.time - startedAt;
			restarted = false;
		}
		// Debug.Log("Animation Clock: " + animationClock + "/ Time.time: " + (Time.time - startedAt));
		return animationClock;
	}
}
