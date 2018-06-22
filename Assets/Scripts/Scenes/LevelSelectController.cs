using UnityEngine;					// To inherit from Monobehaviour
using UnityEngine.SceneManagement;	// To change scenes

// Main class for LevelSelect scene
public class LevelSelectController : MonoBehaviour {

	private Transform _scrollContainer;
	//private LevelSelectScroll _scroll;

	// Runs when LevelSelect scene is loaded
	void Start () {
		_scrollContainer = GameObject.Find("LevelPacks").transform;

		LoadLevelPacks();
		LoadProgress();
	}


	/// Public methods -------------------------------------------------------------
	// Opens Start scene
	public void GoToStart() {
		SceneManager.LoadScene("_Start", LoadSceneMode.Single);
	}


	/// Private methods ------------------------------------------------------------
	// Loads level pack data from config files
	private void LoadLevelPacks() {
		int i = 1;
		while(PlayerPrefs.HasKey("P" + i + "Name")) {
			GameObject pack = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/LevelSelect/LevelPack"));
			i++;
		}
	}

	// Loads level pack progress from PlayerPrefs
	private void LoadProgress() {
		
	}
}
