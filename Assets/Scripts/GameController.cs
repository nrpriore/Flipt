using UnityEngine;					// To inherit from Monobehaviour
using System.Collections.Generic;	// For dictionaries

// Main class for Game scene
public class GameController : MonoBehaviour {

	private List<Tile> _tiles;  // List of all tiles on map
	private Dictionary<int, Tile> _tileByID;  // Returns tile with given ID
	private Dictionary<int, List<int>> _pairedTiles;  // Returns list of IDs paired with given tile ID


	// Runs when Game scene is loaded
	void Start () {
		Level level = (Level.CurrentLevel != null)? Level.CurrentLevel : Level.TestLevel();
		BuildLevel(level);
	}

	/// Public methods -------------------------------------------------------------
	// Flips paired tiles for input tile id
	public void FlipPairedTiles(int id) {
		_tileByID[id].FlipTile();
		List<int> pairedIDs = _pairedTiles[id];
		foreach(int pairedID in pairedIDs) {
			_tileByID[pairedID].FlipTile();
		}
	}

	/// Private methods ------------------------------------------------------------
	// Build input Level
	private void BuildLevel(Level level) {
		// Adjust camera based on level.Width
		float aspect = (float)Screen.width / Screen.height;
		float tempOrtho = level.Width / (2f * aspect);
		float camY = tempOrtho - 0.5f;
		camY -= ((tempOrtho * 2f) - Mathf.Floor(tempOrtho * 2f)) / 2f;
		float camX = (tempOrtho * aspect) - 0.5f;
		Camera.main.orthographicSize = tempOrtho * 1.1f;
		Camera.main.transform.localPosition = new Vector3(camX, camY, -10f);

		// Create tiles
		_tiles = new List<Tile>();
		_tileByID = new Dictionary<int, Tile>();
		string[] tiles = level.TileMap.Split(',');
		foreach(string curTile in tiles) {
			int id = int.Parse(curTile);
			bool on = id > 0;
			id = Mathf.Abs(id);

			Tile tile = Instantiate<GameObject>(Tile.Prefab, GameObject.Find("Tiles").transform).GetComponent<Tile>();
			tile.name = tile.name.Substring(0, tile.name.Length - 7);
			tile.transform.localPosition = level.GetTilePosition(id);
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
			while(_tileByID.ContainsKey(tile.ID + (i * level.Width))) {
				pairedTiles.Add(tile.ID + (i * level.Width));
				i++;
			}
			// Check left
			i = 1;
			while(_tileByID.ContainsKey(tile.ID - i) && (tile.ID - i) / level.Width == tile.ID / level.Width) {
				pairedTiles.Add(tile.ID - i);
				i++;
			}
			// Check down
			i = 1;
			while(_tileByID.ContainsKey(tile.ID - (i * level.Width))) {
				pairedTiles.Add(tile.ID - (i * level.Width));
				i++;
			}
			// Check right
			i = 1;
			while(_tileByID.ContainsKey(tile.ID + i) && (tile.ID + i) / level.Width == tile.ID / level.Width) {
				pairedTiles.Add(tile.ID + i);
				i++;
			}
			// Set dictionary
			if(pairedTiles.Count > 0) {
				_pairedTiles[tile.ID] = pairedTiles;
			}
		}
	}

}
