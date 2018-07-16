using UnityEngine;					// To inherit from Monobehaviour
using UnityEngine.SceneManagement;	// To change scenes

using System.Collections.Generic;

// Main class for Start scene
public class StartController : MonoBehaviour {

	// Runs FIRST when Start scene is loaded (app startup)
	void Awake () {
		// DEV
		CryptoUtil.ResetKeys();
		// END DEV
		if(!CryptoUtil.ValidKeys()) { // Make sure we can decrypt the game data
			CryptoUtil.ResetKeys();
		}
		StaticLevelData.LoadLevelData();

		GameController.Main = null;
		LevelSelectController.CurrentLevelPack = null;
		
		Level.CurrentLevel = Level.MenuLevel(); // Create easter egg menu level
		Level.CurrentLevel.Play();

		Solve.Level(Level.CurrentLevel);

		//PerformanceTest(10000000);
	}


	/// Public methods -------------------------------------------------------------
	// Opens LevelSelect scene
	public void GoToLevelSelect() {
		SceneManager.LoadScene("LevelSelect", LoadSceneMode.Single);
	}

	private static void PerformanceTest(int count) {
		System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
		int[] array = new int[count];
		List<int> list = new List<int>();
		int temp = 0;

		time.Restart();
		for(int i = 0; i < count; i++) {
			array[i] = 1;
		}
		Debug.Log("Array for write: " + time.ElapsedMilliseconds);

		time.Restart();
		for(int i = 0; i < count; i++) {
			list.Add(1);
		}
		Debug.Log("List for write: " + time.ElapsedMilliseconds);

		temp = 0;
		time.Restart();
		for(int i = 0; i < array.Length; i++) {
			temp++;
		}
		Debug.Log("Array for read: " + time.ElapsedMilliseconds);

		temp = 0;
		time.Restart();
		for(int i = 0; i < list.Count; i++) {
			temp++;
		}
		Debug.Log("List for read: " + time.ElapsedMilliseconds);

		temp = 0;
		time.Restart();
		foreach(int i in array) {
			temp++;
		}
		Debug.Log("Array foreach read: " + time.ElapsedMilliseconds);

		temp = 0;
		time.Restart();
		foreach(int i in list) {
			temp++;
		}
		Debug.Log("List foreach read: " + time.ElapsedMilliseconds);

		temp = 0;
		time.Restart();
		for(int i = 0; i < count; i++) {
			temp = array.Length;
		}
		Debug.Log("Array length read: " + time.ElapsedMilliseconds);

		temp = 0;
		time.Restart();
		for(int i = 0; i < count; i++) {
			temp = list.Count;
		}
		Debug.Log("List count read: " + time.ElapsedMilliseconds);

		time.Stop();
	}
	
}
