using UnityEngine;					// To inherit from Monobehaviour
using UnityEngine.UI;				// To set UI properties on scene
using UnityEngine.SceneManagement;	// To change scenes

// Main class for Game scene
public class GameController : MonoBehaviour {

	private Modal _confirmMenu;

	// Runs when Game scene is loaded
	void Start () {
		GameObject.Find("Name").GetComponent<Text>().text = LevelSelectController.CurrentLevelPack.Data.Name;
		GameObject.Find("CurrentLevel").GetComponent<Text>().text = "Level:  " + (Level.CurrentLevel.ID + 1) + " / 10";
		if(Level.CurrentLevel.ID == 0) {
			GameObject.Find("PrevLevel").GetComponent<Button>().interactable = false;
		}
		else if(Level.CurrentLevel.ID == 9 || LevelSelectController.CurrentLevelPack.Data.Progress == Level.CurrentLevel.ID) {
			GameObject.Find("NextLevel").GetComponent<Button>().interactable = false;
		}

		// Dev
		if(Level.CurrentLevel == null) {
			Level.CurrentLevel = Level.TestLevel();
			Level.CurrentLevel.Play();
			return;
		}
		// End dev

		_confirmMenu = GameObject.Find("ConfirmMenu").GetComponent<ConfirmMenu>();

		Level.CurrentLevel.Play();
	}	


	/// Public methods -------------------------------------------------------------
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

	// Navigates to next level
	private void GoToNextLevel() {
		Level.CurrentLevel = LevelSelectController.CurrentLevelPack.Data.Levels[Level.CurrentLevel.ID + 1];
		SceneManager.LoadScene("Game", LoadSceneMode.Single);
	}

	// Navigates to levelselect scene
	private void GoToLevelSelect() {
		SceneManager.LoadScene("LevelSelect", LoadSceneMode.Single);
	}

}
