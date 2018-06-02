using UnityEngine;					// For RectTransform class

// Governs functionality of the ShopMenu gameobject
public class SettingsMenu : Modal {

	// Runs when ShopMenu gameobject is in loaded scene
	public override void Start () {
		base.Start();
		Menu = gameObject.GetComponent<RectTransform>();
	}
	
}
