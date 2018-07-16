using UnityEngine;					// To inherit from Monobehaviour
using UnityEngine.EventSystems;		// OnClick

// Holds tile information and methods
public class Tile : MonoBehaviour, IPointerClickHandler {

	// Returns the tile resource for instantiation
	public static GameObject Prefab {
		get {return Resources.Load<GameObject>("Prefabs/Game/Tile");}
	}
	// Returns Color to paint 'on' tiles
	public static Color ColorON {
		get {return ColorUtil.HexToColor("FFFFFF");}
	}
	// Returns Color to paint 'off' tiles
	public static Color ColorOFF {
		get {return ColorUtil.HexToColor("1F325B");}
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
		gameObject.GetComponent<SpriteRenderer>().color = (_on)? ColorON : ColorOFF;
	}

	// Runs when mouse click on tile
	public void OnPointerClick(PointerEventData data) {
		if(Level.CurrentLevel != null) {
			Level.CurrentLevel.ClickTile(_id);
		}  // Maybe add some error handling if there are somehow tiles without a CurrentLevel set
	}

	// Flips this tile in-game
	public void FlipTile() {
		_on = !_on;
		gameObject.GetComponent<SpriteRenderer>().color = (_on)? ColorON : ColorOFF;
	}

}
