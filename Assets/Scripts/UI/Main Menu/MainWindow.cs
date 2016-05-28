using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainWindow : MonoBehaviour {

	public void ItemButton () {
		SoundManager.Click ();
		GameHUD.instance.mainMenu.itemWindow.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

	public void MapButton () {
		SoundManager.Click ();
		MapWindow map = GameHUD.instance.mainMenu.mapWindow;
		MapWindow.UpdateTexture (map.miniMap.GetComponent<Image> ());
		GameHUD.instance.mainMenu.mapWindow.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

	public void SaveButton () {
		SoundManager.Click ();
		GameHUD.instance.mainMenu.saveWindow.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

	public void QuitButton () {
		SoundManager.Click ();
		Destroy (MazeManager.musicPlayer.gameObject);
		SceneManager.LoadScene ("Title");
	}

	public void Return () {
		SoundManager.Click ();
		Player.instance.Resume ();
		GameHUD.instance.gameObject.SetActive (true);
		GameHUD.instance.mainMenu.Close ();
	}

}
