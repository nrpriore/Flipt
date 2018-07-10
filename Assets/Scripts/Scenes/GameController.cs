using UnityEngine;					// To inherit from Monobehaviour
using UnityEngine.UI;				// To set UI properties on scene
using UnityEngine.SceneManagement;	// To change scenes

// Main class for Game scene
public class GameController : MonoBehaviour {

	public static GameController Main;

	private Modal _confirmMenu;
	private Modal _winMenu;
	private Text _currScore;
	private GameObject _bestScore;

	// Runs when Game scene is loaded
	void Start () {
		Main = this;

		GameObject.Find("Name").GetComponent<Text>().text = LevelSelectController.CurrentLevelPack.Data.Name;
		GameObject.Find("CurrentLevel").GetComponent<Text>().text = "Level:  " + (Level.CurrentLevel.ID + 1) + " / 10";

		_confirmMenu = GameObject.Find("ConfirmMenu").GetComponent<ConfirmMenu>();
		_winMenu = GameObject.Find("WinMenu").GetComponent<WinMenu>();
		_currScore = GameObject.Find("CurrScore").transform.Find("Text").GetComponent<Text>();
		_bestScore = GameObject.Find("BestScore");

		ConfigBestScore();
		SetButtons();
		Level.CurrentLevel.Play();

		//Solve.Level(Level.CurrentLevel);
	}	


	/// Public methods -------------------------------------------------------------
	// Updates UI with current move number
	public void UpdateMoves(int numMoves) {
		_currScore.text = numMoves.ToString();
	}

	// Runs when you complete the level(flip the last right tile)
	public void Win() {
		StaticLevelData.UpdateProgressData();
		_winMenu.Open();
	}

	// Reset or retry level
	public void RetryLevel() {
		Level.CurrentLevel.Reset();
		ConfigBestScore();
		SetButtons();
		_winMenu.Close();
	}

	// Navigates to next level
	public void GoToNextLevel() {
		if(Level.CurrentLevel.ID < 9) {
			Level.CurrentLevel = LevelSelectController.CurrentLevelPack.Data.Levels[Level.CurrentLevel.ID + 1];
			SceneManager.LoadScene("Game", LoadSceneMode.Single);
		}
		else {
			GoToLevelSelect();
		}
	}

	// Navigates to levelselect scene
	public void GoToLevelSelect() {
		SceneManager.LoadScene("LevelSelect", LoadSceneMode.Single);
	}

	// Runs when PrevLevel button is pressed
	public void PressPrevLevel() {
		if(Level.CurrentLevel.ID > 0) {
			if(!Level.CurrentLevel.Modified) {
				GoToPrevLevel();
				return;
			}
			OpenConfirmMenu("prevlevel");
		}
	}

	// Runs when NextLevel button is pressed
	public void PressNextLevel() {
		if(Level.CurrentLevel.ID < 9 && LevelSelectController.CurrentLevelPack.Data.Progress > Level.CurrentLevel.ID) {
			if(!Level.CurrentLevel.Modified) {
				GoToNextLevel();
				return;
			}
			OpenConfirmMenu("nextlevel");
		}
	}

	// Runs when LevelSelect button is pressed
	public void PressLevelSelect() {
		if(!Level.CurrentLevel.Modified) {
			GoToLevelSelect();
			return;
		}
		OpenConfirmMenu("levelselect");
	}

	// Runs when player confirms modal action
	public void ConfirmModalAction(string action) {
		switch(action) {
			case "prevlevel":
				GoToPrevLevel();
				return;
			case "nextlevel":
				GoToNextLevel();
				return;
			case "levelselect":
				GoToLevelSelect();
				return;
			default:
				_confirmMenu.Close();
				break;
		}
	}


	/// Private methods ------------------------------------------------------------
	// Opens confirmation window for _intendedAction
	private void OpenConfirmMenu(string action) {
		_confirmMenu.SetData(action);
		_confirmMenu.Open();
	}

	// Navigates to previous level
	private void GoToPrevLevel() {
		Level.CurrentLevel = LevelSelectController.CurrentLevelPack.Data.Levels[Level.CurrentLevel.ID - 1];
		SceneManager.LoadScene("Game", LoadSceneMode.Single);
	}

	// Checks prev/next button interactability
	private void SetButtons() {
		if(Level.CurrentLevel.ID != 0) {
			GameObject.Find("PrevLevel").GetComponent<Button>().interactable = true;
		}
		if(Level.CurrentLevel.ID < 9 && Level.CurrentLevel.ID < LevelSelectController.CurrentLevelPack.Data.Progress) {
			GameObject.Find("NextLevel").GetComponent<Button>().interactable = true;
		}
	}

	// Configures BestScore gameobject
	private void ConfigBestScore() {
		if(Level.CurrentLevel.BestMoves > 0) {
			_bestScore.SetActive(true);
			_bestScore.transform.Find("Text").GetComponent<Text>().text = Level.CurrentLevel.BestMoves.ToString();

			Image trophy = _bestScore.transform.Find("Trophy").GetComponent<Image>();
			//trophy.sprite = Resources.Load<Sprite>("Sprites/Trophy/Trophy" + (Level.CurrentLevel.Tier));
			trophy.color = ColorUtil.TROPHY_COLOR[Level.CurrentLevel.Tier];
		}
		else {
			_bestScore.SetActive(false);
		}
	}

}
