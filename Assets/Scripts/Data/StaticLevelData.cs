using UnityEngine;					// For access to Application class
using System.IO;					// For directory & file classes
using Newtonsoft.Json.Linq;			// For access to JObject
using Newtonsoft.Json;				// Access to Formatting enum

// Class to store static level data
public static class StaticLevelData {

	// Static reference to existing levelpacks and data/progress, loaded on app startup
	private static LevelPackData[] _levelPacks;
	public static LevelPackData[] LevelPacks {
		get{return _levelPacks;}
	}

	// Loads levelpack data/progress on app startup
	public static void LoadLevelData() {
		DirectoryInfo dataDir = new DirectoryInfo(Application.streamingAssetsPath + "/Data");
		FileInfo[] files = dataDir.GetFiles("*.txt");
		
		_levelPacks = new LevelPackData[files.Length];
		for(int i = 0; i < files.Length; i++) {
			using(StreamReader sr = new StreamReader(files[i].FullName, System.Text.Encoding.ASCII)) {
				string data = sr.ReadToEnd();
				JObject jo = JObject.Parse(data);

				LevelPackData lp = new LevelPackData(jo);
				_levelPacks[i] = lp;
			}
		}
	}

	// Initially sets or resets encrypted level data, called when new encryption keys are created
	public static void ResetLevelData() {
		DirectoryInfo dataDir = new DirectoryInfo(Application.streamingAssetsPath + "/Data");
		FileInfo[] files = dataDir.GetFiles("*.txt");
		for(int i = 0; i < files.Length; i++) {
			JObject jo;
			using(StreamReader sr = new StreamReader(files[i].FullName, System.Text.Encoding.ASCII)) {
				string data = sr.ReadToEnd();
				jo = JObject.Parse(data);
				string defaultData = DefLevelPackData(files[i].Name);
				jo["level_pack"]["data"] = CryptoUtil.Encrypt(defaultData);
			}
			File.WriteAllText(files[i].FullName, jo.ToString(Formatting.None), System.Text.Encoding.ASCII);
		}
	}
	// Holds default levelpack data if we need to reset progress
	private static string DefLevelPackData(string name) {
		switch(name) {
			case "0.txt":
				return "{\"id\":0,\"unlocked\":true,\"progress\":10}";
			case "1.txt":
				return "{\"id\":1,\"unlocked\":true,\"progress\":10}";
			case "2.txt":
				return "{\"id\":2,\"unlocked\":true,\"progress\":4}";
			case "3.txt":
				return "{\"id\":3,\"unlocked\":false,\"progress\":0}";
			case "4.txt":
				return "{\"id\":4,\"unlocked\":false,\"progress\":0}";
			case "5.txt":
				return "{\"id\":5,\"unlocked\":false,\"progress\":0}";
			case "6.txt":
				return "{\"id\":6,\"unlocked\":false,\"progress\":0}";
			case "7.txt":
				return "{\"id\":7,\"unlocked\":false,\"progress\":0}";
			case "8.txt":
				return "{\"id\":8,\"unlocked\":false,\"progress\":0}";
			case "9.txt":
				return "{\"id\":9,\"unlocked\":false,\"progress\":0}";
			default:
				return "{\"id\":-1,\"unlocked\":false,\"progress\":0}";
		}
	}

}