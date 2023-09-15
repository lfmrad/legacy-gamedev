using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashController : MonoBehaviour {

	public RectTransform logo;
	Image logoImg;
	public Text msgNubeProds, msgPresents, msgTitle;
	Properties properties;
	public Color textColor;

	void Start () {

		properties = this.GetComponent<Properties>();
		properties.SetGameText(properties.gameLanguage);

		msgNubeProds.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.nubeprintProds);
		msgPresents.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.presentsWord);
		msgTitle.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.MPStheChallenge);

		logoImg = logo.GetComponent<Image>();
	}

	void Update () {
	
		if (Time.time <= 3.8f) {
			
			logoImg.color = ColorFader(Color.white, true, 3.8f, 0f);
			msgNubeProds.color = ColorFader(textColor, true, 3f, 0f);
			msgPresents.color = ColorFader(textColor, true, 3f, 0.5f);
			msgTitle.color = ColorFader(textColor, true, 2.8f, 1f);

//		} else if (Time.time > 4f & Time.time < 5f) {
//			
//			logoImg.canvasRenderer.SetColor(ColorFader(Color.white, false, 0.2f, 0f));
//			msgNubeProds.color = ColorFader(Color.white, false, 0.2f, 0.2f);
//			msgPresents.color = ColorFader(Color.white, false, 0.2f, 0.4f);
//			msgPresents.color = ColorFader(Color.white, false, 0.2f, 0.7f);
// 			lxmn, rvyunvnwcjcrxw jwm jac kh udrb onawjwmx vjacrwni jwmand
//
		} else {
			
			Application.LoadLevel("MainFinal");
		}
	}

	Color ColorFader(Color baseColor, bool fadeIn, float timeToLock, float startDelay) {
		Color initial, target;

		if (fadeIn) {
			initial = target = baseColor;
			initial.a = 0f;
		} else {
			initial = target = baseColor;
			target.a = 0f;
		}

		if (AnimationClock(clock.count) > timeToLock + startDelay) {
			return target;
		} else if (AnimationClock(clock.count) >= startDelay) {
			return Color.Lerp(initial, target, (AnimationClock(clock.count) - startDelay) / timeToLock);	
		} else {
			return initial;
		}
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
		return animationClock;
	}

}
