using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SaveWindow : MonoBehaviour {

	public Text saveName;
	public Button[] saveButtons;

	public void OnEnable() {
		SaveManager.LoadList ();
		for (int i = 0; i < SaveManager.maxSaves; i++) {
			if (SaveManager.saveList [i] != null) {
				saveButtons [i].GetComponentInChildren<Text> ().text = SaveManager.saveList [i];
			} else {
				saveButtons [i].GetComponentInChildren<Text> ().text = "Empty";
			}
		}
	}

	public void Save(int id) {
		SoundManager.Click ();
		SaveManager.SaveGame (id, saveName.text);
		OnEnable ();
	}

	public void Return() {
		SoundManager.Click ();
		GameHUD.instance.mainMenu.mainWindow.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

}
