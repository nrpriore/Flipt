using UnityEngine;					// To inherit from Monobehaviour

// Governs a single level pack on the levelselect scene
public class LevelPack : MonoBehaviour {

	/// Database Level information -------------------------------------------------
	// Name of the levelpack
	private string _name;
	public string Name {
		get{return _name;}
	}
	// Is the levelpack unlocked
	private bool _unlocked;
	public bool Unlocked {
		get{return _unlocked;}
	}
	// Level progress for levelpack
	private int _progress;
	public int Progress {
		get{return _progress;}
	}

}
