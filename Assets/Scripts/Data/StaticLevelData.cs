using UnityEngine;					// For access to Application class
using System.IO;					// For directory & file classes

// Class to store static level data
public static class StaticLevelData {

	// Static reference to existing levelpacks and data/progress, loaded on app startup
	private static LevelPack[] _levelPacks;
	public static LevelPack[] LevelPacks {
		get{return _levelPacks;}
	}

	// Loads levelpack data/progress on app startup
	public static void LoadLevelData() {
		DirectoryInfo data = new DirectoryInfo(Application.streamingAssetsPath + "/Data");
		FileInfo[] files = data.GetFiles();
		Debug.Log(files.Length);
	}

}