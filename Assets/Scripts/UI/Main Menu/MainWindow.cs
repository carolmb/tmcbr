using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainWindow : MonoBehaviour {

	public Button start;

	public void OnEnable() {
		start.interactable = Bag.current.HasItem (Item.DB[9]);
	}

	public void ItemButton () {
		SoundManager.Click ();
		GameHUD.instance.mainMenu.itemWindow.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

	public void MapButton () {
		SoundManager.Click ();
		MapWindow map = GameHUD.instance.mainMenu.mapWindow;
		MapWindow.UpdateTexture (map.miniMap.GetComponent<Image> (), Vector2.one / 2);
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

	public void OnStart () {
		MazeManager.GoToMaze (SaveManager.currentSave.start);
	}

	public void Return () {
		SoundManager.Click ();
		Player.instance.Resume ();
		GameHUD.instance.gameObject.SetActive (true);
		GameHUD.instance.mainMenu.Close ();
	}

}
