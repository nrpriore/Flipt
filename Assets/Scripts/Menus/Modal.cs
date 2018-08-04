using UnityEngine;					// To inherit from Monobehaviour
using UnityEngine.UI;				// For button class

// Parent class for modal windows
public class Modal : MonoBehaviour {

	protected bool BackgroundInteractable;  // Sets if clicking the background closes the window
	
	private Vector2 _target;  // Target scale of _menu
	private RectTransform _menu;  // Menu UI to change scale of
	private GameObject _background;  // Background for faded look
	private Color _backgroundColor;  // Color of background


	// Runs when a scene with a modal child is loaded
	public virtual void Start () {
		_target = Vector2.zero;
		_menu = gameObject.GetComponent<RectTransform>();
		_background = gameObject.transform.parent.Find("Background").gameObject;
		if(BackgroundInteractable) {
			_background.GetComponent<Button>().onClick.AddListener(Close);
		}
	}
	
	// Runs every frame
	void Update () {
		_menu.localScale = Vector2.Lerp(_menu.localScale, _target, Time.deltaTime * 30f);
	}


	/// Public methods -------------------------------------------------------------
	// Opens modal window
	public virtual void Open() {
		_target = Vector2.one;
		_background.SetActive(true);
	}

	// Closes modal window
	public virtual void Close() {
		if(_target == Vector2.one) {
			_target = Vector2.zero;
			_background.SetActive(false);
		}
	}

	// Sets any UI data based on input, overriden by child class
	public virtual void SetData<T>(T value) {
		// We only get here if you pass in the wrong type to the child override
		Debug.Log("Conversion failed in child class");
	}
}
