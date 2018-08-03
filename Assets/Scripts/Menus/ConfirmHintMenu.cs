//using UnityEngine;					// For RectTransform class

// Governs functionality of the ConfirmHintMenu gameobject
public class ConfirmHintMenu : Modal {

	// Runs when ConfirmHintMenu gameobject is in loaded scene
	public override void Start () {
		BackgroundInteractable = false;
		base.Start();
	}

}
