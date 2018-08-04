using UnityEngine.UI;					// For Text class

// Governs functionality of the ConfirmMenu gameobject
public class ConfirmMenu : Modal {

	private string action;

	// Runs when ConfirmMenu gameobject is in loaded scene
	public override void Start () {
		BackgroundInteractable = true;
		base.Start();
	}

	// Runs when user confirms action
	public void ConfirmAction() {
		GameController.Main.ConfirmModalAction(action);
	}

	// Sets UI data - only takes a string
	public override void SetData<T>(T intendedAction) {
		var _action = intendedAction.ToString();
		if(_action == null) { // Conversion failed
			base.SetData(intendedAction);
			return;
		}
		action = _action;

		Text actionText = transform.Find("Text").GetComponent<Text>();
		string varText = "";
		switch(action) {
			case "prevlevel":
				varText = "previous level";
				break;
			case "nextlevel":
				varText = "next level";
				break;
			case "levelselect":
				varText = "Level Select";
				break;
			default:
				varText = "<invalid>";
				break;
		}
		actionText.text = "Level in progress. Go to " + varText + "?";
	}
	
}
