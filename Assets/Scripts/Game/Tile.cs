using UnityEngine;					// To inherit from Monobehaviour
using System;						// Convert
using UnityEngine.EventSystems;		// OnClick

// Holds tile information and methods
public class Tile : MonoBehaviour, IPointerClickHandler {

	// Returns the tile resource for instantiation
	public static GameObject Prefab {
		get {return Resources.Load<GameObject>("Prefabs/Game/Tile");}
	}

	/// Tile information -----------------------------------------------------------
	// ID of the token in the current level, ID = 1 starts bottom left and counts left then up
	private int _id;
	public int ID {
		get{return _id;}
	}
	// Whether the token is on or off
	private bool _on;
	public bool On {
		get{return _on;}
	}
	// Pos ID of the token in the current level, PosID = 1 starts bottom left and counts left then up
	private int _posID;
	public int PosID {
		get{return _posID;}
	}


	/// Public methods -------------------------------------------------------------
	// Sets the tile variables
	public void Set(int id, bool on, int posID) {
		_id = id;
		_on = on;
		_posID = posID;
		UpdateColor();
	}

	// Runs when mouse click on tile
	public void OnPointerClick(PointerEventData data) {
		if(Level.CurrentLevel != null) {
			Level.CurrentLevel.ClickTile(_id);
		}
	}

	// Flips this tile in-game
	public void FlipTile() {
		_on = !_on;
		UpdateColor();
	}

	public void UpdateColor() {
		gameObject.GetComponent<SpriteRenderer>().color = ColorUtil.TILE_COLOR[Convert.ToInt32(_on)];
	}
	
	// Highlights tile to show hint
	public void HighlightHint() {
		gameObject.GetComponent<SpriteRenderer>().color = ColorUtil.HINT_COLOR;
	}

}
