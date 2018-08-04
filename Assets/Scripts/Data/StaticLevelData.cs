using UnityEngine;					// For access to Application class
using Newtonsoft.Json.Linq;			// For access to JObject

// Class to store static level data
public static class StaticLevelData {

	// Static reference to existing levelpacks and data/progress, loaded on app startup
	private static LevelPackData[] _levelPacks;
	public static LevelPackData[] LevelPacks {
		get{return _levelPacks;}
	}

	/// Public methods -------------------------------------------------------------
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

	// Updates progress data, runs when you beat a level
	public static void UpdateProgressData() {
		// Set new best moves if better than existing record
		if(Level.CurrentLevel.NumMoves < Level.CurrentLevel.BestMoves || Level.CurrentLevel.BestMoves == 0) {
			UpdateBestMoves();
		}
		// Increment progress by 1 if user beats a new level
		if(Level.CurrentLevel.ID == LevelSelectController.CurrentLevelPack.Data.Progress) {
			IncrementLevelProgress();
		}
		// Unlock next level pack if user beats last level and next levelpack exists/not already unlocked
		if(Level.CurrentLevel.ID == 9 && LevelSelectController.CurrentLevelPack.Data.ID < _levelPacks.Length - 1) {
			if(!_levelPacks[LevelSelectController.CurrentLevelPack.Data.ID + 1].Unlocked) {
				UnlockLevelPack(LevelSelectController.CurrentLevelPack.Data.ID + 1);
			}
		}
	}

	// Initially sets or resets encrypted level data, called when new encryption keys are created
	public static void ResetLevelData() {
		TextAsset[] files = Resources.LoadAll<TextAsset>("Data/");
		for(int i = 0; i < files.Length; i++) {
			string data = DefLevelPackData(files[i].name);
			SaveData(i, data);
		}
	}

	// Reads levelpack data for given levelpackID
	public static JObject ReadData(int levelpackID) {
		string dataStr = PlayerPrefs.GetString("progress" + levelpackID);
		dataStr = CryptoUtil.Decrypt(dataStr);
		JObject data = JObject.Parse(dataStr);
		return data;
	}


	/// Private methods ------------------------------------------------------------
	// Update best moves for current level
	private static void UpdateBestMoves() {
		int id = LevelSelectController.CurrentLevelPack.Data.ID;
		JObject data = ReadData(id);
		data["moves"][Level.CurrentLevel.ID] = Level.CurrentLevel.NumMoves;
		SaveData(id, data.ToString());

		RefreshCurrentLevelPack();
	}

	// Increments level progress by 1 for current levelpackID
	private static void IncrementLevelProgress() {
		int id = LevelSelectController.CurrentLevelPack.Data.ID;
		JObject data = ReadData(id);
		data["progress"] = Mathf.Min(data["progress"].Value<int>() + 1, 10);
		SaveData(id, data.ToString());

		RefreshCurrentLevelPack();
	}

	// Unlocks levelpack with given levelpackID
	private static void UnlockLevelPack(int levelpackID) {
		JObject data = ReadData(levelpackID);
		data["unlocked"] = true;
		SaveData(levelpackID, data.ToString());

		RefreshLevelPack(levelpackID);
	}

	// Saves data for given levelpackID and updates _levelPacks
	private static void SaveData(int levelpackID, string data) {
		data = CryptoUtil.Encrypt(data);
		PlayerPrefs.SetString("progress" + levelpackID, data);
	}

	// Update _levelPacks with given levelpackID
	private static void RefreshLevelPack(int levelpackID) {
		_levelPacks[levelpackID] = new LevelPackData(_levelPacks[levelpackID].LPData);
	}

	// Update _levelPacks AND refresh current Level pack
	private static void RefreshCurrentLevelPack() {
		int id = LevelSelectController.CurrentLevelPack.Data.ID;
		RefreshLevelPack(id);
		LevelSelectController.CurrentLevelPack.RefreshData(_levelPacks[id]);
	}

	// Holds default levelpack data if we need to reset progress
	private static string DefLevelPackData(string name) {
		switch(name) {
			case "0":
				return "{\"id\":0,\"unlocked\":true,\"progress\":10,\"moves\":[6,6,6,6,6,6,6,6,6,6]}";
			case "1":
				return "{\"id\":1,\"unlocked\":true,\"progress\":10,\"moves\":[8,6,8,16,26,6,14,8,8,6]}";
			case "2":
				return "{\"id\":2,\"unlocked\":true,\"progress\":4,\"moves\":[14,26,10,6,0,0,0,0,0,0]}";
			case "3":
				return "{\"id\":3,\"unlocked\":false,\"progress\":0,\"moves\":[0,0,0,0,0,0,0,0,0,0]}";
			case "4":
				return "{\"id\":4,\"unlocked\":false,\"progress\":0,\"moves\":[0,0,0,0,0,0,0,0,0,0]}";
			case "5":
				return "{\"id\":5,\"unlocked\":false,\"progress\":0,\"moves\":[0,0,0,0,0,0,0,0,0,0]}";
			case "6":
				return "{\"id\":6,\"unlocked\":false,\"progress\":0,\"moves\":[0,0,0,0,0,0,0,0,0,0]}";
			case "7":
				return "{\"id\":7,\"unlocked\":false,\"progress\":0,\"moves\":[0,0,0,0,0,0,0,0,0,0]}";
			case "8":
				return "{\"id\":8,\"unlocked\":false,\"progress\":0,\"moves\":[0,0,0,0,0,0,0,0,0,0]}";
			case "9":
				return "{\"id\":9,\"unlocked\":false,\"progress\":0,\"moves\":[0,0,0,0,0,0,0,0,0,0]}";
			default:
				return "{\"id\":-1,\"unlocked\":false,\"progress\":0,\"moves\":[0,0,0,0,0,0,0,0,0,0]}";
		}
	}

}