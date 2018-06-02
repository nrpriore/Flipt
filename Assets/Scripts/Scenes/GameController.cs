using UnityEngine;					// To inherit from Monobehaviour

// Main class for Game scene
public class GameController : MonoBehaviour {

	// Runs when Game scene is loaded
	void Start () {
		if(Level.CurrentLevel == null) {
			Level.CurrentLevel = Level.TestLevel();
		}
		Level.CurrentLevel.Build();
	}	

}
