//using UnityEngine;					// For RectTransform class

// Governs functionality of the SettingsMenu gameobject
public class SettingsMenu : Modal {

	// Runs when SettingsMenu gameobject is in loaded scene
	public override void Start () {
		BackgroundInteractable = true;
		base.Start();
	}
	
}
