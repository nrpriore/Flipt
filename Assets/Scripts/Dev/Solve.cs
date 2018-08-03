//using UnityEngine;				// Debug.Log
using System;						// Random
using System.Threading;
using System.Collections.Generic;	// List

// Class used during development to solve levels
public static class Solve {

	private static List<List<int>> _movesets;
	private static List<bool> _bool0;  // Starting 'on' config. reset to this after check
	private static List<bool> _bool1;  // Current config in algo
	private static List<int> _ids;

	private static int _tileID;
	private static int _start;
	private static List<int> _next;
	private static bool _win;

	private static int _hintID;
	public static int HintID {
		get{return _hintID;}
	}
	private static Random rnd = new Random();

	public static IEnumerator<int> GetNextHint(Level level) {
		_hintID = -1;
		Thread solve_thread = new Thread( () => {SolveLevel(level);} );
		solve_thread.IsBackground = true;
		solve_thread.Start();

		while(_hintID < 0) {
			yield return -1;
		}

		//Debug.Log("Next hint is " + _hintID);
	}

	// Solves given Level
	private static void SolveLevel(Level level) {
		/*_freqOffset = (float)System.Diagnostics.Stopwatch.Frequency / 1000000f;
		_time.Restart();
		_prevTime = 0;*/

		_ids = new List<int>();
		List<Tile> uniqueTiles = new List<Tile>(level.Tiles);  // Remove tiles with identical pairs
		for(int i = 0; i < uniqueTiles.Count-1; i++) {
			for(int j = i+1; j < uniqueTiles.Count; j++) {
				if(UnorderedEqual(level.PairedTiles[uniqueTiles[i].ID], level.PairedTiles[uniqueTiles[j].ID])) {
					uniqueTiles.Remove(uniqueTiles[j]);
				}
			}
			_ids.Add(uniqueTiles[i].ID);
		}
		_ids.Add(uniqueTiles[uniqueTiles.Count-1].ID);
		//LogTime("Remove redundant tiles");

		// Set bool array to reset tiles quicker
		_bool0 = new List<bool>();
		for(int i = 0; i < level.Tiles.Count; i++) {
			_bool0.Add(level.Tiles[i].On);
		}
		_bool1 = new List<bool>(_bool0);
		//LogTime("Made bool and pos array");

		
		// Loop through combinations of given numMoves, starting low
		for(int numMoves = 1; numMoves <= uniqueTiles.Count; numMoves++) {
			_movesets = new List<List<int>>();
			BuildMovesets(new List<int>(), numMoves, uniqueTiles.Count);  // Build movesets
			//LogTime("Build moveset for " + numMoves + " moves");

			// Check moveset!
			//_avgTime = 0f;
			//_avgFlipTime = 0f;
			//_avgCheckTime = 0f;
			foreach(List<int> moveset in _movesets) {
				//movesettime.Restart();

				//fliptime.Restart();
				foreach(int move in moveset) {  // Execute moveset
					foreach(int pair in level.PairedTiles[_ids[move]]) {
						_bool1[pair] ^= true;
					}
				}
				//_avgFlipTime += fliptime.ElapsedTicks;
				//fliptime.Stop();

				//checktime.Restart();
				_win = true;
				for(int i = 0; i < _bool1.Count; i++) {
					if(!_bool1[i]) {
						_win = false;
						_bool1 = new List<bool>(_bool0);
						break;
					}
				}
				//_avgCheckTime += checktime.ElapsedTicks;
				//checktime.Stop();

				if(_win) {  // If win
					/*LogTime("Found solution");
					_time.Reset();
					_prevTime = 0;*/

					/*string solution = "";
					foreach(int move in moveset) {
						solution = solution + uniqueTiles[move].ID + ",";
					}
					solution = solution.Substring(0, solution.Length - 1);
					Debug.Log("Solved level by pressing " + solution);*/
					//Debug.Log("Level can be solved in " + numMoves + " moves");

					_hintID = uniqueTiles[moveset[rnd.Next(moveset.Count)]].ID;
					return;
				}

				//_avgTime += movesettime.ElapsedTicks;
			}
			//_avgTime /= (_freqOffset * _movesets.Count);
			//_avgFlipTime /= (_freqOffset * _movesets.Count);
			//_avgCheckTime /= (_freqOffset * _movesets.Count);
			//Debug.Log("Average time per solution for " + numMoves + "moves: " + _avgTime + "us");
			//Debug.Log("Average time per fliptiles for " + numMoves + "moves: " + _avgFlipTime + "us");
			//Debug.Log("Average time per checksol for " + numMoves + "moves: " + _avgCheckTime + "us");

			//LogTime("Checked solutions for " + numMoves + " moves");
		}
	}

	// Iterates through the possible combinations via recursion
	private static void BuildMovesets(List<int> moveset, int numMoves, int numTiles) {
		if(moveset.Count == numMoves) {
			_movesets.Add(moveset);
			return;
		}
		_start = (moveset.Count > 0)? moveset[moveset.Count-1]+1 : 0;
		for(int i = _start; i <= numTiles + moveset.Count - numMoves; i++) {
			_next = new List<int>(moveset);
			_next.Add(i);
			BuildMovesets(_next, numMoves, numTiles);
		}
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

	/// Debug ----------------------------------------------------------------------

	/*private static float _freqOffset;
	private static System.Diagnostics.Stopwatch _time = new System.Diagnostics.Stopwatch();
	private static float _prevTime = 0;*/

	/*private static System.Diagnostics.Stopwatch movesettime = new System.Diagnostics.Stopwatch();
	private static float _avgTime;*/
	/*private static System.Diagnostics.Stopwatch fliptime = new System.Diagnostics.Stopwatch();
	private static float _avgFlipTime;*/
	/*private static System.Diagnostics.Stopwatch checktime = new System.Diagnostics.Stopwatch();
	private static float _avgCheckTime;*/


	// Logs the values in a list
	/*private static void LogList(List<int> list) {
		string debug = "";
		foreach(int move in list) {
			debug = debug + move + ",";
		}
		debug = debug.Substring(0, debug.Length - 1);
		Debug.Log(debug);
	}

	// Log the elapsed time taken
	private static void LogTime(string caption) {
		float newTime = (float)_time.ElapsedTicks - _prevTime;
		_prevTime = (float)_time.ElapsedTicks;

		newTime /= _freqOffset; // Convert to microseconds
		string inctime = "";
		if(newTime > 1000f) {
			newTime /= 1000f;
			inctime = newTime + "ms";
		}
		else {
			inctime = newTime + "us";
		}

		float total = _prevTime / _freqOffset;
		string tottime = "";
		if(total > 1000f) {
			total /= 1000f;
			tottime = total + "ms";
		}
		else {
			tottime = total + "us";
		}

		Debug.Log(caption + ": " + inctime + " - total: " + tottime);
	}*/

}
