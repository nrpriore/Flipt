//using UnityEngine;					// For RectTransform class

// Governs functionality of the ShopMenu gameobject
public class ShopMenu : Modal {

	// Runs when ShopMenu gameobject is in loaded scene
	public override void Start () {
		BackgroundInteractable = true;
		base.Start();
	}
	
}
