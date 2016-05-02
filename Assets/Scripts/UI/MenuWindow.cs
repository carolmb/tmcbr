using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuWindow : MonoBehaviour {

	public void ItemButton() {
		GameMenu.instance.ClickItemSound ();
		GameMenu.instance.itemWindow.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

	public void MapButton() {
		GameMenu.instance.ClickItemSound ();
		GameMenu.instance.mapWindow.UpdateTexture ();
		GameMenu.instance.mapWindow.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

	public void SaveButton() {
		GameMenu.instance.ClickItemSound ();
		GameMenu.instance.saveWindow.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

	public void QuitButton() {
		GameMenu.instance.ClickItemSound ();
		SceneManager.LoadScene ("MainMenu");
	}

	public void Return() {
		GameMenu.instance.ClickItemSound ();
		Player.instance.Resume ();
		GameMenu.instance.gameWindow.SetActive (true);
		gameObject.SetActive (false);
	}

}
