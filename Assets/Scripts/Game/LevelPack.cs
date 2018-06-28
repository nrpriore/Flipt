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
	public void SetUIData(LevelPackData data) {
		RefreshData(data);
		_name.text = data.Name;
		_locked.SetActive(!data.Unlocked);
		if(!data.Unlocked) {
			gameObject.GetComponent<Button>().interactable = false;
		}
		for(int i = 0; i < data.Progress; i++) {
			_progress[i].color = ColorUtil.HexToColor(PROGRESS_HEX);//ColorUtil.TROPHY_COLOR[data.Levels[i].Tier];
			/*foreach(Transform child in _progress[i].transform) {
				child.gameObject.SetActive(true);
				child.GetComponent<Image>().color = ColorUtil.TROPHY_COLOR[data.Levels[i].Tier];
			}*/
		}
	}

	public void RefreshData(LevelPackData data) {
		_data = data;
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
	}

	// Runs when cancel is hit or another levelpack button
	public void UnselectLevelPack() {
		_confirmMenu.Close();
		LevelSelectController.CurrentLevelPack = null;
	}

	// Runs when play is selected from ConfirmPlayMenu
	public void PlayLevelPack() {
		Level.CurrentLevel = _data.Levels[Mathf.Min(_data.Progress, 9)];
		SceneManager.LoadScene("Game", LoadSceneMode.Single);
	}

	// Opens shop menu - likely to unlock level pack
	public void OpenShop() {
		_shopMenu.Open();
	}

}

// Contains data struct to load from data files
public struct LevelPackData {
	public int ID;
	public string Name;
	public bool Unlocked;
	public int Progress;
	public Level[] Levels;

	public JObject LPData;

	public LevelPackData(JObject lpData) {
		LPData = lpData; // Keep a copy so it's easy to update

		lpData = lpData["level_pack"].Value<JObject>();  // Remove level_pack since would be at start of every reference
		ID = lpData["id"].Value<int>();
		Name = lpData["name"].Value<string>();

		JObject progData = StaticLevelData.ReadData(ID);
		JArray levelsja = lpData["levels"].Value<JArray>();
		Levels = new Level[levelsja.Count];
		for(int i = 0; i < Levels.Length; i++) {
			Level level = new Level();
			level.LoadFrom(levelsja[i].Value<JObject>(), progData);
			Levels[i] = level;
		}

		Unlocked = progData["unlocked"].Value<bool>();
		Progress = progData["progress"].Value<int>();
	}
}