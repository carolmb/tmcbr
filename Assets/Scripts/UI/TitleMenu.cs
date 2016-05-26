using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour {

	public Button loadButton;
	public Button[] saveButtons;

	public void Start() {
		SaveManager.currentSave = null;
		SaveManager.maxSaves = saveButtons.Length;
		SaveManager.LoadSaves ();
		bool hasSave = false;
		for (int i = 0; i < SaveManager.maxSaves; i++) {
			if (SaveManager.allSaves [i] != null) {
				saveButtons [i].interactable = true;
				saveButtons [i].GetComponentInChildren<Text> ().text = SaveManager.allSaves[i].name;
				hasSave = true;
			} else {
				saveButtons [i].interactable = false;
				saveButtons [i].GetComponentInChildren<Text> ().text = "Empty";
			}
		}
		loadButton.interactable = hasSave;
	}

	public void CallGameScene() {
		SceneManager.LoadScene ("Game");
	}

	public void LoadGame(int id) {
		SaveManager.LoadGame (id);
		CallGameScene ();
	}

	public void Quit() {
		Application.Quit ();
	}

	public AudioClip confirmSound;

	public void Sound() {
		AudioSource.PlayClipAtPoint(confirmSound, new Vector3(0,0,0));
	}

}
