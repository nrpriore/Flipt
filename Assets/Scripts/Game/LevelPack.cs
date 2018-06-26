using UnityEngine;					// To inherit from Monobehaviour
using UnityEngine.UI;				// To set UI properties on button
using Newtonsoft.Json.Linq;			// For access to JObject

// Loaded static data for a levelpack. Mapped to LevelPackUI for LevelSelect scene
public class LevelPack : MonoBehaviour {

	private const string PROGRESS_HEX = "00CB2A"; // Color to set level progress image if complete

	// Holds a copy of the LevelPackData
	private LevelPackData _data;
	public LevelPackData Data {
		get{return _data;}
	}

	// References to UI objects that are set when instance is created
	private Text _name;
	private GameObject _locked;
	private Image[] _progress;

	// Runs immedietely on instantiation before the next line of code from the instantiating script
	void Awake() {
		_name = transform.Find("Name").GetComponent<Text>();
		_locked = transform.Find("Locked").gameObject;
		_progress = transform.Find("Progress").GetComponentsInChildren<Image>();
	}

	/// Public methods -------------------------------------------------------------
	// Maps data to UI objects
	public void SetData(LevelPackData data) {
		_data = data;
		_name.text = data.Name;
		_locked.SetActive(!data.Unlocked);
		for(int i = 0; i < data.Progress; i++) {
			_progress[i].color = ColorUtil.HexToColor(PROGRESS_HEX);
		}
	}

}

// Contains data struct to load from data files
public struct LevelPackData {
	public string Name;
	public bool Unlocked;
	public int Progress;
	public Level[] Levels;

	public LevelPackData(JObject jo) {
		jo = jo["level_pack"].Value<JObject>();  // Remove level_pack since would be at start of every reference
		Name = jo["name"].Value<string>();

		JArray levelsja = jo["levels"].Value<JArray>();
		Levels = new Level[levelsja.Count];
		for(int i = 0; i < Levels.Length; i++) {
			Level level = new Level();
			level.LoadFrom(levelsja[i].Value<JObject>());
			Levels[i] = level;
		}

		string endcodedData = jo["data"].Value<string>();
		string decodedData = CryptoUtil.Decrypt(endcodedData);
		JObject data = JObject.Parse(decodedData);
		Unlocked = data["unlocked"].Value<bool>();
		Progress = data["progress"].Value<int>();
	}
}