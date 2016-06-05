using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SaveWindow : WindowBase {

	public Text saveName;
	public Button[] saveButtons;

	protected override void OnEnable() {
		SaveManager.LoadList ();
		base.OnEnable ();
		for (int i = 0; i < SaveManager.maxSaves; i++) {
			if (SaveManager.saveList [i] != null) {
				saveButtons [i].GetComponentInChildren<Text> ().text = SaveManager.saveList [i];
			} else {
				saveButtons [i].GetComponentInChildren<Text> ().text = "Empty";
			}
		}
		//saveName.text = SaveManager.currentSave.name;
	}

	public void Save(int id) {
		SoundManager.Rose ();
		SaveManager.SaveGame (id, saveName.text);
		OnEnable ();
	}

	public void Return() {
		SoundManager.Click ();
		GameHUD.instance.mainMenu.mainWindow.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

}
