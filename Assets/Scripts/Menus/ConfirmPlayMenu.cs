//using UnityEngine;					// For RectTransform class

// Governs functionality of the ConfirmPlayMenu gameobject
public class ConfirmPlayMenu : Modal {

	// Runs when ConfirmPlayMenu gameobject is in loaded scene
	public override void Start () {
		BackgroundInteractable = false;
		base.Start();
	}
	
}
