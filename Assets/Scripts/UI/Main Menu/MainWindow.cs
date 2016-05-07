using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainWindow : MonoBehaviour {

	public void ItemButton () {
		GameHUD.ClickItemSound ();
		GameHUD.instance.mainMenu.itemWindow.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

	public void MapButton () {
		GameHUD.ClickItemSound ();
		GameHUD.instance.mainMenu.mapWindow.UpdateTexture ();
		GameHUD.instance.mainMenu.mapWindow.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

	public void SaveButton () {
		GameHUD.ClickItemSound ();
		GameHUD.instance.mainMenu.saveWindow.gameObject.SetActive (true);
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
		GameHUD.instance.mainMenu.Close ();
	}

}
