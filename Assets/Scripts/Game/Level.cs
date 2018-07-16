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
	// Score tier based on specified # of turns taken. [2] gold, [1] silver, [0] bronze
	private int[] _tierReq;
	public int[] TierReq {
		get{return _tierReq;}
	}


	/// In-Game Level information --------------------------------------------------
	// List of all tiles on map
	private List<Tile> _tiles;
	public List<Tile> Tiles {
		get{return _tiles;}
	}
	// Returns tile with given ID
	private Dictionary<int, Tile> _tileByPosID;
	public Dictionary<int, Tile> TileByPosID {
		get{return _tileByPosID;}
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
	// Returns the number of moves taken so far this level
	private int _numMoves;
	public int NumMoves {
		get{return _numMoves;}
	}
	// Returns the lowest number of moves used to beat level
	private int _bestMoves;
	public int BestMoves {
		get{return _bestMoves;}
	}
	// Returns the trophy tier associated with _bestMoves
	private int _tier;
	public int Tier {
		get{return _tier;}
	}


	/// Public methods -------------------------------------------------------------
	// Play this level
	public void Play() {
		Build();
		_modified = false;
		_numMoves = 0;
	}

	// Resets level
	public void Reset() {
		ResetTiles();

		// Reset game vars
		_modified = false;
		_numMoves = 0;
		_bestMoves = LevelSelectController.CurrentLevelPack.Data.Levels[Level.CurrentLevel.ID].BestMoves;
		SetTier();
		GameController.Main.UpdateMoves(_numMoves);
	}
	// Resets tiles
	public void ResetTiles() {
		string[] tiles = _tileMap.Split(',');
		for(int i = 0; i < _tiles.Count; i++) {
			bool on = int.Parse(tiles[i]) > 0;
			_tiles[i].Set(_tiles[i].ID, on, _tiles[i].PosID);
		}
	}
	
	// Maps JObject data to Level
	public void LoadFrom(JObject levelData, JObject progData) {
		_id = levelData["id"].Value<int>();
		_width = levelData["width"].Value<int>();
		_tileMap = levelData["tile_map"].Value<string>();
		_tierReq = new int[] {levelData["silver"].Value<int>(), levelData["gold"].Value<int>()};

		_bestMoves = progData["moves"][_id].Value<int>();
		SetTier();
	}

	// Gets vector2 position for tile with given ID based on level.Width
	// Tiles are 1x1 unit with (0,0) starting bottom left
	public Vector2 GetTilePosition(int id) {
		int x = id % _width;
		int y = id / _width;
		return new Vector2(x, y);
	}

	// Runs on tile click, called from Tile
	public void ClickTile(int id) {
		foreach(int pairedID in _pairedTiles[id]) {
			_tiles[pairedID].FlipTile();
		}

		_modified = true;
		_numMoves++;
		if(GameController.Main != null) {
			GameController.Main.UpdateMoves(_numMoves);
			CheckWinCondition();
		}
	}

	/// Private methods ------------------------------------------------------------
	// Checks when condition after flipping tiles
	private void CheckWinCondition() {
		bool win = true;
		foreach(Tile tile in _tiles) {
			if(!tile.On) {
				win = false;
			}
		}

		if(win) {
			GameController.Main.Win();
		}
	}

	// Calculates what tier is associated with _bestMoves
	private void SetTier() {
		if(_bestMoves == 0) {
			_tier = 0;
			return;
		}
		_tier = 1;
		for(int i = 1; i <= _tierReq.Length; i++) {
			if(_bestMoves <= _tierReq[i - 1]) {
				_tier++;
			}
			else {
				break;
			}
		}
	}

	// Build input Level
	private void Build() {
		// Adjust camera based on level.Width
		CameraUtil.SizeToWidth(_width);

		// Create tiles
		_tiles = new List<Tile>();
		_tileByPosID = new Dictionary<int, Tile>();
		Transform tileContainer = new GameObject().transform;
		tileContainer.name = "Tiles";
		string[] tiles = _tileMap.Split(',');
		foreach(string curTile in tiles) {
			int posID = int.Parse(curTile);
			bool on = posID > 0;
			posID = Mathf.Abs(posID) - 1;
			Tile tile = GameObject.Instantiate<GameObject>(Tile.Prefab, tileContainer).GetComponent<Tile>();
			tile.name = tile.name.Substring(0, tile.name.Length - 7);
			tile.transform.localPosition = GetTilePosition(posID);
			tile.Set(_tiles.Count, on, posID);

			_tiles.Add(tile);
			_tileByPosID[posID] = tile;
		}

		// Pair tiles
		_pairedTiles = new Dictionary<int, List<int>>();
		foreach(Tile tile in _tiles) {
			List<int> pairedTiles = new List<int>();
			pairedTiles.Add(tile.ID);
			// Check up
			int i = 1;
			while(_tileByPosID.ContainsKey(tile.PosID + (i * _width))) {
				pairedTiles.Add(_tileByPosID[tile.PosID + (i * _width)].ID);
				i++;
			}
			// Check left
			i = 1;
			while(_tileByPosID.ContainsKey(tile.PosID - i) && (tile.PosID - i) / _width == tile.PosID / _width) {
				pairedTiles.Add(_tileByPosID[tile.PosID - i].ID);
				i++;
			}
			// Check down
			i = 1;
			while(_tileByPosID.ContainsKey(tile.PosID - (i * _width))) {
				pairedTiles.Add(_tileByPosID[tile.PosID - (i * _width)].ID);
				i++;
			}
			// Check right
			i = 1;
			while(_tileByPosID.ContainsKey(tile.PosID + i) && (tile.PosID + i) / _width == tile.PosID / _width) {
				pairedTiles.Add(_tileByPosID[tile.PosID + i].ID);
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
