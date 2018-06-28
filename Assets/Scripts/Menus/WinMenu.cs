using UnityEngine;					// For RectTransform class
using UnityEngine.UI;				// Text class

// Governs functionality of the WinMenu gameobject
public class WinMenu : Modal {

	// Runs when WinMenu gameobject is in loaded scene
	public override void Start () {
		if(Level.CurrentLevel.ID == 9) {
			RectTransform retryRT = transform.Find("Retry").GetComponent<RectTransform>();
			retryRT.anchoredPosition = new Vector2(retryRT.anchoredPosition.x, 170);

			transform.Find("LevelSelect").gameObject.SetActive(false);

			transform.Find("NextLevel").Find("Text").GetComponent<Text>().text = "Level Select";
		}

		BackgroundInteractable = false;
		base.Start();
	}

}
