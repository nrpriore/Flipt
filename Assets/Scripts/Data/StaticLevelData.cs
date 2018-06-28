using UnityEngine;					// For access to Application class
using Newtonsoft.Json.Linq;			// For access to JObject

// Class to store static level data
public static class StaticLevelData {

	// Static reference to existing levelpacks and data/progress, loaded on app startup
	private static LevelPackData[] _levelPacks;
	public static LevelPackData[] LevelPacks {
		get{return _levelPacks;}
	}

	// Loads levelpack data/progress on app startup
	public static void LoadLevelData() {
		TextAsset[] files = Resources.LoadAll<TextAsset>("Data/");
		_levelPacks = new LevelPackData[files.Length];

		for(int i = 0; i < files.Length; i++) {
			string data = files[i].text;
			JObject jo = JObject.Parse(data);
			LevelPackData lp = new LevelPackData(jo);
			_levelPacks[i] = lp;
		}
	}

	// Initially sets or resets encrypted level data, called when new encryption keys are created
	public static void ResetLevelData() {
		TextAsset[] files = Resources.LoadAll<TextAsset>("Data/");
		for(int i = 0; i < files.Length; i++) {
			string data = DefLevelPackData(files[i].name);
			data = CryptoUtil.Encrypt(data);
			PlayerPrefs.SetString("progress" + i, data);
		}
	}

	// Holds default levelpack data if we need to reset progress
	private static string DefLevelPackData(string name) {
		switch(name) {
			case "0":
				return "{\"id\":0,\"unlocked\":true,\"progress\":10}";
			case "1":
				return "{\"id\":1,\"unlocked\":true,\"progress\":10}";
			case "2":
				return "{\"id\":2,\"unlocked\":true,\"progress\":4}";
			case "3":
				return "{\"id\":3,\"unlocked\":false,\"progress\":0}";
			case "4":
				return "{\"id\":4,\"unlocked\":false,\"progress\":0}";
			case "5":
				return "{\"id\":5,\"unlocked\":false,\"progress\":0}";
			case "6":
				return "{\"id\":6,\"unlocked\":false,\"progress\":0}";
			case "7":
				return "{\"id\":7,\"unlocked\":false,\"progress\":0}";
			case "8":
				return "{\"id\":8,\"unlocked\":false,\"progress\":0}";
			case "9":
				return "{\"id\":9,\"unlocked\":false,\"progress\":0}";
			default:
				return "{\"id\":-1,\"unlocked\":false,\"progress\":0}";
		}
	}

}