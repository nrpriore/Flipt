using UnityEngine;					// For access to Vector2

// Holds the level information and methods
public class Level {

	public static Level CurrentLevel = null;


	/// Level information ----------------------------------------------------------
	// Width of level in Unity units (Generally 5-10)
	private int _width;
	public int Width {
		get{return _width;}
	}
	// Tile pattern in easily savable string format
	// (+/-) = tile starts on/off, ID, (comma): "4,-8,-9,-10,12,16"
	private string _tileMap;
	public string TileMap {
		get{return _tileMap;}
	}

	/// Public methods -------------------------------------------------------------
	// Gets vector2 position for tile with given ID based on level.Width
	// Tiles are 1x1 unit with (0,0) starting bottom left
	public Vector2 GetTilePosition(int id) {
		int x = id % _width;
		int y = id / _width;
		return new Vector2(x, y);
	}


	/// Test -----------------------------------------------------------------------
	// Instantiates a test level configuration
	public static Level TestLevel() {
		Level test = new Level();
		test._width = 4;
		test._tileMap = "4,8,9,10,12,14,-16,17,18,19,20";

		return test;
	}
	public static Level TestLevel2() {
		Level test = new Level();
		test._width = 3;
		test._tileMap = "3,4,5,6,7,8,9,10,11";

		return test;
	}
}
