using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadWindow : WindowBase {

	public Button[] saveButtons;

	public bool CheckSaves () {
		SaveManager.currentSave = null;
		SaveManager.maxSaves = saveButtons.Length;
		SaveManager.LoadList ();
		bool hasSave = false;
		for (int i = 0; i < SaveManager.maxSaves; i++) {
			if (SaveManager.saveList [i] != null) {
				saveButtons [i].interactable = true;
				saveButtons [i].GetComponentInChildren<Text> ().text = SaveManager.saveList[i];
				hasSave = true;
			} else {
				saveButtons [i].interactable = false;
				saveButtons [i].GetComponentInChildren<Text> ().text = "Empty";
			}
		}
		return hasSave;
	}

	public void LoadGame (int id) {
		SaveManager.LoadSave (id);
		TitleMenu.instance.titleWindow.OnNewGame ();
	}

	public void Return () {
		SoundManager.Click ();
		gameObject.SetActive (false);
		TitleMenu.instance.titleWindow.gameObject.SetActive (true);
	}

}
