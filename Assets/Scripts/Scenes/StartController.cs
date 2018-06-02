using UnityEngine;					// To inherit from Monobehaviour
using UnityEngine.SceneManagement;	// To change scenes

// Main class for Start scene
public class StartController : MonoBehaviour {

	// Runs FIRST when Start scene is loaded (app startup)
	void Awake () {
		Level.CurrentLevel = Level.MenuLevel();
		Level.CurrentLevel.Build();
	}


	/// Public methods -------------------------------------------------------------
	// Opens LevelSelect scene
	public void GoToLevelSelect() {
		SceneManager.LoadScene("LevelSelect", LoadSceneMode.Single);
	}
	
}
