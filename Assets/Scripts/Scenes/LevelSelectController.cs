using UnityEngine;					// To inherit from Monobehaviour
using UnityEngine.UI;				// For Text class
using UnityEngine.SceneManagement;	// To change scenes

// Main class for LevelSelect scene
public class LevelSelectController : MonoBehaviour {

	public static LevelPack CurrentLevelPack;

	private const float LP_SPACING = 20f;

	private RectTransform _scrollContainer;

	// Runs when LevelSelect scene is loaded
	void Start () {
		_scrollContainer = GameObject.Find("LevelPacks").GetComponent<RectTransform>();

		GameController.Main = null;
		CurrentLevelPack = null;
		Level.CurrentLevel = null;

		SetTotalTrophies();
		BuildLevelPacks();
	}


	/// Public methods -------------------------------------------------------------
	// Opens Start scene
	public void GoToStart() {
		SceneManager.LoadScene("_Start", LoadSceneMode.Single);
	}


	/// Private methods ------------------------------------------------------------
	// Create level packs from StaticLevelData
	private void BuildLevelPacks() {
		float scrollContSize = 0;
		for(int i = 0; i < StaticLevelData.LevelPacks.Length; i++) {
			RectTransform packRT = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/LevelSelect/LevelPack"), _scrollContainer).GetComponent<RectTransform>();
			packRT.anchoredPosition = new Vector2(0, -LP_SPACING - (i * (LP_SPACING + packRT.sizeDelta.y)));
			scrollContSize = LP_SPACING - packRT.anchoredPosition.y + packRT.sizeDelta.y;

			packRT.gameObject.GetComponent<LevelPack>().SetUIData(StaticLevelData.LevelPacks[i]);
		}
		_scrollContainer.sizeDelta = new Vector2(_scrollContainer.sizeDelta.x, scrollContSize);
	}

	// Counts total trophies and sets UI value
	private void SetTotalTrophies() {
		int total = 0;
		foreach(LevelPackData lp in StaticLevelData.LevelPacks) {
			int lptotal = 0;
			foreach(Level l in lp.Levels) {
				lptotal += l.Tier;
			}
			total += lptotal;
		}

		GameObject.Find("TotalTrophies").transform.Find("Count").GetComponent<Text>().text = total.ToString();
	}

}
