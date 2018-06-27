using UnityEngine;					// To inherit from Monobehaviour
using UnityEngine.SceneManagement;	// To change scenes

// Main class for LevelSelect scene
public class LevelSelectController : MonoBehaviour {

	public static LevelPack CurrentLevelPack;

	private const float LP_SPACING = 20f;

	private RectTransform _scrollContainer;
	//private LevelSelectScroll _scroll;

	// Runs when LevelSelect scene is loaded
	void Start () {
		_scrollContainer = GameObject.Find("LevelPacks").GetComponent<RectTransform>();

		CurrentLevelPack = null;
		Level.CurrentLevel = null;
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

			packRT.gameObject.GetComponent<LevelPack>().SetData(StaticLevelData.LevelPacks[i]);
		}
		_scrollContainer.sizeDelta = new Vector2(_scrollContainer.sizeDelta.x, scrollContSize);
	}

}
