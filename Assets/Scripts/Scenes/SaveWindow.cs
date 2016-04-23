using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SaveWindow : MonoBehaviour {

	public Text saveName;
	public Button[] saveButtons;

	public void UpdateSaveButtons() {
		SaveManager.LoadSaves ();
		for (int i = 0; i < SaveManager.maxSaves; i++) {
			if (SaveManager.allSaves [i] != null) {
				saveButtons [i].GetComponentInChildren<Text> ().text = SaveManager.allSaves [i].name;
			} else {
				saveButtons [i].GetComponentInChildren<Text> ().text = "Empty";
			}
		}
	}

	public void Save(int id) {
		GameMenu.instance.ClickItemSound ();
		SaveManager.SaveGame (id, saveName.text);
		UpdateSaveButtons ();
	}

	public void Return() {
		GameMenu.instance.ClickItemSound ();
		GameMenu.instance.mainWindow.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

}
