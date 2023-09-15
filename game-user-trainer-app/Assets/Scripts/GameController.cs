using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(GameController))]
[RequireComponent(typeof(Properties))]

public class GameController : MonoBehaviour {
	public static bool debug;
	public bool debugEnabled = false;
		
	public Transform earth;

	public bool signalSentToC2LightSystem = false;

	public RectTransform superTop, upperFrame, upperBody;
	public RectTransform answersPanel;
	public RectTransform answer1, answer2, answer3, answer4;
	public RectTransform moneyField, bonusField, CO2Field;
	public RectTransform menuButton, imageButton;
	public RectTransform leftButton, rightButton;
	public RectTransform progressButton;
	public RectTransform feedbackField, feedbackTitleField, leftBracketField, rightBracketField;
	public RectTransform deltaScoreField;
	public RectTransform contextualImage;	

	public RectTransform specialPopupC2A2;

	Button ans1Button, ans2Button, ans3Button, ans4Button;
	Button menuButtonComponent, imageButtonComponent;
	Button progressButtonComponent;
	Button leftButtonComponent, rightButtonComponent;

	public Text ans1Text, ans2Text, ans3Text, ans4Text;
	public Text menuButtonText, imageButtonText;
	public Text progressButtonText;
	public Text descriptionText, challengeTitle;
	public Text moneyTextScore, moneyTextDelta, bonusTextScore, bonusTextDelta, CO2TextScore, CO2TextDelta;
	public Text feedbackText, feedbackTitle, feedbackLeftBracket, feedbackRightBracket;
	public Text introText;
	public Text introTitleText;
	public Text moneyPointsDeltaText, bonusPointsDeltaText, CO2PointsDeltaText;
	public Text moneyBigDeltaText, bonusBigDeltaText, CO2BigDeltaText;

	public Image darkCurtain;

	Image contextualImageComponent;

	[SerializeField]
	int currentChallenge;
	int selectedAnswer;
	Properties properties;
	
	void Start () {
		// ************ INITIALIZATION *********************************************
		debug = debugEnabled;

		// Get Buttons
		ans1Button = answer1.GetComponent<Button>();
		ans2Button = answer2.GetComponent<Button>();
		ans3Button = answer3.GetComponent<Button>();
		ans4Button = answer4.GetComponent<Button>();
		menuButtonComponent = menuButton.GetComponent<Button>();
		imageButtonComponent = imageButton.GetComponent<Button>();
		progressButtonComponent = progressButton.GetComponent<Button>();
		leftButtonComponent = leftButton.GetComponent<Button>();
		rightButtonComponent = rightButton.GetComponent<Button>();	

		// Get Images
		contextualImageComponent = contextualImage.GetComponent<Image>();

		// Set Properties
		properties = GetComponent<Properties>();
		properties.SetGameText(properties.gameLanguage);
		properties.SetGameImages(properties.gameLanguage);
		properties.SetChallengeAnswersLayout();
		properties.SetChallengeCO2();
		properties.SetChallengeBonus();
		properties.SetChallengeMoney();
		properties.SetImagesLayout();

		properties.SetUILockedPositions();
		
		GetUIDefaultParameters();

		answersHistory = new int[properties.numberOfChallenges+1];

		popupC2A2ButtonComponent = popupC2A2Button.GetComponent<Button>();
		#if UNITY_ANDROID
		SetAndroidButtonColors();
 		#endif

		// ***********************************************************************

		// Coming from intro/menu mockup
		transitionCompleted = false;
		currentUIState = UIState.enterIntro;
		currentChallenge = 1;

		// Tests
		// contextualImage.gameObject.SetActive(true);
		// contextualImageComponent.overrideSprite = properties.GetChallengeImage(1);
		// lxmn, rvyunvnwcjcrxw jwm jac kh udrb onawjwmx vjacrwni jwmand
		// Debug.Log(properties.GetMiscellanyText(Properties.miscellanyTextTypes.nubeprintWord));		
	}

	public Color androidButtonHigh, androidButtonPressed;
	public Color androidButtonHighPrincipal;

	void SetAndroidButtonColors() {
		var colors = ans1Button.colors;
		colors.highlightedColor = androidButtonHigh;
		colors.pressedColor = androidButtonPressed;

		ans1Button.colors = colors;
		ans2Button.colors  = colors;
		ans3Button.colors = colors;
		ans4Button.colors = colors;
		progressButtonComponent.colors = colors;
		leftButtonComponent.colors = colors;
		rightButtonComponent.colors = colors;

		var colorsPrincipal = imageButtonComponent.colors;
		colorsPrincipal.highlightedColor = androidButtonHighPrincipal;
		colorsPrincipal.pressedColor = androidButtonPressed;

		imageButtonComponent.colors = colorsPrincipal;
		menuButtonComponent.colors = colorsPrincipal;
		popupC2A2ButtonComponent.colors = colorsPrincipal;
	}

	public enum UIState {
		playMode = 1,
		feedbackMode = 2,
		scoreMode = 3,
		
		preGame = 4,
		deltaScoreToFeedback = 5,
		feedbackToNextChallenge = 6,
		feedbackToScore = 7,
		
		playToMenu = 8,
		feedbackToMenu = 9,
		scoreToMenu = 10,
		continueGame = 11,

		menuMode = 12,
		introMode = 13,
		enterIntro = 14,
		turningPage = 15,
		exitIntro = 16,
		deltaScoreMode = 17,
		playToDeltaScore = 18,
		deltaScoreExit = 19,
		feedbackEnter = 20,
		feedbackExit = 21,
		scoreEnter = 22,
		scoreExit = 23
	}

	UIState currentUIState;
	bool transitionCompleted = true;
	bool feedbackMode = false;
	bool scoreMode = false;

	void Update() {
		if(!transitionCompleted) {
			switch (currentUIState) {
				case UIState.menuMode:
					break;
				case UIState.enterIntro:
					EnterIntro();
					break;
				case UIState.introMode:
					ReportUIStatus();
					transitionCompleted = true;	
					break;
				case UIState.turningPage:
					TurnPage();
					break;
				case UIState.exitIntro:
					ExitIntro();
					break;
				case UIState.preGame:
					PreGame();
					break;
				case UIState.playMode:
					ReportUIStatus();
					transitionCompleted = true;	
					break;
				case UIState.continueGame:
					ContinueGame();
					break;
				case UIState.feedbackEnter:
					FeedbackEnter();
					break;
				case UIState.feedbackExit:
					FeedbackExit();
					break;
				case UIState.feedbackMode:
					ReportUIStatus();
					transitionCompleted = true;	
					break;
				case UIState.playToDeltaScore:
					PlayToDeltaScore();
					break;
				case UIState.deltaScoreMode:
					ReportUIStatus();
					transitionCompleted = true;	
					break;
				case UIState.deltaScoreExit:
					DeltaScoreExit();
					break;
				case UIState.scoreMode:
					ReportUIStatus();
					transitionCompleted = true;	
					break;
				case UIState.feedbackToNextChallenge:
					DeltaScoreExit();
					break;
				case UIState.deltaScoreToFeedback:
					break;
				case UIState.scoreEnter:
					ScoreEnter();
					break;
				case UIState.scoreExit:
					ScoreExit();
					break;
				case UIState.playToMenu:
					break;
				case UIState.feedbackToMenu:
					break;
				case UIState.scoreToMenu:
					break;
				default:
					break;
			}
		}

		if (imageButtonPressed) {
			if (!nextIsHide) {
				ShowScreen();
			} else {
				HideScreen();
			}
		}

		if (gameOver & gameOverNotSet) {
			TriggerGameOver();
		}

		if (popupAnswerSelected) {
			TriggerPopUpC2A2();
		}
	}

	// -------------------------------------------------------------- Others

	public Color darkenedCurtain;
	public Color mildDarkenedCurtain; 

	public RectTransform gameOverPanel;
	public Text gameOverText, gameOverMsgText;
	Color gameOverTextD, gameOverMsgD;

	bool gameOver = false;
	bool gameOverNotSet = true;

	void TriggerGameOver() {
		if(AnimationClock(clock.count) == 0) {
			darkCurtain.gameObject.SetActive(true);
			gameOverPanel.gameObject.SetActive(true);
	
			imageButtonText.gameObject.SetActive(true);
			gameOverMsgText.text = properties.GetChallengeText(currentChallenge, Properties.textSection.feedAns3);
			imageButtonText.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.restartWord);
			nubeprintLogo.gameObject.SetActive(false);
	
			SetPlayModeInteractivity(false);

			StartCoroutine(ShowGameOverMsg(1f, 0.5f));
			StartCoroutine(DarkenScreen(true, 0.5f, 0f, darkenedCurtain));
		} else if (AnimationClock(clock.count) > 1.5f) {
			StopAllCoroutines();
			AnimationClock(clock.restart);
			
			gameOverNotSet = false;
			imageButtonComponent.interactable = true;
			menuButtonComponent.interactable = true;
		}
	}

	public RectTransform popupC2A2Panel, popupC2A2Button;
	Button popupC2A2ButtonComponent;
	public Text popupC2A2Msg, popupC2A2ButtonMsg;
	public Image popupC2A2Image;
	Color popupC2A2TextColorD;

	bool popupAnswerSelected = false;

	void TriggerPopUpC2A2() {
		if(AnimationClock(clock.count) == 0) {
			popupC2A2Msg.text = properties.GetSpecialText(0);
			popupC2A2ButtonMsg.text = properties.GetSpecialText(1);

			popupC2A2Panel.gameObject.SetActive(true);
			popupC2A2Button.gameObject.SetActive(true);

			popupC2A2ButtonComponent.interactable = false;
			darkCurtain.gameObject.SetActive(true);

			SetPlayModeInteractivity(false);
			StartCoroutine(DarkenScreen(true, 0.5f, 0f, mildDarkenedCurtain));
			StartCoroutine(ShowPopupC2A2(0.7f, 0f));

		} else if (AnimationClock(clock.count) > 0.8f) {
			StopAllCoroutines();
			AnimationClock(clock.restart);
			popupC2A2ButtonComponent.interactable = true;
			imageButtonComponent.interactable = false;
			menuButtonComponent.interactable = true;
			popupAnswerSelected = false;
		}
	}

	IEnumerator ShowPopupC2A2(float timeToAppear, float timeDelay) {
		Vector3 buttonAnchor = popupC2A2Button.anchoredPosition3D;
		Vector3 buttonOffset = new Vector3(buttonAnchor.x, -20f, buttonAnchor.z);

		while(true) {
			popupC2A2Msg.color = ColorFader(popupC2A2TextColorD, true, timeToAppear, timeDelay);
			popupC2A2ButtonMsg.color = ColorFader(popupC2A2TextColorD, true, timeToAppear, timeDelay);
			popupC2A2Image.color = ColorFader(Color.white, true, timeToAppear, timeDelay);
			popupC2A2Button.anchoredPosition3D = posLerper(timeToAppear, timeDelay, buttonOffset, buttonAnchor);
			
			yield return new WaitForSeconds(transitionStep);
		}
	}

	IEnumerator ShowGameOverMsg(float timeToAppear, float timeDelay) {
		while(true) {
			gameOverText.color = ColorFader(gameOverTextD, true, timeToAppear, timeDelay);
			gameOverMsgText.color = ColorFader(gameOverMsgD, true, timeToAppear, timeDelay);
			yield return new WaitForSeconds(transitionStep);
		}
	}

	public RectTransform nubeprintLogo;
	Vector3 contextualImageCD;
	bool imageButtonPressed = false;
	public Camera gameCamera;

	void ShowScreen() {
		if(AnimationClock(clock.count) == 0) {

			SetPlayModeInteractivity(false);
			darkCurtain.gameObject.SetActive(true);
			contextualImage.gameObject.SetActive(true);

			StartCoroutine(DarkenScreen(true, 0.5f, 0f, GetDarkenedColor()));
			StartCoroutine(TransitionShowImage(true, 0.5f, 0.5f));
		} else if (AnimationClock(clock.count) > 1.1f) {

			StopAllCoroutines();
			imageButtonComponent.interactable = true;
			menuButtonComponent.interactable = true;
			AnimationClock(clock.restart);
			imageButtonPressed = false;
			nextIsHide = true;
		}
	}

	bool nextIsHide = false;

	void HideScreen() {
		if(AnimationClock(clock.count) == 0) {
			imageButtonComponent.interactable = false;
			menuButtonComponent.interactable = false;
			
			StartCoroutine(DarkenScreen(false, 0.5f, 0.5f, GetDarkenedColor()));
			StartCoroutine(TransitionShowImage(false, 0.5f, 0f));
		} else if (AnimationClock(clock.count) > 1.1f) {
			StopAllCoroutines();
			SetPlayModeInteractivity(true);
			darkCurtain.gameObject.SetActive(false);
			contextualImage.gameObject.SetActive(false);

			
			AnimationClock(clock.restart);
			imageButtonPressed = false;
			nextIsHide = false;
		}
	}

	public Color nubeprintDarkened;

	Color GetDarkenedColor() {

		if (currentChallenge == 2) {
			return Color.white;
		} else {
			return nubeprintDarkened;
		}
	}

	IEnumerator TransitionShowImage(bool enter, float time, float delay) {
		while(true) {

			contextualImageComponent.color = ColorFader(Color.white, enter, time, delay);
			yield return new WaitForSeconds(transitionStep);
		}
	}

	IEnumerator DarkenScreen(bool fade, float timeToDarkness, float delay, Color target) {
		Color origin = new Color(0, 0, 0, 0);

		while(true) {
			if (fade) {
				darkCurtain.color = ColorLerper(origin, target, timeToDarkness, delay);
			} else {
				darkCurtain.color = ColorLerper(target, origin, timeToDarkness, delay);
			}
			
			yield return new WaitForSeconds(transitionStep);
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


	// -------------------------------------------------------------- UI TRANSITIONS MANAGERS
	void ReportUIStatus() {
		if (debug) {
			Debug.Log("UIState [ " + currentUIState + " ]");
		}
	}

	void EnterIntro() {
		if (AnimationClock(clock.count) == 0) {
			ReportUIStatus();
			currentPage = 1;
			introMode = false;
			GetColorsCurrentDefaults();
			SetIntroDefaultLayout();
			SetIntroInteractivity(false);
			StartCoroutine(TransitionEnterIntro());
	
		} else if (AnimationClock(clock.count) > 1.5f) {
			StopAllCoroutines();
			SetIntroInteractivity(true);
			AnimationClock(clock.restart);
			currentUIState = UIState.introMode;
		}
	}

	int currentPage, previousPage;
	int repeater = 0;

	void TurnPage() {
		if (AnimationClock(clock.count) == 0) {
			ReportUIStatus();
			SetIntroInteractivity(false);
			SetTurnPageLayout();
			
			if (repeater == 0) {
				StartCoroutine(TransitionTurnPage());
			} else if (repeater == 1) {
				introText.text = properties.GetIntroText(currentPage);
				StartCoroutine(TransitionLoadNewPage());
			}
		} else if (AnimationClock(clock.count) > 0.7f) {
			StopAllCoroutines();
			AnimationClock(clock.restart);
			repeater++;
			if (repeater == 2) {
				SetIntroInteractivity(true);
				currentUIState = UIState.introMode;
				repeater = 0;
				transitionCompleted = true;
			}
		}
	}
	
	void ExitIntro() {
		if (AnimationClock(clock.count) == 0) {
			ReportUIStatus();
			GetColorsCurrentDefaults();
			SetIntroInteractivity(false);
			StartCoroutine(TransitionExitIntro());
		} else if (AnimationClock(clock.count) > 0.6f) {
			StopAllCoroutines();
			AnimationClock(clock.restart);
			currentUIState = UIState.preGame;
		}
	}

	// First Game In
	void PreGame() {
		if (AnimationClock(clock.count) == 0) {
			ReportUIStatus();
			SetGameLayout();
			UpdateScore();
			ShowDeltas(false);
			SetPlayModeInteractivity(false);

			StartCoroutine(TransitionPreGame());
		} else if (AnimationClock(clock.count) > 1.5f) {
			StopAllCoroutines();
			SetPlayModeInteractivity(true);
			imageButtonComponent.interactable = true;
			AnimationClock(clock.restart);
			currentUIState = UIState.playMode;
		}
	}

	void PlayToDeltaScore() {
		if (AnimationClock(clock.count) == 0) {
			ReportUIStatus();

			GetColorsCurrentDefaults();
			SetPlayModeInteractivity(false);
			SetFeedbackModeInteractivity(false);
			SetScoreLayout();

			StartCoroutine(TransitionPlayToDeltaScore());
		} else if (AnimationClock(clock.count) > 1.3f) {
			SetFeedbackModeInteractivity(true);
			StopAllCoroutines();
			AnimationClock(clock.restart);
			currentUIState = UIState.deltaScoreMode;
			WriteProgress(currentChallenge, selectedAnswer);	
		}
	}

	void FeedbackEnter() {
		if (AnimationClock(clock.count) == 0) {
			ReportUIStatus();
			GetColorsCurrentDefaults();
			SetPlayModeInteractivity(false);
			SetFeedbackModeInteractivity(false);
			SetFeedbackLayout();
			StartCoroutine(TransitionFeedbackEnter());
			scoreMode = false;
		} else if (AnimationClock(clock.count) > 0.6f) {
			SetFeedbackModeInteractivity(true);
			StopAllCoroutines();
			AnimationClock(clock.restart);
			currentUIState = UIState.feedbackMode;
		}
	}

	void FeedbackExit() {
		if (AnimationClock(clock.count) == 0) {
			ReportUIStatus();
			GetColorsCurrentDefaults();
			SetFeedbackModeInteractivity(false);
			StartCoroutine(TransitionFeedbackExit());
		} else if (AnimationClock(clock.count) > 0.6f) {
			StopAllCoroutines();
			AnimationClock(clock.restart);

			if (!scoreMode) {
				currentUIState = UIState.feedbackEnter;	
			} else {
				currentUIState = UIState.scoreEnter;
			}
		}
	}

	void ScoreEnter() {
		if (AnimationClock(clock.count) == 0) {
			ReportUIStatus();
			imageButtonComponent.interactable = false;
			menuButtonComponent.interactable = false;
			progressButtonComponent.interactable = false;
			SetFeedbackModeInteractivity(false);
			SetFinalScoreLayout();
			StartCoroutine(TransitionScoreEnter());
		} else if (AnimationClock(clock.count) > 2.8f) {
			StopAllCoroutines();
			AnimationClock(clock.restart);
			imageButtonComponent.interactable = true; // no function
			menuButtonComponent.interactable = true;
			progressButtonComponent.interactable = true;
			currentUIState = UIState.scoreMode;
		}
	}

	bool introMode = false;

	void ScoreExit() {
		if (AnimationClock(clock.count) == 0) {
			ReportUIStatus();
			SetFeedbackModeInteractivity(false);
			StartCoroutine(TransitionScoreExit());
		} else if (AnimationClock(clock.count) > 2.8f) {
			StopAllCoroutines();
			AnimationClock(clock.restart);
			if (introMode) {
				currentUIState = UIState.enterIntro;
			} else {
				currentUIState = UIState.feedbackEnter;
			}
		}
	}

	// Feedback Out
	void FeedbackToNextChallenge() {
		if (AnimationClock(clock.count) == 0) {

			ReportUIStatus();
			GetAnswersCurrentDefaults();
			GetColorsCurrentDefaults();
			imageButtonComponent.interactable = false;
			
			SetFeedbackModeInteractivity(false);

			StartCoroutine(TransitionFeedbackToNextChallenge());
		} else if (AnimationClock(clock.count) > 1f) {
			StopAllCoroutines();
			ShowDeltas(false);

			AnimationClock(clock.restart);
			currentUIState = UIState.continueGame;
		}
	}

	void DeltaScoreExit() {
		if (AnimationClock(clock.count) == 0) {
			
			ReportUIStatus();
			GetAnswersCurrentDefaults();
			GetColorsCurrentDefaults();
			imageButtonComponent.interactable = false;
			
			SetFeedbackModeInteractivity(false);

			StartCoroutine(TransitionDeltaScoreExit(feedbackMode));
		} else if (AnimationClock(clock.count) > 1f) {
			StopAllCoroutines();
			ShowDeltas(false);
			WriteProgress(currentChallenge, selectedAnswer);
			AnimationClock(clock.restart);
			if (!scoreMode) {
				currentUIState = UIState.continueGame;
			} else if (scoreMode) {
				currentUIState = UIState.scoreEnter;
			}
		}
	}

	// Game In
	void ContinueGame() {
		if (AnimationClock(clock.count) == 0) {
			ReportUIStatus();
			
			SetGameLayout();	
			GetAnswersCurrentDefaults();
			GetColorsCurrentDefaults();
		
			StartCoroutine(TransitionContinueGame());
		} else if (AnimationClock(clock.count) > 1.5f) {
			StopAllCoroutines();
			SetPlayModeInteractivity(true);
			AnimationClock(clock.restart);
			currentUIState = UIState.playMode;
		}
	}

	// -------------------------------------------------------------- INTERACTION

	public void UpperLeftButtonClicked() {
		if (gameOver) {
			Application.LoadLevel(Application.loadedLevel);
			if (debug) { Debug.Log("Restart game!"); }
		} else if (currentUIState == UIState.introMode) {
			currentUIState = UIState.exitIntro;
			transitionCompleted = false;
		} else if (currentUIState == UIState.playMode || currentUIState == UIState.feedbackMode || currentUIState == UIState.deltaScoreMode) {
			imageButtonPressed = true;
		} else if (currentUIState == UIState.scoreMode) {
			Application.LoadLevel(Application.loadedLevel);
		}
	}

	public void UpperRightButtonClicked() {
		Application.Quit();
	}

	public void LeftArrowClicked() {
		if (currentUIState == UIState.introMode) {
			previousPage = currentPage;
			currentPage--;
			currentPage = Mathf.Clamp(currentPage, 1, 3);
			currentUIState = UIState.turningPage;
			transitionCompleted = false;
		}
	}

	public void RightArrowClicked() {
		if (currentUIState == UIState.introMode) {
			previousPage = currentPage;
			currentPage++;
			currentPage = Mathf.Clamp(currentPage, 1, 3);
			currentUIState = UIState.turningPage;
			transitionCompleted = false;
		}
	}
	
	public void AnswerSelected(int answerNumber) {
		selectedAnswer = answerNumber;
		if (currentChallenge == 1 & selectedAnswer == 3) {
			if (debug) {Debug.Log("Game Over Triggered!");}
			gameOver = true;
			gameOverNotSet = true;
		} else if (currentChallenge == 2 & selectedAnswer == 2) {
			if (debug) {Debug.Log("Pop-up C2A2 Triggered!");}
			popupAnswerSelected = true;
		} else {
			currentUIState = UIState.playToDeltaScore;
			transitionCompleted = false;
		}
	}

	public void ContinueAfterPopupClicked() {
		currentUIState = UIState.playToDeltaScore;
		transitionCompleted = false;
	}

	public void ProgressButtonClicked() {
		if (currentUIState == UIState.deltaScoreMode & currentChallenge < 6) {
			currentChallenge++;
			currentUIState = UIState.deltaScoreExit;
		} else if (currentUIState == UIState.deltaScoreMode & currentChallenge > 5) {
			currentChallenge = 1;
			scoreMode = true;
			currentUIState = UIState.deltaScoreExit;
		} else if (currentUIState == UIState.feedbackMode & currentChallenge < 6) {
			currentChallenge++;
			currentUIState = UIState.feedbackExit;
		} else if (currentUIState == UIState.feedbackMode & currentChallenge > 5) {
			currentChallenge = 1;
			currentUIState = UIState.feedbackExit;
			scoreMode = true;
		} else if (currentUIState == UIState.introMode) {
			currentUIState = UIState.exitIntro;	
		} else if (currentUIState == UIState.scoreMode) {
			currentUIState = UIState.scoreExit;	
		}
		transitionCompleted = false;
	}

	// -------------------------------------------------------------- GAME CFG

	void WriteProgress(int challenge, int selectedAnswer) {

		PlayerPrefs.SetInt(challenge.ToString(), selectedAnswer);
	}

	void WriteBestScore() {
		if (finalScore > PlayerPrefs.GetInt("BestScore") || !PlayerPrefs.HasKey("BestScore"))  {
			PlayerPrefs.SetInt("BestScore", finalScore);
		}
	}

	void WriteNewLast() {
			PlayerPrefs.SetInt("LastScore", finalScore);
	}

	string GetBestScore() {
		return PlayerPrefs.GetInt("BestScore").ToString();
	}

	string GetLastScore() {
		if (PlayerPrefs.HasKey("LastScore")) {
			return PlayerPrefs.GetInt("LastScore").ToString();
		} else {
			return "-";
		}	
	}

	int[] answersHistory;
	public int moneyScore, moneyDelta;
	public int CO2Score, CO2Delta;
	public int bonusScore, bonusDelta;

	int finalScore;

	int GetFinalScore() {
		if (bonusScore > 0 & moneyScore > 0) {
			finalScore = moneyScore * (Mathf.Abs(bonusScore)+1);
		} else if (bonusScore > 0 & moneyScore < 0) {
			finalScore = moneyScore / (Mathf.Abs(bonusScore)+1);
		} else if (bonusScore < 0 & moneyScore > 0) {
			finalScore = moneyScore / (Mathf.Abs(bonusScore)+1);
		} else if (bonusScore < 0 & moneyScore < 0) {
			finalScore = moneyScore * (Mathf.Abs(bonusScore)+1);
		
		} else if (bonusScore == 0 & moneyScore > 0) {
			finalScore = moneyScore;
		} else { // moneyScore == 0
			finalScore = 0;
		}
		return finalScore;
	}

	void GetCurrentDeltas() {
		moneyDelta = properties.GetChallengeMoney(currentChallenge, selectedAnswer);
		bonusDelta = properties.GetChallengeBonus(currentChallenge, selectedAnswer);
		CO2Delta = properties.GetChallengeCO2(currentChallenge, selectedAnswer);
	}

	void UpdateScore() {
		answersHistory[currentChallenge] = selectedAnswer;
		
		GetCurrentDeltas();
		moneyScore += moneyDelta;
		CO2Score += CO2Delta;
		bonusScore += bonusDelta;

		moneyTextScore.text = moneyScore.ToString();
		moneyTextDelta.text = GetDeltaSign(moneyDelta) + moneyDelta.ToString();
		moneyBigDeltaText.text = GetDeltaSign(moneyDelta) + moneyDelta.ToString();

		bonusTextScore.text = bonusScore.ToString();
		bonusTextDelta.text = GetDeltaSign(bonusDelta) + bonusDelta.ToString();
		bonusBigDeltaText.text = GetDeltaSign(bonusDelta) + bonusDelta.ToString();

		CO2TextScore.text = CO2Score.ToString();
		CO2TextDelta.text = GetDeltaSign(CO2Delta) + CO2Delta.ToString();
		CO2BigDeltaText.text = GetDeltaSign(CO2Delta) + CO2Delta.ToString();

		if (debug) {
			Debug.Log("Score updated to - Money = " + moneyScore + " / Bonus = " + bonusScore + " / CO2 = " + CO2Score);
		}
	}

	string GetDeltaSign(int score) {
		if (score == 0) {
			return "  ";
		} else if (score > 0) {
			return "+";
		} else if (score < 0) {
			return " ";
		}
		return "";
	}

	string GetDeltaSignExplicit(int score) {
		if (score == 0) {
			return "";
		} else if (score > 0) {
			return "+";
		} else if (score < 0) {
			return "";
		}
		return "";
	}

	void ShowDeltas(bool state) {
		moneyTextDelta.gameObject.SetActive(state);
		bonusTextDelta.gameObject.SetActive(state);
		CO2TextDelta.gameObject.SetActive(state);
	}

	// -------------------------------------------------------------- LAYOUT CFG

	void SetFinalScoreLayout() {
		feedbackRightBracket.gameObject.SetActive(false);
		feedbackLeftBracket.gameObject.SetActive(false);
		feedbackTitle.gameObject.SetActive(false);

		finalScorePanel.gameObject.SetActive(true);

		imageButtonText.gameObject.SetActive(true);
		imageButtonText.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.restartWord);
		nubeprintLogo.gameObject.SetActive(false);

		finalScoreText1.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.yourFinalScoreIs);
		finalScoreText2.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.outOf);
		finalScoreActual.text = GetFinalScore().ToString();
		finalScoreMax.text = 720.ToString();
		moneyTotalText.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.totalMoney);
		moneyTotalNo.text = moneyScore.ToString();
		bonusTotalText.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.totalBonus);
		bonusTotalNo.text = bonusScore.ToString();
		CO2TotalText.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.totalCO2);
		CO2TotalNo.text = CO2Score.ToString();

		descriptionText.gameObject.SetActive(false);
		progressButtonText.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.reviewFeedbackWord);
		introTitleText.gameObject.SetActive(true);
		introTitleText.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.finalScoreWord);
		challengeTitle.text = "";

		WriteBestScore();

		bestText.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.bestScore);
		lastText.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.lastScore);
		bestNo.text = GetBestScore();
		lastNo.text = GetLastScore();

		WriteNewLast();

		progressButton.gameObject.SetActive(true);
		progressButton.anchoredPosition3D = properties.GetUIElementAnchor(Properties.UIElement.progressButton, UIState.feedbackMode);
		progressButton.localRotation = properties.GetUIElemenRotation(Properties.UIElement.progressButton, UIState.feedbackMode);

		
	}


	void SetGameLayout() {
		// Disables components
		introField.gameObject.SetActive(false);
		introTitleText.gameObject.SetActive(false);
		leftButton.gameObject.SetActive(false);
		rightButton.gameObject.SetActive(false);
		progressButton.gameObject.SetActive(false);
		feedbackField.gameObject.SetActive(false);
		feedbackRightBracket.gameObject.SetActive(false);
		feedbackLeftBracket.gameObject.SetActive(false);
		feedbackTitle.gameObject.SetActive(false);
		deltaScoreField.gameObject.SetActive(false);
		progressButton.gameObject.SetActive(false);
		ShowDeltas(false);

		// Enables components
		imageButtonComponent.gameObject.SetActive(true);
		menuButtonComponent.gameObject.SetActive(true);
		answersPanel.gameObject.SetActive(true);
		CO2Field.gameObject.SetActive(true);
		bonusField.gameObject.SetActive(true);
		moneyField.gameObject.SetActive(true);
		descriptionText.gameObject.SetActive(true);
		earth.gameObject.SetActive(true);

		// Sets Others' Layout Defaults
		upperBody.anchoredPosition3D = properties.GetUIElementAnchor(Properties.UIElement.upperBody, UIState.playMode);
		upperFrame.anchoredPosition3D = properties.GetUIElementAnchor(Properties.UIElement.upperFrame, UIState.playMode);
		superTop.anchoredPosition3D = properties.GetUIElementAnchor(Properties.UIElement.superTop, UIState.playMode);


		imageButtonText.gameObject.SetActive(false);
		nubeprintLogo.gameObject.SetActive(true);

		// Sets Answers' Layout
		SetAnswersLayoutToChallenge();	

		SetChallengeImage();

		// Sets Text
		SetChallengeText();
		SetImageButtonText(false);

	}

	void SetChallengeText() {
		menuButtonText.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.exitWord);
		challengeTitle.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.challengeWord) + " " + currentChallenge.ToString() + " / 6" ;
		descriptionText.text = ans1Text.text = properties.GetChallengeText(currentChallenge,Properties.textSection.description);
		ans1Text.text = properties.GetChallengeText(currentChallenge,Properties.textSection.ans1);
		ans2Text.text = properties.GetChallengeText(currentChallenge,Properties.textSection.ans2);
		ans3Text.text = properties.GetChallengeText(currentChallenge,Properties.textSection.ans3);
		ans4Text.text = properties.GetChallengeText(currentChallenge,Properties.textSection.ans4);
	}

	void SetChallengeImage() {
		contextualImageComponent.overrideSprite = properties.GetChallengeImage(currentChallenge);
		contextualImage.anchoredPosition3D = properties.GetChallengeImageAnchor(currentChallenge);
		contextualImage.sizeDelta = new Vector2(contextualImage.sizeDelta.x, properties.GetChallengeImageHeight(currentChallenge));
		if (debug) {
			Debug.Log("Challenge " + currentChallenge + " image loaded!");
		}
		if(currentChallenge == 2) {
			popupC2A2Image.overrideSprite = properties.GetAnswerImage(currentChallenge, 2);
			if (debug) {
				Debug.Log("Answer image loaded!");
			}
		}
	}

	void SetAnswersLayoutToChallenge() {
		answer1.anchoredPosition3D = properties.GetChallengeAnswerAnchor(currentChallenge, 1);
		answer1.sizeDelta = new Vector2(answer1.sizeDelta.x, properties.GetChallengeAnswerSizeY(currentChallenge, 1));
		answer1.localRotation = properties.GetChallengeAnswerRotation(currentChallenge, 1);
		answer2.anchoredPosition3D = properties.GetChallengeAnswerAnchor(currentChallenge, 2);
		answer2.sizeDelta = new Vector2(answer2.sizeDelta.x, properties.GetChallengeAnswerSizeY(currentChallenge, 2));
		answer2.localRotation = properties.GetChallengeAnswerRotation(currentChallenge, 2);
		answer3.anchoredPosition3D = properties.GetChallengeAnswerAnchor(currentChallenge, 3);
		answer3.sizeDelta = new Vector2(answer3.sizeDelta.x, properties.GetChallengeAnswerSizeY(currentChallenge, 3));
		answer3.localRotation = properties.GetChallengeAnswerRotation(currentChallenge, 3);
		answer4.anchoredPosition3D = properties.GetChallengeAnswerAnchor(currentChallenge, 4);
		answer4.sizeDelta = new Vector2(answer4.sizeDelta.x, properties.GetChallengeAnswerSizeY(currentChallenge, 4));
		answer4.localRotation = properties.GetChallengeAnswerRotation(currentChallenge, 4);
	}

	void SetPlayModeInteractivity(bool interaction) {
		ans1Button.interactable = interaction;
		ans2Button.interactable = interaction;
		ans3Button.interactable = interaction;
		ans4Button.interactable = interaction;
		imageButtonComponent.interactable = interaction;
		menuButtonComponent.interactable = interaction;
	}

	RectTransform feedbackSelectedAns;
	Vector3 feedbackTAnchor, feedbackLBracketAnchor, feedbackRBracketAnchor;

	void SetFeedbackLayout() {
		finalScorePanel.gameObject.SetActive(false);
		introTitleText.gameObject.SetActive(false);
		descriptionText.gameObject.SetActive(true);

		// Enables more components
		feedbackField.gameObject.SetActive(true);
		feedbackRightBracket.gameObject.SetActive(true);
		feedbackLeftBracket.gameObject.SetActive(true);
		feedbackTitle.gameObject.SetActive(true);
		progressButton.gameObject.SetActive(true);
		answersPanel.gameObject.SetActive(true);
		imageButton.gameObject.SetActive(true);

		imageButtonText.gameObject.SetActive(false);
		nubeprintLogo.gameObject.SetActive(true);

		// Sets default positions for feedbackMode
		feedbackField.anchoredPosition3D = properties.GetUIElementAnchor(Properties.UIElement.feedbackField, UIState.feedbackMode);
		feedbackTAnchor = feedbackTitleField.anchoredPosition3D = properties.GetUIElementAnchor(Properties.UIElement.feedbackTitle, UIState.feedbackMode);
		feedbackRBracketAnchor = rightBracketField.anchoredPosition3D = properties.GetUIElementAnchor(Properties.UIElement.rightBracket, UIState.feedbackMode);
		feedbackLBracketAnchor = leftBracketField.anchoredPosition3D = properties.GetUIElementAnchor(Properties.UIElement.leftBracket, UIState.feedbackMode);
		progressButton.anchoredPosition3D = properties.GetUIElementAnchor(Properties.UIElement.progressButton, UIState.feedbackMode);
		progressButton.localRotation = properties.GetUIElemenRotation(Properties.UIElement.progressButton, UIState.feedbackMode);


		GetCurrentDeltas();
		string dS = GetDeltaSignExplicit(moneyDelta) + moneyDelta;
		string bS = GetDeltaSignExplicit(bonusDelta) + bonusDelta;
		string coS = GetDeltaSignExplicit(CO2Delta) + CO2Delta;
		string thisChoice = properties.GetMiscellanyText(Properties.miscellanyTextTypes.thisChoiceChanged);
		string thisChoiceDidNot = properties.GetMiscellanyText(Properties.miscellanyTextTypes.thisChoiceDidNotChange);
		string pointsForFE = properties.GetMiscellanyText(Properties.miscellanyTextTypes.moneyPoints);
		string bonusForGM = properties.GetMiscellanyText(Properties.miscellanyTextTypes.bonusPoints);
		string byWord = properties.GetMiscellanyText(Properties.miscellanyTextTypes.byWord);
		string CO2F = properties.GetMiscellanyText(Properties.miscellanyTextTypes.CO2Points);
		string intro, deltaInfo;
		
		if (moneyDelta != 0 | bonusDelta != 0 | CO2Delta != 0) {
			intro = "\n\n <i>(" + thisChoice;
			deltaInfo = intro;
			if (moneyDelta != 0) {
			deltaInfo += " " + pointsForFE + " " + byWord + " " + dS + ".";
			}
			if (bonusDelta != 0) {
				deltaInfo += " " + bonusForGM + " " + byWord + " " + bS + ".";
			}
			if (CO2Delta != 0) {
				deltaInfo += " " + CO2F + " " + byWord + " " + coS + ".";
			}
			deltaInfo += ")</i>";
		} else {
			deltaInfo = "\n\n <i>(" + thisChoiceDidNot + ".)</i>";
		}
		

		feedbackTitle.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.feedbackWord);
		imageButtonText.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.showImgWord);

		SetChallengeText();

		SetChallengeImage();

		// PlaceHolder
		feedbackSelectedAns = answer1;

		selectedAnswer = answersHistory[currentChallenge];
		if(debug) {
			Debug.Log("Challenge: " + currentChallenge + " | Answer: " + answersHistory[currentChallenge]);
		}
		// Feedback Text
		switch (selectedAnswer) {
			case 1:
				SetAnswerHeightAndRotation(answer1);
				answer1.gameObject.SetActive(true); answer2.gameObject.SetActive(false);
				answer3.gameObject.SetActive(false); answer4.gameObject.SetActive(false);
				feedbackText.text = properties.GetChallengeText(currentChallenge, Properties.textSection.feedAns1);
				break;
			case 2:
				SetAnswerHeightAndRotation(answer2);
				answer1.gameObject.SetActive(false); answer2.gameObject.SetActive(true);
				answer3.gameObject.SetActive(false); answer4.gameObject.SetActive(false);
				feedbackText.text = properties.GetChallengeText(currentChallenge, Properties.textSection.feedAns2);
				break;
			case 3:
				SetAnswerHeightAndRotation(answer3);
				answer1.gameObject.SetActive(false); answer2.gameObject.SetActive(false);
				answer3.gameObject.SetActive(true); answer4.gameObject.SetActive(false);
				feedbackText.text = properties.GetChallengeText(currentChallenge, Properties.textSection.feedAns3);
				break;
			case 4:
				SetAnswerHeightAndRotation(answer4);
				answer1.gameObject.SetActive(false); answer2.gameObject.SetActive(false);
				answer3.gameObject.SetActive(false); answer4.gameObject.SetActive(true);
				feedbackText.text = properties.GetChallengeText(currentChallenge, Properties.textSection.feedAns4);
				break;
		}
		feedbackText.text = feedbackText.text + deltaInfo;

		// Progress Button Text
		if (currentChallenge < 6) {
			progressButtonText.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.nextChallengeWord);
		} else {
			progressButtonText.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.scoreAgain);
		} 
	}

	void SetAnswerHeightAndRotation(RectTransform answer) {
		feedbackSelectedAns = answer;
		answer.sizeDelta = new Vector2(answer1.sizeDelta.x, properties.GetChallengeAnswerSizeY(currentChallenge, selectedAnswer));
		answer.localRotation = properties.GetChallengeAnswerRotation(currentChallenge, 1);
	}

	Vector3 scoreTAnchor, scoreLBracketAnchor, scoreRBracketAnchor;

	void SetScoreLayout() {
		// Enables more components
		feedbackRightBracket.gameObject.SetActive(true);
		feedbackLeftBracket.gameObject.SetActive(true);
		feedbackTitle.gameObject.SetActive(true);
		deltaScoreField.gameObject.SetActive(true);
		progressButton.gameObject.SetActive(true);

		popupC2A2Panel.gameObject.SetActive(false);
		popupC2A2Button.gameObject.SetActive(false);
		darkCurtain.gameObject.SetActive(false);

		// Sets default positions for feedbackMode
		scoreTAnchor = feedbackTitleField.anchoredPosition3D = properties.GetUIElementAnchor(Properties.UIElement.feedbackTitle, UIState.deltaScoreMode);
		scoreRBracketAnchor = rightBracketField.anchoredPosition3D = properties.GetUIElementAnchor(Properties.UIElement.rightBracket, UIState.deltaScoreMode);
		scoreLBracketAnchor = leftBracketField.anchoredPosition3D = properties.GetUIElementAnchor(Properties.UIElement.leftBracket, UIState.deltaScoreMode);
		progressButton.anchoredPosition3D = properties.GetUIElementAnchor(Properties.UIElement.progressButton, UIState.feedbackMode);
		progressButton.localRotation = properties.GetUIElemenRotation(Properties.UIElement.progressButton, UIState.feedbackMode);

		// Text
		feedbackTitle.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.scoreWord);

		moneyDelta = properties.GetChallengeMoney(currentChallenge, selectedAnswer);
		CO2Delta = properties.GetChallengeCO2(currentChallenge, selectedAnswer);
		bonusDelta = properties.GetChallengeBonus(currentChallenge, selectedAnswer);

		

		SetDeltaScoreText(moneyPointsDeltaText, Properties.miscellanyTextTypes.moneyPoints, moneyDelta);
		SetDeltaScoreText(bonusPointsDeltaText, Properties.miscellanyTextTypes.bonusPoints, bonusDelta);
		SetDeltaScoreText(CO2PointsDeltaText, Properties.miscellanyTextTypes.CO2Points, CO2Delta);

		if (currentChallenge < 6) {
			progressButtonText.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.nextChallengeWord);
		} else {
			progressButtonText.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.finalScoreWord);
		}
	}

	void SetDeltaScoreText(Text reference, Properties.miscellanyTextTypes textType, int score) {
		string byWord = properties.GetMiscellanyText(Properties.miscellanyTextTypes.byWord);
		string mainText = properties.GetMiscellanyText(textType);

		if (score > 0) {
			string increasedWord = properties.GetMiscellanyText(Properties.miscellanyTextTypes.increasedWord);
			reference.text = mainText + " " + increasedWord + " " + byWord;
		} else if (score == 0) {
			string noChangeWord = properties.GetMiscellanyText(Properties.miscellanyTextTypes.noChange);
			reference.text = mainText + " " + noChangeWord;
		} else {
			string decreasedWord = properties.GetMiscellanyText(Properties.miscellanyTextTypes.decreasedWord);
			reference.text = mainText + " " + decreasedWord + " " + byWord;
		}
	}

	// For feedback & scoreMode
	void SetFeedbackModeInteractivity(bool interaction) {
		progressButtonComponent.interactable = interaction;
		imageButtonComponent.interactable = interaction;
		menuButtonComponent.interactable = interaction;
	}

	void SetTurnPageLayout() {
		progressButtonText.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.startChallengesWord);
		progressButton.localRotation = properties.GetUIElemenRotation(Properties.UIElement.progressButton, UIState.introMode);

		if (currentPage == 1) {
			leftButton.gameObject.SetActive(false);
			rightButton.gameObject.SetActive(true);
		} else if (currentPage == 2) {
			leftButton.gameObject.SetActive(true);
			rightButton.gameObject.SetActive(true);
		} else if (currentPage == 3) {
			leftButton.gameObject.SetActive(true);
			progressButton.gameObject.SetActive(true);
		}
	}

	public RectTransform introField;

	void SetIntroDefaultLayout() {
		introText.gameObject.SetActive(true);
		introTitleText.gameObject.SetActive(true);
		imageButtonComponent.gameObject.SetActive(true);
		menuButtonComponent.gameObject.SetActive(true);
		leftButton.gameObject.SetActive(false);
		rightButton.gameObject.SetActive(true);
		introField.gameObject.SetActive(true);
		progressButton.gameObject.SetActive(false);
		descriptionText.gameObject.SetActive(false);
		finalScorePanel.gameObject.SetActive(false);

		gameOverPanel.gameObject.SetActive(false);
		darkCurtain.gameObject.SetActive(false);

		answersPanel.gameObject.SetActive(false);
		CO2Field.gameObject.SetActive(false);
		bonusField.gameObject.SetActive(false);
		moneyField.gameObject.SetActive(false);
		descriptionText.gameObject.SetActive(false);
		earth.gameObject.SetActive(false);

		upperBody.anchoredPosition3D = properties.GetUIElementAnchor(Properties.UIElement.upperBody, UIState.introMode);
		upperFrame.anchoredPosition3D = properties.GetUIElementAnchor(Properties.UIElement.upperFrame, UIState.introMode);

		imageButtonText.gameObject.SetActive(true);
		imageButtonText.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.skipWord);
		nubeprintLogo.gameObject.SetActive(false);

		menuButtonText.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.exitWord);
		challengeTitle.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.nubeprintWord);
		introText.text = properties.GetIntroText(1);
		introTitleText.text = ans1Text.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.introWord);
	}

	void SetIntroInteractivity(bool state) {
		rightButtonComponent.interactable = state;
		leftButtonComponent.interactable = state;
		imageButtonComponent.interactable = state;
		progressButtonComponent.interactable = state;
	}

	void SetImageButtonText(bool showingImage) {
		if(!showingImage) {
			imageButtonText.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.showImgWord);
		} else {
			imageButtonText.text = properties.GetMiscellanyText(Properties.miscellanyTextTypes.hideImgWord);
		}
	}

	Vector3 upperBodyPlayDPos, upperBodyIntroDPos;
	Vector3 upperFramePlayDPos;
	Vector3 earthDPos;
	Quaternion moneyFieldDRot, bonusFieldDRot;
	Vector3 answersLockPos; Quaternion answersLockRot;

	Vector3 leftBracketDPos, rightBracketDPos;
	Vector3 progressButtonFeedbackDPos, feedbackTitleDPos, feedbackFieldDPos;

	Color introTextD;
	Color answersTextD, descriptionTextD;
	Color CO2DeltaD, BonusDeltaD, MoneyDeltaD, scoresD;
	Color feedbackTextD;
	Color upperRightButtonD, upperLeftButtonD;

	void GetUIDefaultParameters() {
		answersLockPos = properties.GetChallengeAnswerAnchor(1, 1);
		answersLockRot = properties.GetChallengeAnswerRotation(1, 1);

		upperBodyPlayDPos = properties.GetUIElementAnchor(Properties.UIElement.upperBody, UIState.playMode);
		upperBodyIntroDPos = properties.GetUIElementAnchor(Properties.UIElement.upperBody, UIState.introMode);

		earthDPos = properties.GetUIElementAnchor(Properties.UIElement.earth, UIState.playMode);

		upperFramePlayDPos = properties.GetUIElementAnchor(Properties.UIElement.upperFrame, UIState.playMode);
		moneyFieldDRot = properties.GetUIElemenRotation(Properties.UIElement.moneyField, UIState.playMode);
		bonusFieldDRot = properties.GetUIElemenRotation(Properties.UIElement.bonusField, UIState.playMode);

		leftBracketDPos = properties.GetUIElementAnchor(Properties.UIElement.leftBracket, UIState.feedbackMode);
		rightBracketDPos = properties.GetUIElementAnchor(Properties.UIElement.rightBracket, UIState.feedbackMode);
		progressButtonFeedbackDPos = properties.GetUIElementAnchor(Properties.UIElement.progressButton, UIState.feedbackMode);

		feedbackTitleDPos = properties.GetUIElementAnchor(Properties.UIElement.feedbackTitle, UIState.feedbackMode);
		feedbackFieldDPos = properties.GetUIElementAnchor(Properties.UIElement.feedbackField, UIState.feedbackMode);

		GetAnswersCurrentDefaults();
	
		introTextD = introText.color;
		feedbackTextD = feedbackText.color;
		answersTextD = ans1Text.color;
		descriptionTextD = descriptionText.color;
		MoneyDeltaD = moneyTextDelta.color;
		BonusDeltaD = bonusTextDelta.color;
		CO2DeltaD = CO2TextDelta.color;
		scoresD = moneyTextScore.color;
		upperRightButtonD = menuButtonText.color;
		upperLeftButtonD = imageButtonText.color;

		popupC2A2TextColorD = popupC2A2Msg.color;

		gameOverTextD = gameOverText.color;
		gameOverMsgD = gameOverMsgText.color;
	}

	Vector3 ans1CDPos, ans2CDPos, ans3CDPos, ans4CDPos;
	Quaternion ans1CDRot, ans2CDRot, ans3CDRot, ans4CDRot;

	void GetAnswersCurrentDefaults() {
		ans1CDPos = answer1.anchoredPosition3D;
		ans1CDRot = answer1.localRotation;
		ans2CDPos = answer2.anchoredPosition3D;
		ans2CDRot = answer2.localRotation;
		ans3CDPos = answer3.anchoredPosition3D;
		ans3CDRot = answer3.localRotation;
		ans4CDPos = answer4.anchoredPosition3D;
		ans4CDRot = answer4.localRotation;
	}


	Color introTextCD;
	Color answersTextCD, descriptionTextCD;
	Color CO2DeltaCD, BonusDeltaCD, MoneyDeltaCD, scoresCD;
	Color feedbackTextCD;
	Color upperRightButtonCD, upperLeftButtonCD;
	
	void GetColorsCurrentDefaults() {
		introTextCD = introText.color;
		feedbackTextCD = feedbackText.color;
		answersTextCD = ans1Text.color;
		descriptionTextCD = descriptionText.color;
		MoneyDeltaCD = moneyTextDelta.color;
		BonusDeltaCD = bonusTextDelta.color;
		CO2DeltaCD = CO2TextDelta.color;
		scoresCD = moneyTextScore.color;
		upperRightButtonCD = menuButtonText.color;
		upperLeftButtonCD = imageButtonText.color;
	}

	// -------------------------------------------------------------- ANIMATION / MANAGERS
	bool WaitForSeconds(float waitForThis) {
		if(AnimationClock(clock.count) < waitForThis) {
			return true;
		}
		return false;
	} 

	int lastCallID = -1;

	bool doOnceCheck(int callID) {
		if(callID == lastCallID) {
			return false;
		} else {
			lastCallID = callID;
			return true;
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
			if (debug) {
				Debug.Log("Clock restarted!");
			}
		} else if (statusRecieved == clock.count) {
			if(justStarted) {
				justStarted = false;
				startedAt = Time.time;
				if (debug) {
					Debug.Log("Clock resumed!");
				}
			}
			animationClock = Time.time - startedAt;
			restarted = false;
		}
		// Debug.Log("Animation Clock: " + animationClock + "/ Time.time: " + (Time.time - startedAt));
		return animationClock;
	}

	// -------------------------------------------------------------- ANIMATION / TRANSITIONS

	public float transitionStep = 0f;

	IEnumerator TransitionEnterIntro() {

		float fadeInTime = 1f;
		float fadeInDelay = 0.5f;

		float timeToLockUpperFrame = 0.5f;
		float delayUpperFrame = 0f;
		float yOffset = 100f;
		Vector3 upperFrameAnchor = properties.GetUIElementAnchor(Properties.UIElement.upperFrame, UIState.introMode);

		float timeToLockArrows = 0.5f;
		float delayArrows = 1f;
		float xOffset = 60f;
		Vector3 rightArrowAnchor =  properties.GetUIElementAnchor(Properties.UIElement.rightArrowButton, UIState.introMode);
		
		while (true) {
			introText.color = ColorFader(introTextD, true, fadeInTime, fadeInDelay);		
			upperFrame.anchoredPosition3D = posLerper(timeToLockUpperFrame, delayUpperFrame, new Vector3(upperFrameAnchor.x, yOffset, upperFrameAnchor.z), upperFrameAnchor);
			rightButton.anchoredPosition3D = posLerper(timeToLockArrows, delayArrows, new Vector3(xOffset, rightArrowAnchor.y, rightArrowAnchor.z), rightArrowAnchor);

			yield return new WaitForSeconds(transitionStep);
		}	
	}

	IEnumerator TransitionTurnPage() {
		float fadeInTime = 0.5f;
		float fadeInDelay = 0f;

		float timeToLockArrows = 0.5f;
		float delayArrows = 0f;
		float xOffset = 60f;
		Vector3 rightArrowAnchor =  properties.GetUIElementAnchor(Properties.UIElement.rightArrowButton, UIState.introMode);
		Vector3 leftArrowAnchor =  properties.GetUIElementAnchor(Properties.UIElement.leftArrowButton, UIState.introMode);

		float progressButtonTimeToLock = 0.5f;
		float progressButtonDelay = 0f;
		float yOffset = 120;
		Vector3 progressButtonAnchor = properties.GetUIElementAnchor(Properties.UIElement.progressButton, UIState.introMode);

		while (true) {
			if (currentPage == 2 & previousPage == 1) {
				leftButton.anchoredPosition3D = posLerper(timeToLockArrows, delayArrows, new Vector3(-xOffset, leftArrowAnchor.y, leftArrowAnchor.z), leftArrowAnchor);
			} else if (currentPage == 3 & previousPage == 2) {
				rightButton.anchoredPosition3D = posLerper(timeToLockArrows, delayArrows, rightArrowAnchor, new Vector3(xOffset, rightArrowAnchor.y, rightArrowAnchor.z));
				progressButton.anchoredPosition3D = posLerper(progressButtonTimeToLock, progressButtonDelay, new Vector3(progressButtonAnchor.x, -yOffset, progressButtonAnchor.z), progressButtonAnchor);
			} else if (currentPage == 2 & previousPage == 3) {
				rightButton.anchoredPosition3D = posLerper(timeToLockArrows, delayArrows, new Vector3(xOffset, rightArrowAnchor.y, rightArrowAnchor.z), rightArrowAnchor);
				progressButton.anchoredPosition3D = posLerper(progressButtonTimeToLock, progressButtonDelay, progressButtonAnchor, new Vector3(progressButtonAnchor.x, -yOffset, progressButtonAnchor.z));
			} else if (currentPage == 1 & previousPage == 2) {
				leftButton.anchoredPosition3D = posLerper(timeToLockArrows, delayArrows, leftArrowAnchor, new Vector3(-xOffset, leftArrowAnchor.y, leftArrowAnchor.z));
			}
			introText.color = ColorFader(introTextD, false, fadeInTime, fadeInDelay);	
			yield return new WaitForSeconds(transitionStep);
		}
	}

	IEnumerator TransitionLoadNewPage() {
		float fadeInTime = 0.2f;
		float fadeInDelay = 0f;

		while (true) {
			introText.color = ColorFader(introTextD, true, fadeInTime, fadeInDelay);	
			yield return new WaitForSeconds(transitionStep);
		}
	}

	IEnumerator TransitionExitIntro() {
		float fadeInTime = 0.5f;
		float fadeInDelay = 0f;

		float timeToLockUpperFrame = 0.5f;
		float delayUpperFrame = 0f;
		float upperFrameYOffset = 150f;
		
		float progressButtonTimeToLock = 0.5f;
		float progressButtonDelay = 0f;
		float progressButtonYOffset = 120;
		
		float timeToLockArrows = 0.5f;
		float delayArrows = 0f;
		float arrowsXOffset = 60f;

		Vector3 upperFrameAnchor = properties.GetUIElementAnchor(Properties.UIElement.upperFrame, UIState.introMode);
		Vector3 progressButtonAnchor = properties.GetUIElementAnchor(Properties.UIElement.progressButton, UIState.introMode);
		Vector3 rightArrowAnchor =  properties.GetUIElementAnchor(Properties.UIElement.rightArrowButton, UIState.introMode);
		Vector3 leftArrowAnchor =  properties.GetUIElementAnchor(Properties.UIElement.leftArrowButton, UIState.introMode);
		
		while (true) {
			introText.color = ColorFader(introTextD, false, fadeInTime, fadeInDelay);		
			upperFrame.anchoredPosition3D = posLerper(timeToLockUpperFrame, delayUpperFrame, upperFrameAnchor, new Vector3(upperFrameAnchor.x, upperFrameYOffset, upperFrameAnchor.z));

			if (currentPage == 1) {
				rightButton.anchoredPosition3D = posLerper(timeToLockArrows, delayArrows, rightArrowAnchor, new Vector3(arrowsXOffset, rightArrowAnchor.y, rightArrowAnchor.z));
			} else if (currentPage == 2) {
				rightButton.anchoredPosition3D = posLerper(timeToLockArrows, delayArrows, rightArrowAnchor, new Vector3(arrowsXOffset, rightArrowAnchor.y, rightArrowAnchor.z));
				leftButton.anchoredPosition3D = posLerper(timeToLockArrows, delayArrows, leftArrowAnchor, new Vector3(-arrowsXOffset, leftArrowAnchor.y, leftArrowAnchor.z));
			} else if (currentPage == 3) {
				leftButton.anchoredPosition3D = posLerper(timeToLockArrows, delayArrows, leftArrowAnchor, new Vector3(-arrowsXOffset, leftArrowAnchor.y, leftArrowAnchor.z));
				progressButton.anchoredPosition3D = posLerper(progressButtonTimeToLock, progressButtonDelay, progressButtonAnchor, new Vector3(progressButtonAnchor.x, -progressButtonYOffset, progressButtonAnchor.z));
			}

			yield return new WaitForSeconds(transitionStep);
		}	
	}

	IEnumerator TransitionPreGame() {
		
		float earthOffset = -50f;

		float time = 1f;
		float delay = 0.5f;

		Vector3 CO2FieldAnchor = CO2Field.anchoredPosition3D;
		Vector3 CO2FieldOffset = new Vector3(CO2FieldAnchor.x, -36f, CO2FieldAnchor.z);

		while (true) {
			IntroduceAnswersAndDescription();
			upperBody.anchoredPosition3D = posLerper(0.3f, 0.3f, upperBodyIntroDPos, upperBodyPlayDPos);
			earth.transform.position = posLerper(1.1f, 0.3f, new Vector3(earthDPos.x, earthOffset, earthDPos.z), earthDPos);
			
			moneyField.localRotation = rotLerper(time, delay, Quaternion.Euler(new Vector3(0, -140f, 0f)), moneyFieldDRot);
			bonusField.localRotation = rotLerper(time, delay, Quaternion.Euler(new Vector3(0, 140f, 0f)), bonusFieldDRot);
			CO2Field.anchoredPosition3D = posLerper(time, delay, CO2FieldOffset, CO2FieldAnchor);
			yield return new WaitForSeconds(transitionStep);
		}
	}

	// ---------------- < FEEDBACK ENTER/EXIT > 
	float feedbackXOffset = 1000f;
	float feedbackYOffset = 200f;

	IEnumerator TransitionFeedbackEnter() {
		float answerTimeToLock = 0.5f;
		float answerDelay = 0f;
		float bracketsTimeToLock = 0.1f;
		float bracketsDelay = 0.3f; // *1.2 = 1.15
		float colorFadeTime = 0.4f;
		float colorFadeDelay = 0.1f;

		while (true) {	
			feedbackSelectedAns.anchoredPosition3D = posLerper(answerTimeToLock, answerDelay, new Vector3(-feedbackXOffset, answersLockPos.y, answersLockPos.z), answersLockPos);
			upperFrame.anchoredPosition3D = posLerper(0.5f, 0f, new Vector3(0, feedbackYOffset, upperFramePlayDPos.z), upperFramePlayDPos);
			progressButton.anchoredPosition3D = posLerper(0.5f, 0f, new Vector3(-feedbackXOffset, progressButtonFeedbackDPos.y, progressButtonFeedbackDPos.z), progressButtonFeedbackDPos);
			EmbraceWithBrackets(movDir.enter, feedbackTAnchor, feedbackLBracketAnchor, feedbackRBracketAnchor, bracketsTimeToLock, bracketsDelay);
			feedbackText.color = ColorFader(feedbackTextD, true, colorFadeTime, colorFadeDelay);
			yield return new WaitForSeconds(transitionStep);
		}
	}

	IEnumerator TransitionFeedbackExit() {
		float answerTimeToLock = 0.5f;
		float answerDelay = 0f;
		float bracketsTimeToLock = 0.1f;
		float bracketsDelay = 0.3f; // *1.2 = 1.15
		float colorFadeTime = 0.4f;
		float colorFadeDelay = 0.1f;

		while (true) {	
			feedbackSelectedAns.anchoredPosition3D = posLerper(answerTimeToLock, answerDelay, answersLockPos, new Vector3(-feedbackXOffset, answersLockPos.y, answersLockPos.z));
			upperFrame.anchoredPosition3D = posLerper(0.5f, 0f, upperFramePlayDPos, new Vector3(0, feedbackYOffset, upperFramePlayDPos.z));
			progressButton.anchoredPosition3D = posLerper(0.5f, 0f, progressButtonFeedbackDPos, new Vector3(-feedbackXOffset, progressButtonFeedbackDPos.y, progressButtonFeedbackDPos.z));
			EmbraceWithBrackets(movDir.exit, feedbackTAnchor, feedbackLBracketAnchor, feedbackRBracketAnchor, bracketsTimeToLock, bracketsDelay);
			feedbackText.color = ColorFader(feedbackTextD, false, colorFadeTime, colorFadeDelay);
			yield return new WaitForSeconds(transitionStep);
		}
	}
	// ---------------- </ FEEDBACK ENTER/EXIT > 

	IEnumerator TransitionPlayToDeltaScore() {
		float xOffset = 1300f;

		float selectedAnsTimeToLock = 0.3f;
		float selectedAnsDelay = 0.5f;
		float notSelectedAnsTimeToLock = 0.5f;

		float flashTime = 1f;
		float flashDelay = 0f;

		while (true) {	
			// Reorganizes answers to feedback screen
			if (!(selectedAnswer == 1)) {
				answer1.anchoredPosition3D = posLerper(notSelectedAnsTimeToLock, 0f, ans1CDPos, new Vector3(xOffset, ans1CDPos.y, ans1CDPos.z));
			}
			if (selectedAnswer == 2) {
				answer2.anchoredPosition3D = posLerper(selectedAnsTimeToLock, selectedAnsDelay, ans2CDPos, answersLockPos);
				answer2.localRotation = rotLerper(selectedAnsTimeToLock, selectedAnsDelay, ans2CDRot, answersLockRot);
			} else {
				answer2.anchoredPosition3D = posLerper(notSelectedAnsTimeToLock, 0f, ans2CDPos, new Vector3(-xOffset, ans2CDPos.y, ans2CDPos.z));
			}
			if (selectedAnswer == 3) {
				answer3.anchoredPosition3D = posLerper(selectedAnsTimeToLock, selectedAnsDelay, ans3CDPos, answersLockPos);
				answer3.localRotation = rotLerper(selectedAnsTimeToLock, selectedAnsDelay, ans3CDRot, answersLockRot);
			} else {
				answer3.anchoredPosition3D = posLerper(notSelectedAnsTimeToLock, 0f, ans3CDPos, new Vector3(xOffset, ans3CDPos.y, ans3CDPos.z));
			}
			if (selectedAnswer == 4) {
				answer4.anchoredPosition3D = posLerper(selectedAnsTimeToLock, selectedAnsDelay, ans4CDPos, answersLockPos);
				answer4.localRotation = rotLerper(selectedAnsTimeToLock, selectedAnsDelay, ans4CDRot, answersLockRot);
			} else {
				answer4.anchoredPosition3D = posLerper(notSelectedAnsTimeToLock, 0f, ans4CDPos, new Vector3(-xOffset, ans4CDPos.y, ans4CDPos.z));
			}
		
			// Progress Button
			progressButton.anchoredPosition3D = posLerper(0.5f, 0.6f, new Vector3(-xOffset, progressButtonFeedbackDPos.y, progressButtonFeedbackDPos.z), progressButtonFeedbackDPos);
			
			EmbraceWithBrackets(movDir.enter, scoreTAnchor, scoreLBracketAnchor, scoreRBracketAnchor, 0.3f, 0.7f);
	
			DeltaScoreFade(true, 0.3f, 0.9f);
			UpdateScoreFlash(flashTime, flashDelay);

			yield return new WaitForSeconds(transitionStep);
		}
	}

	IEnumerator TransitionDeltaScoreExit(bool feedbackModeEnabled) {
		float xOffset = 1300f;
		float answerTimeToGetOut = 0.5f;
		float answerDelayToGetOut = 0.2f;
		float proButtonOffset = xOffset;

		float yOffset = 200f;
		float upperFrameTimeToHide = 1f;
		float upperFrameDelayToHide = 0f;

		float deltasTimeToHide = 1f;
		float deltasDelayToHide = 0f;

		float feedbackTextTimeToHide = deltasTimeToHide;
		float feedbackTextDelayToHide = deltasDelayToHide;
		
		float feedbackTextOffset = 50f;
		float feedbackTimeToLock = 0.3f;
		float feedbackDelay = 0f;

		Vector3 scoreTitleDPos = properties.GetUIElementAnchor(Properties.UIElement.feedbackTitle, UIState.deltaScoreMode);

		while (true) {
			switch (selectedAnswer) {
				case 1:
					answer1.anchoredPosition3D = posLerper(answerTimeToGetOut, answerDelayToGetOut, ans1CDPos, new Vector3(xOffset, ans1CDPos.y, ans1CDPos.z));
					proButtonOffset = -xOffset;
					break;
				case 2:
					answer2.anchoredPosition3D = posLerper(answerTimeToGetOut, answerDelayToGetOut, ans2CDPos, new Vector3(-xOffset, ans2CDPos.y, ans2CDPos.z));
					proButtonOffset = xOffset;
					break;
				case 3:
					answer3.anchoredPosition3D = posLerper(answerTimeToGetOut, answerDelayToGetOut, ans3CDPos, new Vector3(xOffset, ans3CDPos.y, ans3CDPos.z));
					proButtonOffset = -xOffset;
					break;
				case 4:
					answer4.anchoredPosition3D = posLerper(answerTimeToGetOut, answerDelayToGetOut, ans4CDPos, new Vector3(-xOffset, ans4CDPos.y, ans4CDPos.z));
					proButtonOffset = xOffset;
					break;
			}
			progressButton.anchoredPosition3D = posLerper(answerTimeToGetOut, answerDelayToGetOut, progressButtonFeedbackDPos, new Vector3(proButtonOffset, progressButtonFeedbackDPos.y, progressButtonFeedbackDPos.z));
			upperFrame.anchoredPosition3D = posLerper(upperFrameTimeToHide, upperFrameDelayToHide, upperFramePlayDPos, new Vector3(0, yOffset, upperFramePlayDPos.z));
		
			moneyTextDelta.color = ColorFader(MoneyDeltaCD, false, deltasTimeToHide, deltasDelayToHide);
			bonusTextDelta.color = ColorFader(BonusDeltaCD, false, deltasTimeToHide, deltasDelayToHide);
			CO2TextDelta.color = ColorFader(CO2DeltaCD, false, deltasTimeToHide, deltasDelayToHide);		

			DeltaScoreFade(false, 0.5f, 0f);
			EmbraceWithBrackets(movDir.exit, scoreTAnchor, scoreLBracketAnchor, scoreRBracketAnchor, 0.3f, 0.7f);
	
			yield return new WaitForSeconds(transitionStep);
		}

		yield return new WaitForSeconds(transitionStep);
	}

	// ---------------- HELPERS

	void DeltaScoreFade(bool fadeIn, float time, float delay) {
		moneyPointsDeltaText.color = ColorFader(feedbackTextD, fadeIn, time, delay);
		bonusPointsDeltaText.color = ColorFader(feedbackTextD, fadeIn, time, delay);
		CO2PointsDeltaText.color = ColorFader(feedbackTextD, fadeIn, time, delay);
		moneyBigDeltaText.color = ColorFader(GetDeltaColor(moneyDelta, false), fadeIn, time, delay);
		bonusBigDeltaText.color = ColorFader(GetDeltaColor(bonusDelta, false), fadeIn, time, delay);
		CO2BigDeltaText.color = ColorFader(GetDeltaColor(CO2Delta, true), fadeIn, time, delay);
	}

	void EmbraceWithBrackets(movDir dir, Vector3 titleAnchor, Vector3 leftAnchor, Vector3 rightAnchor, float time, float delay) {

		float xOffset = 50f;

		if (dir == movDir.enter) {
			feedbackTitleField.anchoredPosition3D = posLerper(time, delay * 1.2f, new Vector3(-xOffset, titleAnchor.y, titleAnchor.z), titleAnchor);
			leftBracketField.anchoredPosition3D  = posLerper(time, delay, new Vector3(-xOffset, leftAnchor.y, leftAnchor.z), leftAnchor);
			rightBracketField.anchoredPosition3D  = posLerper(time, delay, new Vector3(xOffset, rightAnchor.y, rightAnchor.z), rightAnchor);
		} else {
			feedbackTitleField.anchoredPosition3D = posLerper(time, delay * 0.8f, titleAnchor, new Vector3(-xOffset, titleAnchor.y, titleAnchor.z));
			leftBracketField.anchoredPosition3D  = posLerper(time, delay, leftAnchor, new Vector3(-xOffset, leftAnchor.y, leftAnchor.z));
			rightBracketField.anchoredPosition3D  = posLerper(time, delay, rightAnchor, new Vector3(xOffset, rightAnchor.y, rightAnchor.z));
		}
	}

	bool scoreUpdated = false;

	void UpdateScoreFlash(float flashTime, float flashDelay) {

		if (AnimationClock(clock.count) > flashDelay + flashTime) {
			scoreUpdated = false;
			signalSentToC2LightSystem = false;
			moneyTextScore.color = bonusTextScore.color = moneyTextScore.color = scoresCD;

			moneyTextDelta.color = GetDeltaColor(moneyDelta, false);
			bonusTextDelta.color = GetDeltaColor(bonusDelta, false);
			CO2TextDelta.color = GetDeltaColor(CO2Delta, true);
		} else if (AnimationClock(clock.count) > flashDelay + flashTime / 2f) {
			if (!scoreUpdated) {
				UpdateScore();
				ShowDeltas(true);
				scoreUpdated = true;
				signalSentToC2LightSystem = true;
			}
			// THIS NEEDS FIXING, IT IS NOT PROPERLY IMPLEMENTED (FLASHTIME NEEDS A CLOCK.RESTART TO BE TRUE, ETC) ***************************** BUG!
			moneyTextScore.color = ColorFader(scoresCD, true, flashTime, flashDelay);
			moneyTextDelta.color = ColorFader(GetDeltaColor(moneyDelta, false), true, flashTime, flashDelay);
	
			bonusTextScore.color = ColorFader(scoresCD, true, flashTime, flashDelay);
			bonusTextDelta.color = ColorFader(GetDeltaColor(bonusDelta, false), true, flashTime, flashDelay);
	
			CO2TextScore.color = ColorFader(scoresCD, true, flashTime, flashDelay);
			CO2TextDelta.color = ColorFader(GetDeltaColor(CO2Delta, true), true, flashTime, flashDelay);			

		} else if (AnimationClock(clock.count) > flashDelay) {
			moneyTextScore.color = ColorFader(scoresCD, false, flashTime / 2f, flashDelay);
			moneyTextDelta.color = ColorFader(MoneyDeltaCD, false, flashTime / 2f, flashDelay);
	
			bonusTextScore.color = ColorFader(scoresCD, false, flashTime / 2f, flashDelay);
			bonusTextDelta.color = ColorFader(BonusDeltaCD, false, flashTime / 2f, flashDelay);
	
			CO2TextScore.color = ColorFader(scoresCD, false, flashTime / 2f, flashDelay);
			CO2TextDelta.color = ColorFader(CO2DeltaCD, false, flashTime / 2f, flashDelay);
		} else {
			moneyTextScore.color = bonusTextScore.color = moneyTextScore.color = scoresCD;
		}
	}

	public Color deltaRed, deltaGreen, deltaNeutral;

	Color GetDeltaColor(int score, bool inverted) {
		if (score == 0) {
			return deltaNeutral;
		} else if (score > 0) {
			if (inverted) {
				return deltaRed;
			} else {
				return deltaGreen;
			}
		} else {
			if (inverted) {
				return deltaGreen;
			} else {
				return deltaRed;
			}
		}
	}

	IEnumerator TransitionFeedbackToNextChallenge() {
		float xOffset = 1300f;
		float answerTimeToGetOut = 0.5f;
		float answerDelayToGetOut = 0.2f;
		float proButtonOffset = xOffset;

		float yOffset = 200f;
		float upperFrameTimeToHide = 1f;
		float upperFrameDelayToHide = 0f;

		float deltasTimeToHide = 1f;
		float deltasDelayToHide = 0f;

		float feedbackTextTimeToHide = deltasTimeToHide;
		float feedbackTextDelayToHide = deltasDelayToHide;
		
		float feedbackTextOffset = 50f;
		float feedbackTimeToLock = 0.3f;
		float feedbackDelay = 0f;

		while (true) {
			switch (selectedAnswer) {
				case 1:
					answer1.anchoredPosition3D = posLerper(answerTimeToGetOut, answerDelayToGetOut, ans1CDPos, new Vector3(xOffset, ans1CDPos.y, ans1CDPos.z));
					proButtonOffset = -xOffset;
					break;
				case 2:
					answer2.anchoredPosition3D = posLerper(answerTimeToGetOut, answerDelayToGetOut, ans2CDPos, new Vector3(-xOffset, ans2CDPos.y, ans2CDPos.z));
					proButtonOffset = xOffset;
					break;
				case 3:
					answer3.anchoredPosition3D = posLerper(answerTimeToGetOut, answerDelayToGetOut, ans3CDPos, new Vector3(xOffset, ans3CDPos.y, ans3CDPos.z));
					proButtonOffset = -xOffset;
					break;
				case 4:
					answer4.anchoredPosition3D = posLerper(answerTimeToGetOut, answerDelayToGetOut, ans4CDPos, new Vector3(-xOffset, ans4CDPos.y, ans4CDPos.z));
					proButtonOffset = xOffset;
					break;
			}
			progressButton.anchoredPosition3D = posLerper(answerTimeToGetOut, answerDelayToGetOut, progressButtonFeedbackDPos, new Vector3(proButtonOffset, progressButtonFeedbackDPos.y, progressButtonFeedbackDPos.z));
			upperFrame.anchoredPosition3D = posLerper(upperFrameTimeToHide, upperFrameDelayToHide, upperFramePlayDPos, new Vector3(0, yOffset, upperFramePlayDPos.z));
		
			moneyTextDelta.color = ColorFader(MoneyDeltaCD, false, deltasTimeToHide, deltasDelayToHide);
			bonusTextDelta.color = ColorFader(BonusDeltaCD, false, deltasTimeToHide, deltasDelayToHide);
			CO2TextDelta.color = ColorFader(CO2DeltaCD, false, deltasTimeToHide, deltasDelayToHide);		

			feedbackText.color = ColorFader(feedbackTextCD, false, feedbackTextTimeToHide, feedbackTextDelayToHide);
			feedbackTitleField.anchoredPosition3D = posLerper(feedbackTimeToLock, feedbackDelay * 1.2f, feedbackTitleDPos, new Vector3(-feedbackTextOffset, feedbackTitleDPos.y, feedbackTitleDPos.z));
			leftBracketField.anchoredPosition3D  = posLerper(feedbackTimeToLock, feedbackDelay, leftBracketDPos, new Vector3(-feedbackTextOffset, leftBracketDPos.y, leftBracketDPos.z));
			rightBracketField.anchoredPosition3D  = posLerper(feedbackTimeToLock, feedbackDelay, rightBracketDPos, new Vector3(feedbackTextOffset, rightBracketDPos.y, rightBracketDPos.z));

			yield return new WaitForSeconds(transitionStep);
		}
	}

	IEnumerator TransitionContinueGame() {
		while (true) {
			IntroduceAnswersAndDescription();
			yield return new WaitForSeconds(transitionStep);
		}
	}

	float IntroduceAnswersAndDescription() {
			float yOffset = 200f;
			float timeToPos = 0.5f;
	
			Vector3 upperFrameAnchor = properties.GetUIElementAnchor(Properties.UIElement.upperFrame, UIState.playMode);
			upperFrame.anchoredPosition3D = posLerper(timeToPos, 1f, new Vector3(0, yOffset, upperFrameAnchor.z), upperFrameAnchor);
			answer1.anchoredPosition3D = posLerper(timeToPos, 0.75f, new Vector3(0, yOffset, ans1CDPos.z), ans1CDPos);
			answer2.anchoredPosition3D = posLerper(timeToPos, 0.5f, new Vector3(0, yOffset, ans2CDPos.z), ans2CDPos);
			answer3.anchoredPosition3D = posLerper(timeToPos, 0.25f, new Vector3(0, yOffset, ans3CDPos.z), ans3CDPos);
			answer4.anchoredPosition3D = posLerper(timeToPos, 0f, new Vector3(0, yOffset, ans4CDPos.z), ans4CDPos);

			float timeToRot = 1f;
			float rotDelay = 0.75f;
			answer1.localRotation = rotLerper(timeToRot, rotDelay, Quaternion.identity, ans1CDRot);
			answer2.localRotation = rotLerper(timeToRot, rotDelay, Quaternion.identity, ans2CDRot);
			answer3.localRotation = rotLerper(timeToRot, rotDelay, Quaternion.identity, ans3CDRot);
			answer4.localRotation = rotLerper(timeToRot, rotDelay, Quaternion.identity, ans4CDRot);

			float timeToColor = 0.5f;
			float colorDelay = 0.5f;
			ans1Text.color = ColorFader(answersTextCD, true, timeToColor, colorDelay);
			ans2Text.color = ColorFader(answersTextCD, true, timeToColor, colorDelay);
			ans3Text.color = ColorFader(answersTextCD, true, timeToColor, colorDelay);
			ans4Text.color = ColorFader(answersTextCD, true, timeToColor, colorDelay);
			
			return 2f;
	}

	public RectTransform finalScorePanel;
	public Text finalScoreText1, finalScoreText2, finalScoreActual, finalScoreMax;
	public Text moneyTotalText, moneyTotalNo, bonusTotalText, bonusTotalNo, CO2TotalText, CO2TotalNo;
	public Text bestText, lastText, bestNo, lastNo;
	public Color dColorFinalScoreText, dColorFinalScoreNumbers;

	IEnumerator TransitionScoreEnter() {
		
		Vector3 upperFrameAnchor = properties.GetUIElementAnchor(Properties.UIElement.upperFrame, UIState.introMode);
		
		while (true) {
			upperFrame.anchoredPosition3D = posLerper(0.5f, 0f, new Vector3(upperFrameAnchor.x, 200f, upperFrameAnchor.z), upperFrameAnchor);
			progressButton.anchoredPosition3D = posLerper(0.5f, 2f, new Vector3(1300f, progressButtonFeedbackDPos.y, progressButtonFeedbackDPos.z), progressButtonFeedbackDPos);
			FinalScoresFade(true, 0.7f, 2f);
			yield return new WaitForSeconds(transitionStep);
		}
	}

	IEnumerator TransitionScoreExit() {
		
		Vector3 upperFrameAnchor = properties.GetUIElementAnchor(Properties.UIElement.upperFrame, UIState.introMode);

		while (true) {
			upperFrame.anchoredPosition3D = posLerper(0.5f, 0f, upperFrameAnchor, new Vector3(upperFrameAnchor.x, 200f, upperFrameAnchor.z));
			progressButton.anchoredPosition3D = posLerper(0.5f, 2f, progressButtonFeedbackDPos, new Vector3(1300f, progressButtonFeedbackDPos.y, progressButtonFeedbackDPos.z));
			FinalScoresFade(false, 0.7f, 2f);
			yield return new WaitForSeconds(transitionStep);
		}
	}

	void FinalScoresFade(bool fadeIn, float timeToFade, float fadeDelay) {

		finalScoreText1.color = ColorFader(dColorFinalScoreText, fadeIn, timeToFade, fadeDelay * 0.07f);
		finalScoreActual.color = ColorFader(dColorFinalScoreNumbers, fadeIn, timeToFade, fadeDelay * 0.14f);
		finalScoreText2.color = ColorFader(dColorFinalScoreText, fadeIn, timeToFade, fadeDelay * 0.21f);
		finalScoreMax.color = ColorFader(dColorFinalScoreNumbers, fadeIn, timeToFade, fadeDelay * 0.29f);
	
		moneyTotalText.color = ColorFader(dColorFinalScoreText, fadeIn, timeToFade, fadeDelay * 0.36f);
		moneyTotalNo.color = ColorFader(GetDeltaColor(moneyScore, false), fadeIn, timeToFade, fadeDelay * 0.43f);
		bonusTotalText.color = ColorFader(dColorFinalScoreText, fadeIn, timeToFade, fadeDelay * 0.5f);
		bonusTotalNo.color = ColorFader(GetDeltaColor(bonusScore, false), fadeIn, timeToFade, fadeDelay * 0.57f);
		CO2TotalText.color = ColorFader(dColorFinalScoreText, fadeIn, timeToFade, fadeDelay * 0.64f);
		CO2TotalNo.color = ColorFader(GetDeltaColor(CO2Score, true), fadeIn, timeToFade, fadeDelay * 0.71f);

		lastText.color = ColorFader(dColorFinalScoreText, fadeIn, timeToFade, fadeDelay * 0.79f);
		lastNo.color = ColorFader(dColorFinalScoreNumbers, fadeIn, timeToFade, fadeDelay * 0.86f);
		bestText.color = ColorFader(dColorFinalScoreText, fadeIn, timeToFade, fadeDelay * 0.93f);
		bestNo.color = ColorFader(dColorFinalScoreNumbers, fadeIn, timeToFade, fadeDelay * 1f);
		
	}

	// -------------------------------------------------------------- ANIMATION / MOVERS / GENERAL

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

//	Color ColorTurn(Color baseColor, Color targetColor, bool targetIsBaseColor, int oscillations, float timeToLock, float startDelay) {
//		Color initial, target;
//		float speed;
//
//		if (targetIsBaseColor) {
//			speed = 1f * oscillations;
//		} else {
//			speed = 
//		}
//
//		if (AnimationClock(clock.count) > timeToLock + startDelay) {
//			return targetColor;
//		} else if (AnimationClock(clock.count) >= startDelay) {
//			return Color.Lerp(initial, target, (Mathf.Sin((AnimationClock(clock.count) - startDelay) * speed / timeToLock) * Mathf.PI/2f));	
//		} else {
//			return baseColor;
//		}
//	}


	Vector3 posLerper(float timeToLock, float startDelay, Vector3 pointA, Vector3 pointB) {
		if (AnimationClock(clock.count) > timeToLock + startDelay) {
			return pointB;
		} else if (AnimationClock(clock.count) >= startDelay) {
			return Vector3.Lerp(pointA, pointB, (AnimationClock(clock.count) - startDelay) / timeToLock);	
		} else {
			return pointA;
		}
	}

	Quaternion rotLerper(float timeToLock, float startDelay, Quaternion rotA, Quaternion rotB) {
		if (AnimationClock(clock.count) > timeToLock + startDelay) {
			return rotB;
		} else if (AnimationClock(clock.count) >= startDelay) {
			return Quaternion.Lerp(rotA, rotB, (AnimationClock(clock.count) - startDelay) / timeToLock);	
		} else {
			return rotA;
		}
	}

	void FadeInAnswersText(float fadeTime, float targetAlpha) {

		ans1Text.CrossFadeAlpha(targetAlpha, fadeTime, true);
		return;
	}

	// -------------------------------------------------------------- ANIMATION / MOVERS / SPECIFIC

	enum movDir {enter = 0, exit = 1}

//	bool MoveAnswersPanel(movDir dir, float timeToLock, float startDelay, float clock) {
//		Vector2 initialPos, finalPos;
//
//		if (dir == movDir.enter) {
//			initialPos = new Vector2(0, 0);
//			finalPos = defaultAnswersPanelAnchor;
//		} else {
//			initialPos = defaultAnswersPanelAnchor;
//			finalPos = new Vector2(0, 0);
//		}
//
//		if (clock == 0) {
//			answersPanel.anchoredPosition = initialPos;
//		} else if (clock > timeToLock + startDelay) {
//			answersPanel.anchoredPosition = finalPos;
//			return true;
//		} else if (clock >= startDelay) {
//			answersPanel.anchoredPosition = Vector2.Lerp(initialPos, finalPos, (clock - startDelay) / timeToLock);	
//		}
//		return false;
//	}

//	bool RotateInformationFields(movDir dir, float timeToLock, float startDelay, float clock) {
//		Quaternion moneyFieldInitialRot, moneyFieldFinalRot;
//		Quaternion bonusFieldInitialRot, bonusFieldFinalRot;
//
//		if (dir == movDir.enter) {
//			moneyFieldInitialRot = Quaternion.Euler(new Vector3(0, -140f, 0f));
//			moneyFieldFinalRot = defaultMoneyFieldRotation;
//	
//			bonusFieldInitialRot = Quaternion.Euler(new Vector3(0, 140f, 0f));;
//			bonusFieldFinalRot = defaultBonusFieldRotation;
//		} else {
//			moneyFieldInitialRot = defaultMoneyFieldRotation;
//			moneyFieldFinalRot = Quaternion.Euler(new Vector3(0, -140f, 0f));
//	
//			bonusFieldInitialRot = defaultBonusFieldRotation;
//			bonusFieldFinalRot = Quaternion.Euler(new Vector3(0, 140f, 0f));;
//		}
//		
//		if (clock == 0) {
//			moneyField.localRotation = moneyFieldInitialRot;
//			bonusField.localRotation = bonusFieldInitialRot;
//		} else if (clock > timeToLock + startDelay) {
//			moneyField.localRotation = moneyFieldFinalRot;
//			bonusField.localRotation = bonusFieldFinalRot;
//			return true;
//		} else if (clock >= startDelay) {
//			moneyField.localRotation = Quaternion.Lerp(moneyFieldInitialRot, moneyFieldFinalRot, (clock - startDelay) / timeToLock);
//			bonusField.localRotation = Quaternion.Lerp(bonusFieldInitialRot, bonusFieldFinalRot, (clock - startDelay) / timeToLock);
//		}
//		return false;
//	}
}
