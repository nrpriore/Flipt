using UnityEngine;					// To inherit from Monobehaviour
using System.Collections.Generic;	// For lists

// Controls the Start scene background
public class StartBG : MonoBehaviour {

	private List<Tile> _tiles;  // List of all tiles on map
	private Dictionary<int, Tile> _tileByID;  // Returns tile with given ID
	private Dictionary<int, List<int>> _pairedTiles;  // Returns list of IDs paired with given tile ID

	private List<int> _currCol;
	private List<int> _numRepeat;

	private int _num;  // Number of paths
	private int _minID;  // Minimum ID for tile, gradually increases as BG moves
	private int _width;  // Width of background map

	// Runs when Start scene is loaded (app startup)
	void Start () {
		InitBG();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	/// Private methods ------------------------------------------------------------
	// Builds starting background
	private void InitBG() {
		_tiles = new List<Tile>();
		_tileByID = new Dictionary<int, Tile>();
		_pairedTiles = new Dictionary<int, List<int>>();
		_num = 1;
		_minID = 0;
		_width = 4;

		_currCol = new List<int>();
		_numRepeat = new List<int>();
		for(int i = 0; i < _num; i++) {
			_currCol.Add(Mathf.FloorToInt(0.99f * Random.value * _width));
			_numRepeat.Add(0);
		}

		CreateNextRow();
	}

	// Determines next indexes and creates
	private void CreateNextRow() {
		foreach(int i in _currCol) {
			
		}
	}

}
