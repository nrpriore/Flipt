using UnityEngine;					// To inherit from Monobehaviour
using UnityEngine.SceneManagement;	// To change scenes

// Main class for LevelSelect scene
public class LevelSelectController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}


	/// Public methods -------------------------------------------------------------
	// Opens Start scene
	public void GoToStart() {
		SceneManager.LoadScene("Start", LoadSceneMode.Single);
	}
}
