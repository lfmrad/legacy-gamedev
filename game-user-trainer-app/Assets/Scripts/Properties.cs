using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class Properties : MonoBehaviour {

	public int numberOfChallenges = 6;
	public int numberOfAnswers = 4;
	public language gameLanguage = language.en;

	// ------------------------------- GETTERS

	// Text
	public string GetChallengeText(int challenge, textSection textSection) {
		return challengeTextSections[challenge-1, (int)textSection];
	}
	public string GetMiscellanyText(miscellanyTextTypes type) {
		return miscellanyText[(int)type];
	}
	public string GetIntroText(int page) {
		return introTextPages[page-1];
	}
	public string GetSpecialText(int index) {
		return specialText[index];
	}

	string GetLanguageAbbreviation(language selectedLanguage) {
		// Lang. abbreviation to use when declaring /path to language's files
		switch(selectedLanguage) {
			case language.en:
				return "EN";
				break;
			case language.fr:
				return "FR";
				break;
			case language.es:
				return "ES";
				break;
			default:
				return "EN";
				break;
		}
	}

	// Images
	public Sprite GetAnswerImage(int challenge, int answer) {
		return answersImages[challenge-1,answer-1];
	}
	public Sprite GetChallengeImage(int challenge) {
		return challengeContextualImages[challenge-1];
	}
	public Vector3 GetChallengeImageAnchor(int challenge) {
		return challengeImagesPosition[challenge];
	}
	public float GetChallengeImageHeight(int challenge) {
		return challengeImagesHeight[challenge];
	}

	public Vector3 GetUIElementAnchor(UIElement element,  GameController.UIState uiPreset) {
		return UIElementAnchor[(int)element, (int)uiPreset];
	}

	public Quaternion GetUIElemenRotation(UIElement element,  GameController.UIState uiPreset) {
		return UIElementRotation[(int)element, (int)uiPreset];
	}

	// Answers' Layout
	public Vector3 GetChallengeAnswerAnchor(int challenge, int answer) {
		return anchor3D[challenge, answer];
	}

	public float GetChallengeAnswerSizeY(int challenge, int answer) {
		return sizeY[challenge, answer];
	}

	public Quaternion GetChallengeAnswerRotation(int challenge, int answer) {
		return rotation[challenge, answer];
	}


	// Scores
	public int GetChallengeBonus(int challenge, int answer) {
		return challengeBonus[challenge, answer];
	}

	public int GetChallengeMoney(int challenge, int answer) {
		return challengeMoney[challenge, answer];
	}

	public int GetChallengeCO2(int challenge, int answer) {
		return challengeCO2[challenge, answer];
	}

	// ------------------------------- HARD-CODED DATA

	// TO REFINE (20160211):
	// AnswersLayouts, Bonus, Money & CO2 are defined from index 1 onwards; instead of from index 0, as in GetChallenge.
	// I left it like that so I hadn't had to reassing everything from 0 in the Seters.
	// These differences are compensated on the Get functions.

	Vector3[,] UIElementAnchor;
	Quaternion[,] UIElementRotation;

	public enum UIElement {
		upperFrame = 0,
		upperBody = 1,
		superTop = 2,
		
		leftArrowButton = 2,
		rightArrowButton = 3,
		progressButton = 4,

		rightBracket = 6,
		leftBracket = 7,
		feedbackField = 8,
		feedbackTitle = 9,
		earth = 10,
		bonusField = 11,
		moneyField = 12
	}
	int numberOfElements = System.Enum.GetValues(typeof(UIElement)).Length;
	int numberOfUIStates = System.Enum.GetValues(typeof(GameController.UIState)).Length;

	public void SetUILockedPositions() {
		UIElementAnchor = new Vector3[numberOfElements, numberOfUIStates];
		UIElementRotation = new Quaternion[numberOfElements, numberOfUIStates];

		UIElementAnchor[(int)UIElement.upperFrame, (int)GameController.UIState.playMode] = new Vector3(0f, -19.5f, 0f);
		UIElementAnchor[(int)UIElement.upperFrame, (int)GameController.UIState.introMode] = new Vector3(0f, 8.6f, 0f);

		UIElementAnchor[(int)UIElement.upperBody, (int)GameController.UIState.playMode] = new Vector3(0f, 252.36f, 387f);
		UIElementAnchor[(int)UIElement.upperBody, (int)GameController.UIState.introMode] = new Vector3(0f, 121.11f, 387f);

		UIElementAnchor[(int)UIElement.superTop, (int)GameController.UIState.playMode] = new Vector3(0f, -27f, 0f);
		UIElementAnchor[(int)UIElement.superTop, (int)GameController.UIState.introMode] = new Vector3(0f, -27f, 0f);

		UIElementAnchor[(int)UIElement.leftArrowButton, (int)GameController.UIState.introMode] = new Vector3(30f, 0f, 0f);
		UIElementAnchor[(int)UIElement.rightArrowButton, (int)GameController.UIState.introMode] = new Vector3(-30f, 0f, 0f);

		UIElementAnchor[(int)UIElement.progressButton, (int)GameController.UIState.introMode] = new Vector3(0f, 41.21f, 0f);
		UIElementAnchor[(int)UIElement.progressButton, (int)GameController.UIState.feedbackMode] = new Vector3(0f, 107.2f, 0f);
		UIElementRotation[(int)UIElement.progressButton, (int)GameController.UIState.introMode] = Quaternion.Euler(new Vector3(30f, 0f, 0f));
		UIElementRotation[(int)UIElement.progressButton, (int)GameController.UIState.feedbackMode] = Quaternion.Euler(new Vector3(17.53f, 0f, 0f));

		UIElementAnchor[(int)UIElement.rightBracket, (int)GameController.UIState.deltaScoreMode] = new Vector3(-188f, -44.065f, 0f);
		UIElementAnchor[(int)UIElement.leftBracket, (int)GameController.UIState.deltaScoreMode] = new Vector3(188f, -44.065f, 0f);
		UIElementAnchor[(int)UIElement.feedbackTitle, (int)GameController.UIState.deltaScoreMode] = new Vector3(143.21f, -23.22f, 0f);

		UIElementAnchor[(int)UIElement.rightBracket, (int)GameController.UIState.feedbackMode] = new Vector3(-125.8f, -44.065f, 0f);
		UIElementAnchor[(int)UIElement.leftBracket, (int)GameController.UIState.feedbackMode] = new Vector3(125.8f, -44.065f, 0f);
		UIElementAnchor[(int)UIElement.feedbackField, (int)GameController.UIState.feedbackMode] = new Vector3(0f, -27.4f, 0f);
		UIElementAnchor[(int)UIElement.feedbackTitle, (int)GameController.UIState.feedbackMode] = new Vector3(66.8f, -23.22f, 0f);
		
		UIElementAnchor[(int)UIElement.earth, (int)GameController.UIState.playMode] = new Vector3(0f, 0f, 0f);

		UIElementRotation[(int)UIElement.moneyField, (int)GameController.UIState.playMode] = Quaternion.Euler(new Vector3(31.67f, 340f, 0f));
		UIElementRotation[(int)UIElement.bonusField, (int)GameController.UIState.playMode] = Quaternion.Euler(new Vector3(31.67f, 20f, 0f));
	}


	Vector3[] challengeImagesPosition;
	float[] challengeImagesHeight;
	
	public void SetImagesLayout() {
		challengeImagesPosition = new Vector3[numberOfChallenges+2];
		challengeImagesHeight =  new float[numberOfChallenges+2];

		challengeImagesPosition[1] = new Vector3(0f, 240.9f, 0f);
		challengeImagesHeight[1] = 426.76f;
		challengeImagesPosition[2] = new Vector3(0f, 237.86f, 0f);
		challengeImagesHeight[2] = 396.62f;
		challengeImagesPosition[3] = new Vector3(-93.8f, 248.1f, 0f);
		challengeImagesHeight[3] = 495.4f;
		challengeImagesPosition[4] = new Vector3(-209.4f, 266.7f, 0f);
		challengeImagesHeight[4] = 541.2f;
		challengeImagesPosition[5] = new Vector3(-94.2f, 261.4f, 0f);
		challengeImagesHeight[5] = 531.2f;
		challengeImagesPosition[6] = new Vector3(-178.9f, 258.9f, 0f);
		challengeImagesHeight[6] = 514.1f;
		// Popup
		challengeImagesPosition[7] = new Vector3(0f, -295.2f, 0f);
		challengeImagesHeight[7] = 437.7f;
	}

	Vector3[,] anchor3D;
	float[,] sizeY;
	Quaternion[,] rotation;

	public void SetChallengeAnswersLayout() {
		anchor3D = new Vector3[numberOfChallenges+1,numberOfAnswers+1];
		sizeY = new float[numberOfChallenges+1,numberOfAnswers+1];
		rotation = new Quaternion[numberOfChallenges+1,numberOfAnswers+1];

		// Challenge 1
		anchor3D[1,1] = new Vector3(0f, 0f, 0f);
		sizeY[1,1] = 58.9f;
		rotation[1,1] = Quaternion.Euler(new Vector3(351f, 0f, 0f));

		anchor3D[1,2] = new Vector3(0f, -75, 5.18f);
		sizeY[1,2] = 73.39f;
		rotation[1,2] = Quaternion.Euler(new Vector3(357f, 0f, 0f));

		anchor3D[1,3] = new Vector3(0f, -148.4f, 5.18f);
		sizeY[1,3] = 57.37f;
		rotation[1,3] = Quaternion.Euler(new Vector3(0f, 0f, 0f));

		anchor3D[1,4] = new Vector3(0f, -213.8f, 0f);
		sizeY[1,4] = 54.3f;
		rotation[1,4] = Quaternion.Euler(new Vector3(9f, 0f, 0f));

		// Challenge 2
		anchor3D[2,1] = new Vector3(0f, 0f, 0f);
		sizeY[2,1] = 70.18f;
		rotation[2,1] = Quaternion.Euler(new Vector3(351f, 0f, 0f));

		anchor3D[2,2] = new Vector3(0f, -80.2f, 5.18f);
		sizeY[2,2] = 72.03f;
		rotation[2,2] = Quaternion.Euler(new Vector3(357f, 0f, 0f));

		anchor3D[2,3] = new Vector3(0f, -153.4f, 5.18f);
		sizeY[2,3] = 57.37f;
		rotation[2,3] = Quaternion.Euler(new Vector3(0f, 0f, 0f));

		anchor3D[2,4] = new Vector3(0f, -219.4f, 0f);
		sizeY[2,4] = 57.37f;
		rotation[2,4] = Quaternion.Euler(new Vector3(9f, 0f, 0f));

		// Challenge 3
		anchor3D[3,1] = new Vector3(0f, 0f, 0f);
		sizeY[3,1] = 57.37f;
		rotation[3,1] = Quaternion.Euler(new Vector3(351f, 0f, 0f));

		anchor3D[3,2] = new Vector3(0f, -59.9f, 5.18f);
		sizeY[3,2] = 46.8f;
		rotation[3,2] = Quaternion.Euler(new Vector3(357f, 0f, 0f));

		anchor3D[3,3] = new Vector3(0f, -123.3f, 5.18f);
		sizeY[3,3] = 63.8f;
		rotation[3,3] = Quaternion.Euler(new Vector3(0f, 0f, 0f));

		anchor3D[3,4] = new Vector3(0f, -203.1f, 0f);
		sizeY[3,4] = 79.83f;
		rotation[3,4] = Quaternion.Euler(new Vector3(9f, 0f, 0f));

		// Challenge 4
		anchor3D[4,1] = new Vector3(0f, 0f, 0f);
		sizeY[4,1] = 57.37f;
		rotation[4,1] = Quaternion.Euler(new Vector3(351f, 0f, 0f));

		anchor3D[4,2] = new Vector3(0f, -75f, 5.18f);
		sizeY[4,2] = 75.52f;
		rotation[4,2] = Quaternion.Euler(new Vector3(357f, 0f, 0f));

		anchor3D[4,3] = new Vector3(0f, -148f, 5.18f);
		sizeY[4,3] = 53.8f;
		rotation[4,3] = Quaternion.Euler(new Vector3(0f, 0f, 0f));

		anchor3D[4,4] = new Vector3(0f, -221.2f, 0f);
		sizeY[4,4] = 75f;
		rotation[4,4] = Quaternion.Euler(new Vector3(9f, 0f, 0f));

		// Challenge 5
		anchor3D[5,1] = new Vector3(0f, 9.8f, 0f);
		sizeY[5,1] = 58.8f;
		rotation[5,1] = Quaternion.Euler(new Vector3(351f, 0f, 0f));

		anchor3D[5,2] = new Vector3(0f, -59.9f, 5.18f);
		sizeY[5,2] = 62f;
		rotation[5,2] = Quaternion.Euler(new Vector3(357f, 0f, 0f));

		anchor3D[5,3] = new Vector3(0f, -132f, 6f);
		sizeY[5,3] = 62f;
		rotation[5,3] = Quaternion.Euler(new Vector3(0f, 0f, 0f));

		anchor3D[5,4] = new Vector3(0f, -203.3f, 0f);
		sizeY[5,4] = 60.3f;
		rotation[5,4] = Quaternion.Euler(new Vector3(9f, 0f, 0f));

		// Challenge 6
		// lxmn, rvyunvnwcjcrxw jwm jac kh udrb onawjwmx vjacrwni jwmand
		anchor3D[6,1] = new Vector3(0f, 0f, 0f);
		sizeY[6,1] = 57.37f;
		rotation[6,1] = Quaternion.Euler(new Vector3(351f, 0f, 0f));

		anchor3D[6,2] = new Vector3(0f, -75f, 5.18f);
		sizeY[6,2] = 77.04f;
		rotation[6,2] = Quaternion.Euler(new Vector3(357f, 0f, 0f));

		anchor3D[6,3] = new Vector3(0f, -157.5f, 5.18f);
		sizeY[6,3] = 73.7f;
		rotation[6,3] = Quaternion.Euler(new Vector3(0f, 0f, 0f));

		anchor3D[6,4] = new Vector3(0f, -231f, 0f);
		sizeY[6,4] = 57.37f;
		rotation[6,4] = Quaternion.Euler(new Vector3(9f, 0f, 0f));
	}

	int[,] challengeBonus;

	public void SetChallengeBonus() {
		challengeBonus = new int[numberOfChallenges+1, numberOfAnswers+1];

		challengeBonus[1,1] = 1;
		challengeBonus[1,2] = -2;
		challengeBonus[1,3] = 0;
		challengeBonus[1,4] = -1;

		challengeBonus[2,1] = 0;
		challengeBonus[2,2] = 2;
		challengeBonus[2,3] = 0;
		challengeBonus[2,4] = 0;

		challengeBonus[3,1] = 0;
		challengeBonus[3,2] = 0;
		challengeBonus[3,3] = 0;
		challengeBonus[3,4] = -1;

		challengeBonus[4,1] = 0;
		challengeBonus[4,2] = 0;
		challengeBonus[4,3] = 0;
		challengeBonus[4,4] = 0;

		challengeBonus[5,1] = 0;
		challengeBonus[5,2] = 0;
		challengeBonus[5,3] = 0;
		challengeBonus[5,4] = 0;

		challengeBonus[6,1] = 0;
		challengeBonus[6,2] = 0;
		challengeBonus[6,3] = 0;
		challengeBonus[6,4] = -1;
		return;
	}

	int[,] challengeMoney;

	public void SetChallengeMoney() {
		challengeMoney = new int[numberOfChallenges+1, numberOfAnswers+1];

		challengeMoney[1,1] = 0;
		challengeMoney[1,2] = 10;
		challengeMoney[1,3] = 0;
		challengeMoney[1,4] = -10;

		challengeMoney[2,1] = 0;
		challengeMoney[2,2] = 0;
		challengeMoney[2,3] = 0;
		challengeMoney[2,4] = 0;

		challengeMoney[3,1] = -50;
		challengeMoney[3,2] = -50;
		challengeMoney[3,3] = 50;
		challengeMoney[3,4] = 10;

		challengeMoney[4,1] = 30;
		challengeMoney[4,2] = 20;
		challengeMoney[4,3] = 0;
		challengeMoney[4,4] = 50;

		challengeMoney[5,1] = -30;
		challengeMoney[5,2] = 30;
		challengeMoney[5,3] = 10;
		challengeMoney[5,4] = -20;

		challengeMoney[6,1] = 30;
		challengeMoney[6,2] = 50;
		challengeMoney[6,3] = -40;
		challengeMoney[6,4] = -30;
		return;
	}

	int[,] challengeCO2;

	public void SetChallengeCO2() {
		challengeCO2 = new int[numberOfChallenges+1, numberOfAnswers+1];

		challengeCO2[1,1] = 0;
		challengeCO2[1,2] = -10;
		challengeCO2[1,3] = 0;
		challengeCO2[1,4] = 10;

		challengeCO2[2,1] = 0;
		challengeCO2[2,2] = 0;
		challengeCO2[2,3] = 0;
		challengeCO2[2,4] = 0;

		challengeCO2[3,1] = 50;
		challengeCO2[3,2] = 50;
		challengeCO2[3,3] = -50;
		challengeCO2[3,4] = -10;

		challengeCO2[4,1] = -50;
		challengeCO2[4,2] = -20;
		challengeCO2[4,3] = 50;
		challengeCO2[4,4] = -50;

		challengeCO2[5,1] = 0;
		challengeCO2[5,2] = -30;
		challengeCO2[5,3] = -20;
		challengeCO2[5,4] = 30;

		challengeCO2[6,1] = 0;
		challengeCO2[6,2] = -50;
		challengeCO2[6,3] = 30;
		challengeCO2[6,4] = 0;
		return;
	}

	// IMAGE MANAGING
	Sprite[,] answersImages;
	Sprite[] challengeContextualImages;

	public void SetGameImages(language selectedLanguage) {
		answersImages = new Sprite[numberOfChallenges, numberOfAnswers];
		challengeContextualImages = new Sprite[numberOfChallenges];

		// Answer Images
		for (int challenge = 0; challenge < numberOfChallenges; challenge++) {
			for (int answer = 0; answer < numberOfAnswers; answer++) {
				string path = GetLanguageAbbreviation(selectedLanguage)+"/C"+(challenge+1).ToString()+"-A"+(answer+1).ToString()+"-IMG";
				Sprite loadedSprite = Resources.Load<Sprite>(path);
				if (loadedSprite != null) {
					answersImages[challenge, answer] = loadedSprite;
					if(GameController.debug) {
						Debug.Log("Answer Image - Loaded for Chalenge["+(challenge+1).ToString()+"], Answer["+(answer+1).ToString()+"]");
					}
				}
			}
		}

		// Challenge Contextual Images
		for (int challenge = 0; challenge < numberOfChallenges; challenge++) {
				string path = GetLanguageAbbreviation(selectedLanguage)+"/C"+(challenge+1).ToString()+"-IMG";
				Sprite loadedSprite = Resources.Load<Sprite>(path);
				if (loadedSprite != null) {
					challengeContextualImages[challenge] = loadedSprite;
					if(GameController.debug) {
						Debug.Log("Challenge Image - Loaded for Chalenge["+(challenge+1).ToString()+"]");
					}
				} else if (GameController.debug) {
					Debug.Log("Challenge images missing!");
				}
		}
		return;
	}
	

	// TEXT MANAGING
	public enum language {
		en = 1,
		fr = 2,
		es = 3
	}

	public enum textSection {
		description = 0,  
		ans1 = 1, 
		ans2 = 2, 
		ans3 = 3, 
		ans4 = 4, 
		feedAns1 = 5, 
		feedAns2 = 6, 
		feedAns3 = 7, 
		feedAns4 = 8
	}
	int numberOfSections = System.Enum.GetValues(typeof(textSection)).Length;

	public enum miscellanyTextTypes {
		challengeWord = 0,
		showImgWord = 1,
		hideImgWord = 2,
		menuWord = 3,
		exitWord = 4,
		nextChallengeWord = 5,
		scoreWord = 6,
		reviewFeedbackWord = 7,
		skipWord = 8,
		nextPageWord = 9,
		feedbackWord = 10,
		introWord = 11,
		nubeprintWord = 12,
		startChallengesWord = 13,
		moneyPoints = 14,
		bonusPoints = 15,
		CO2Points = 16,
		increasedWord = 17,
		decreasedWord = 18,
		noChange = 19,
		byWord = 20,
		restartWord = 21,
		finalScoreWord = 22,
		yourFinalScoreIs = 23,
		outOf = 24,
		totalMoney = 25,
		totalBonus = 26,
		totalCO2 = 27,
		lastScore = 28,
		bestScore = 29,
		scoreAgain = 30,
		thisChoiceChanged = 31,
		thisChoiceDidNotChange = 32,
		nubeprintProds = 33,
		presentsWord = 34,
		MPStheChallenge = 35
	}
	int numberOfMiscellanyTextTypes = System.Enum.GetValues(typeof(miscellanyTextTypes)).Length;

	string[,] challengeTextSections;
	string[] miscellanyText;
	string[] introTextPages;
	string[] specialText;
	
	public void SetGameText(language selectedLanguage) {
		string languageAbbreviation;
		challengeTextSections = new string[numberOfChallenges, numberOfSections];
		miscellanyText = new string[numberOfMiscellanyTextTypes];
		introTextPages = new string[3];
		specialText = new string[2];
		char[] delimiters = new char[]{'\n','\r'}; // to see if it works on Android
		// Alternative candidate:
		// string delimeters = System.Environment.NewLine + "\n" + "\r"
		// delimeters.ToCharArray();

		// Challenges Text
		for (int challenge = 0; challenge < numberOfChallenges; challenge++) {
			string path = GetLanguageAbbreviation(selectedLanguage)+"/C"+(challenge+1).ToString();
			TextAsset challengeTextAsset = Resources.Load(path) as TextAsset;
			if (challengeTextAsset != null) {
				string[] challengeText = challengeTextAsset.text.Split(new string[]{"--"}, System.StringSplitOptions.None);
				
				for (int section = 0; section < challengeText.Length; section++) {
					// Better Solution:
					challengeText[section] = challengeText[section].TrimStart(delimiters);
					challengeText[section] = challengeText[section].TrimEnd(delimiters);
					
					// Legacy Overkill First Solution (for future reference)
					// Removes All Line Breaks (uses: using System.Text.RegularExpressions;)
					// challengeText[section] = Regex.Replace(challengeText[section],"\n","");
					challengeTextSections[challenge,section] = challengeText[section];	
				}
			} else if (GameController.debug) {
				Debug.Log("Challenges text files are missing!");
			}
		}

		// Miscellany Text
		TextAsset miscellanyTextAsset = Resources.Load(GetLanguageAbbreviation(selectedLanguage)+"/Miscellany") as TextAsset;
		if (miscellanyTextAsset != null) {
			miscellanyText = miscellanyTextAsset.text.Split(new string[]{"--"}, System.StringSplitOptions.None);
			for(int i = 0; i < miscellanyText.Length; i++) {
				miscellanyText[i] = miscellanyText[i].TrimStart(delimiters);
				miscellanyText[i] = miscellanyText[i].TrimEnd(delimiters);
			}
		} else if (GameController.debug) {
			Debug.Log("Miscellany text file is missing!");
		}

		// Intro Text
		TextAsset introTextAsset = Resources.Load(GetLanguageAbbreviation(selectedLanguage)+"/Intro") as TextAsset;
		if (introTextAsset != null) {
			introTextPages = introTextAsset.text.Split(new string[]{"--"}, System.StringSplitOptions.None);
			// Replace only the first \n and last \n in each page
			for (int i = 0; i < introTextPages.Length; i++) {
				introTextPages[i] = introTextPages[i].TrimStart(delimiters);
				introTextPages[i] = introTextPages[i].TrimEnd(delimiters);
			}
		} else if (GameController.debug) {
			Debug.Log("Intro text file is missing!");
		}

		// Special Text
		TextAsset specialTextAsset = Resources.Load(GetLanguageAbbreviation(selectedLanguage)+"/Special") as TextAsset;
		if (specialTextAsset != null) {
			specialText = specialTextAsset.text.Split(new string[]{"--"}, System.StringSplitOptions.None);
			for (int i = 0; i < specialText.Length; i++) {
				specialText[i] = specialText[i].TrimStart(delimiters);
				specialText[i] = specialText[i].TrimEnd(delimiters);
			}
		} else if (GameController.debug) {
			Debug.Log("Special text file is missing!");
		}
		return;
	}
}