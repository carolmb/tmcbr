using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour {

	public Text saveName;
	public Button[] saveButtons;

	void Start() {
		UpdateSaveButtons ();
	}

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
		SaveManager.SaveGame (id, saveName.text);
		UpdateSaveButtons ();
	}

	public void Quit() {
		SceneManager.LoadScene ("MainMenu");
	}

}
