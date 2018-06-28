using UnityEngine;					// To inherit from Monobehaviour
using UnityEngine.SceneManagement;	// To change scenes
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
	private Modal _confirmMenu;
	private Modal _shopMenu;

	// Runs immedietely on instantiation before the next line of code from the instantiating script
	void Awake() {
		_name = transform.Find("Name").GetComponent<Text>();
		_locked = transform.Find("Locked").gameObject;
		_progress = transform.Find("Progress").GetComponentsInChildren<Image>();
		_confirmMenu = transform.Find("Menus").Find("ConfirmPlayMenu").GetComponent<ConfirmPlayMenu>();
		
		_shopMenu = GameObject.Find("ShopMenu").GetComponent<ShopMenu>();
	}

	/// Public methods -------------------------------------------------------------
	// Maps data to UI objects
	public void SetData(LevelPackData data) {
		_data = data;
		_name.text = data.Name;
		_locked.SetActive(!data.Unlocked);
		if(!data.Unlocked) {
			gameObject.GetComponent<Button>().interactable = false;
		}
		for(int i = 0; i < data.Progress; i++) {
			_progress[i].color = ColorUtil.HexToColor(PROGRESS_HEX);
		}
	}

	// Runs when levelpack button clicked
	public void SelectLevelPack() {
		if(_data.Unlocked) {
			if(LevelSelectController.CurrentLevelPack != null) {
				LevelSelectController.CurrentLevelPack.UnselectLevelPack();
			}
			LevelSelectController.CurrentLevelPack = this;
			_confirmMenu.Open();
		}
		else {

		}
	}
	// Runs when cancel is hit or another levelpack button
	public void UnselectLevelPack() {
		_confirmMenu.Close();
		LevelSelectController.CurrentLevelPack = null;
	}

	// Runs when play is selected from ConfirmPlayMenu
	public void PlayLevelPack() {
		Level.CurrentLevel = _data.Levels[_data.Progress];
		SceneManager.LoadScene("Game", LoadSceneMode.Single);
	}

	// Opens shop menu - likely to unlock level pack
	public void OpenShop() {
		_shopMenu.Open();
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

		string dataStr = PlayerPrefs.GetString("progress" + jo["id"].Value<int>());
		dataStr = CryptoUtil.Decrypt(dataStr);
		JObject data = JObject.Parse(dataStr);
		Unlocked = data["unlocked"].Value<bool>();
		Progress = data["progress"].Value<int>();
	}
}