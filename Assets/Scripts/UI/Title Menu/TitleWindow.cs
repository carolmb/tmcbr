using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleWindow : WindowBase {

	public Button loadButton;

	public void UpdateLoadButton () {
		loadButton.interactable = TitleMenu.instance.loadWindow.CheckSaves ();
	}

	public void OnNewGame () {
		SoundManager.Click ();
		SceneManager.LoadScene ("Game");
	}

	public void OnLoad() {
		SoundManager.Click ();
		TitleMenu.instance.loadWindow.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

	public void OnTutorial () {
		SoundManager.Click ();
		TitleMenu.instance.tutorialWindow.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

	public void OnCredits () {
		SoundManager.Click ();
		TitleMenu.instance.creditsWindow.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

	public void Quit() {
		SoundManager.Click ();
		Application.Quit ();
	}

}
