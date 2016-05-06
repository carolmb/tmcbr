using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainWindow : MonoBehaviour {

	public void ItemButton () {
		GameHUD.ClickItemSound ();
		MainMenu.instance.itemWindow.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

	public void MapButton () {
		GameHUD.ClickItemSound ();
		MainMenu.instance.mapWindow.UpdateTexture ();
		MainMenu.instance.mapWindow.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

	public void SaveButton () {
		GameHUD.ClickItemSound ();
		MainMenu.instance.saveWindow.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

	public void QuitButton () {
		GameHUD.ClickItemSound ();
		SceneManager.LoadScene ("Title");
	}

	public void Return () {
		GameHUD.ClickItemSound ();
		Player.instance.Resume ();
		GameHUD.instance.gameObject.SetActive (true);
		MainMenu.instance.Close ();
	}

}
