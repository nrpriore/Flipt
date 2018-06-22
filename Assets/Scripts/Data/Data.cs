using UnityEngine;					// For PlayerPrefs

// Handles saving and loading game & progress data
public static class Data {

	// Latest version of data, increments by 1
	private const int LatestVersion = 1;


	/// Public methods -------------------------------------------------------------
	// Compares LatestVersion to playerpref version and loads difference
	public static void CheckForUpdates() {
		// Initialize with version 0 if first game load
		if(!PlayerPrefs.HasKey("Version")) {
			PlayerPrefs.SetInt("Version", 0);
		}

		// Loop through each new version and load data
		for(int i = PlayerPrefs.GetInt("Version") + 1; i <= LatestVersion; i++) {
			DataInit(i);
		}
	}


	/// Private methods ------------------------------------------------------------
	// Load in data for input version
	private static void DataInit(int version) {
		switch(version) {
			case 1:
				PlayerPrefs.SetString("P1Name", "Tutorial");
				PlayerPrefs.SetInt("P1Progress", 0);

				PlayerPrefs.SetString("P2Name", "Beginner");
				PlayerPrefs.SetInt("P2Progress", 0);

				PlayerPrefs.SetString("P3Name", "I Maybe Understand");
				PlayerPrefs.SetInt("P3Progress", 0);

				PlayerPrefs.SetString("P4Name", "Kinda Got This");
				PlayerPrefs.SetInt("P4Progress", 0);

				PlayerPrefs.SetString("P5Name", "Easy");
				PlayerPrefs.SetInt("P5Progress", 0);

				PlayerPrefs.SetString("P6Name", "JK This Is Hard");
				PlayerPrefs.SetInt("P6Progress", 0);

				PlayerPrefs.SetString("P7Name", "JK I Got This");
				PlayerPrefs.SetInt("P7Progress", 0);

				PlayerPrefs.SetString("P8Name", "Medium");
				PlayerPrefs.SetInt("P8Progress", 0);

				PlayerPrefs.SetString("P9Name", "Can't Touch This");
				PlayerPrefs.SetInt("P9Progress", 0);

				PlayerPrefs.SetString("P10Name", "Do Your Worst");
				PlayerPrefs.SetInt("P10Progress", 0);

				break;
			case 2:

				break;
			case 3:

				break;
		} 

		// Set version pref
		PlayerPrefs.SetInt("Version", version);
	}
}
