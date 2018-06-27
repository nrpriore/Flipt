using UnityEngine;					// To inherit from Monobehaviour
using UnityEngine.UI;				// To set UI properties on scene
using UnityEngine.SceneManagement;	// To change scenes

// Main class for Game scene
public class GameController : MonoBehaviour {

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

		Level.CurrentLevel.Play();
	}	


	/// Public methods -------------------------------------------------------------
	// Goes to previous level
	public void PrevLevel() {
		if(Level.CurrentLevel.ID > 0) {
			Level.CurrentLevel = LevelSelectController.CurrentLevelPack.Data.Levels[Level.CurrentLevel.ID - 1];
			SceneManager.LoadScene("Game", LoadSceneMode.Single);
		}
	}

	// Goes to previous level
	public void NextLevel() {
		if(Level.CurrentLevel.ID < 9 && LevelSelectController.CurrentLevelPack.Data.Progress > Level.CurrentLevel.ID) {
			Level.CurrentLevel = LevelSelectController.CurrentLevelPack.Data.Levels[Level.CurrentLevel.ID + 1];
			SceneManager.LoadScene("Game", LoadSceneMode.Single);
		}
	}

}
