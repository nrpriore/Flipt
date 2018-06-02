using UnityEngine;					// For RectTransform class

// Governs functionality of the StatsMenu gameobject
public class StatsMenu : Modal {

	// Runs when StatsMenu gameobject is in loaded scene
	public override void Start () {
		base.Start();
		Menu = gameObject.GetComponent<RectTransform>();
	}
	
}
