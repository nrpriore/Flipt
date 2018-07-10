using UnityEngine;	// Debug.Log
using System.Collections.Generic;  // List

// Class used during development to solve levels
public static class Solve {

	private static List<List<int>> _movesets;

	// Solves given Level
	public static void Level(Level level) {
		System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
		time.Start();

		List<Tile> uniqueTiles = new List<Tile>(level.Tiles);  // Remove tiles with identical pairs
		for(int i = 0; i < level.Tiles.Count-1; i++) {
			for(int j = i+1; j < level.Tiles.Count; j++) {
				if(UnorderedEqual(level.PairedTiles[level.Tiles[i].ID], level.PairedTiles[level.Tiles[j].ID])) {
					uniqueTiles.Remove(level.Tiles[j]);
				}
			}
		}
		
		// Loop through combinations of given numMoves, starting low
		for(int numMoves = 1; numMoves <= uniqueTiles.Count; numMoves++) {
			_movesets = new List<List<int>>();
			BuildMovesets(new List<int>(), numMoves, uniqueTiles.Count);  // Build movesets

			// Check moveset!
			level.ResetTiles();
			foreach(List<int> moveset in _movesets) {
				foreach(int move in moveset) {  // Execute moveset
					int tileID = uniqueTiles[move].ID;
					level.FlipPairedTiles(tileID);
				}

				bool win = true;  // Check win condition
				foreach(Tile tile in level.Tiles) {
					if(!tile.On) {
						win = false;
						break;
					}
				}

				if(win) {  // If win
					string solution = "";
					foreach(int move in moveset) {
						solution = solution + uniqueTiles[move].ID + ",";
					}
					solution = solution.Substring(0, solution.Length - 1);
					Debug.Log("Solved level by pressing " + solution);
					Debug.Log("Level can be solved in " + numMoves + " moves");

					time.Stop();
					Debug.Log(time.ElapsedMilliseconds);

					return;
				}

				level.ResetTiles();  // Reset if failed
			}

			Debug.Log("No solution in " + numMoves + " moves");			
		}
	}

	// Iterates through the possible combinations via recursion
	private static void BuildMovesets(List<int> moveset, int numMoves, int numTiles) {
		if(moveset.Count == numMoves) {
			_movesets.Add(moveset);
			return;
		}
		int start = (moveset.Count > 0)? moveset[moveset.Count-1]+1 : 0;
		for(int i = start; i < numTiles; i++) {
			List<int> next = new List<int>(moveset);
			next.Add(i);
			if(numTiles-1 - next[next.Count-1] < (numMoves - next.Count)) {
				return;
			}
			BuildMovesets(next, numMoves, numTiles);
		}
	}
	
	// Logs the values in a list
	private static void Log(List<int> list) {
		string debug = "";
		foreach(int move in list) {
			debug = debug + move + ",";
		}
		debug = debug.Substring(0, debug.Length - 1);
		Debug.Log(debug);
	}

	// Determines unordered equality between integer lists
	static bool UnorderedEqual(List<int> a, List<int> b) {
        // Require that the counts are equal
        if(a.Count != b.Count) {
            return false;
        }
        // Set value frequencies and compare
        Dictionary<int, int> d = new Dictionary<int, int>();
        foreach(int item in a) {
            int c;
            if(d.TryGetValue(item, out c)) {
                d[item] = c + 1;
            }
            else {
                d.Add(item, 1);
            }
        }
        foreach(int item in b) {
            int c;
            if(d.TryGetValue(item, out c)) {
                if (c == 0) {
                    return false;
                }
                else {
                    d[item] = c - 1;
                }
            }
            else {
                // Not in dictionary
                return false;
            }
        }
        // Verify that all frequencies are zero
        foreach(int v in d.Values) {
            if(v != 0) {
                return false;
            }
        }

        return true;
    }

}
