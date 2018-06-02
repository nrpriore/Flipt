using UnityEngine;					// To inherit from Monobehaviour

// Parent class for modal windows
public class Modal : MonoBehaviour {

	// Set this to the gameobject instance in the Start() method of child classes
	public RectTransform Menu {
		get; set;
	}
	// Target scale and background to block input touches
	private Vector2 _target;
	private GameObject _background;


	// Runs when a scene with a modal child is loaded
	public virtual void Start () {
		_target = Vector2.zero;
		_background = gameObject.transform.parent.Find("Background").gameObject;
	}
	
	// Runs every frame
	void Update () {
		Menu.localScale = Vector2.Lerp(Menu.localScale, _target, Time.deltaTime * 20f);
	}


	/// Public methods -------------------------------------------------------------
	// Opens modal window
	public void Open() {
		_target = Vector2.one;
		_background.SetActive(true);
	}

	// Closes modal window
	public void Close() {
		_target = Vector2.zero;
		_background.SetActive(false);
	}
}
