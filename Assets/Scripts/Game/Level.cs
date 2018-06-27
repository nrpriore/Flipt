using UnityEngine;					// For access to Vector2
using System.Collections.Generic;	// For dictionaries
using Newtonsoft.Json.Linq;			// For access to JObject

// Holds the level information and methods
public class Level {

	// Static reference to the selected level so gameobjects can easily communicate
	public static Level CurrentLevel = null;


	/// Database Level information -------------------------------------------------
	// ID of level in levelpack
	private int _id;
	public int ID {
		get{return _id;}
	}
	// Width of level in Unity units (Generally 5-10)
	private int _width;
	public int Width {
		get{return _width;}
	}
	// Tile pattern in easily savable string format
	// (+/-) = tile starts on/off, ID, (comma): "4,-8,-9,-10,12,16"
	// ID = 1 is bottom left and reads from left to right then up
	private string _tileMap;
	public string TileMap {
		get{return _tileMap;}
	}
	// Score tier based on specified # of turns taken. [0] gold, [1] silver, [2] bronze
	/*private int[] _tierReq;
	public int[] TierReq {
		get{return _tierReq;}
	}*/


	/// In-Game Level information --------------------------------------------------
	// List of all tiles on map
	private List<Tile> _tiles;
	public List<Tile> Tiles {
		get{return _tiles;}
	}
	// Returns tile with given ID
	private Dictionary<int, Tile> _tileByID;
	public Dictionary<int, Tile> TileByID {
		get{return _tileByID;}
	}
	// Returns list of IDs paired with given tile ID
	private Dictionary<int, List<int>> _pairedTiles;
	public Dictionary<int, List<int>> PairedTiles {
		get{return _pairedTiles;}
	}
	// Returns whether the level is in progress with changes (i.e. a player has flipped a tile already)
	// Used mainly for determining confirm screen for next/prev/menu
	private bool _modified;
	public bool Modified {
		get{return _modified;}
	}


	/// Public methods -------------------------------------------------------------
	// Play this level
	public void Play() {
		Build();
		_modified = false;
	}
	
	// Maps JObject data to Level
	public void LoadFrom(JObject jo) {
		_id = jo["id"].Value<int>();
		_width = jo["width"].Value<int>();
		_tileMap = jo["tile_map"].Value<string>();
	}

	// Gets vector2 position for tile with given ID based on level.Width
	// Tiles are 1x1 unit with (0,0) starting bottom left
	public Vector2 GetTilePosition(int id) {
		int x = id % _width;
		int y = id / _width;
		return new Vector2(x, y);
	}

	// Flips paired tiles for input tile id
	public void FlipPairedTiles(int id) {
		_tileByID[id].FlipTile();
		List<int> pairedIDs = _pairedTiles[id];
		foreach(int pairedID in pairedIDs) {
			_tileByID[pairedID].FlipTile();
		}
		_modified = true;

		CheckWinCondition();
	}


	/// Private methods ------------------------------------------------------------
	// Checks when condition after flipping tiles
	private void CheckWinCondition() {

	}

	// Build input Level
	private void Build() {
		// Adjust camera based on level.Width
		CameraUtil.SizeToWidth(_width);

		// Create tiles
		_tiles = new List<Tile>();
		_tileByID = new Dictionary<int, Tile>();
		Transform tileContainer = new GameObject().transform;
		tileContainer.name = "Tiles";
		string[] tiles = _tileMap.Split(',');
		foreach(string curTile in tiles) {
			int id = int.Parse(curTile);
			bool on = id > 0;
			id = Mathf.Abs(id) - 1;
			Tile tile = GameObject.Instantiate<GameObject>(Tile.Prefab, tileContainer).GetComponent<Tile>();
			tile.name = tile.name.Substring(0, tile.name.Length - 7);
			tile.transform.localPosition = GetTilePosition(id);
			tile.Set(id, on);

			_tiles.Add(tile);
			_tileByID[id] = tile;
		}

		// Pair tiles
		_pairedTiles = new Dictionary<int, List<int>>();
		foreach(Tile tile in _tiles) {
			List<int> pairedTiles = new List<int>();
			// Check up
			int i = 1;
			while(_tileByID.ContainsKey(tile.ID + (i * _width))) {
				pairedTiles.Add(tile.ID + (i * _width));
				i++;
			}
			// Check left
			i = 1;
			while(_tileByID.ContainsKey(tile.ID - i) && (tile.ID - i) / _width == tile.ID / _width) {
				pairedTiles.Add(tile.ID - i);
				i++;
			}
			// Check down
			i = 1;
			while(_tileByID.ContainsKey(tile.ID - (i * _width))) {
				pairedTiles.Add(tile.ID - (i * _width));
				i++;
			}
			// Check right
			i = 1;
			while(_tileByID.ContainsKey(tile.ID + i) && (tile.ID + i) / _width == tile.ID / _width) {
				pairedTiles.Add(tile.ID + i);
				i++;
			}
			// Set dictionary
			if(pairedTiles.Count > 0) {
				_pairedTiles[tile.ID] = pairedTiles;
			}
		}
	}


	/// Static levels --------------------------------------------------------------
	public static Level TestLevel() {
		Level level = new Level();
		level._width = 4;
		level._tileMap = "5,9,10,-11,13,15,-17,18,19,20,21";
		return level;
	}
	public static Level MenuLevel() {
		Level level = new Level();
		level._width = 6;
		level._tileMap = "-1,-2,-3,-4,-5,-6,-7,-9,-10,-12,-13,-15,-16,-17,-18,-19,-20,-21,-23,-26,-29,-32,-33,-34,-35";
		return level;
	}

}
