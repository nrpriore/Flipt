using UnityEngine;					// To inherit from Monobehaviour
using UnityEngine.SceneManagement;	// To change scenes

// Main class for Start scene
public class StartController : MonoBehaviour {

	// Runs FIRST when Start scene is loaded (app startup)
	void Awake () {
		if(CryptoUtil.ValidKeys()) { // Make sure we can decrypt the game data

		} 
		
		Level.CurrentLevel = Level.MenuLevel(); // Create easter egg menu level
		Level.CurrentLevel.Play();
	}


	/// Public methods -------------------------------------------------------------
	// Opens LevelSelect scene
	public void GoToLevelSelect() {
		SceneManager.LoadScene("LevelSelect", LoadSceneMode.Single);
	}
	
}
