using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleWindow : MonoBehaviour {

	public Button loadButton;

	public void UpdateLoadButton () {
		loadButton.interactable = TitleMenu.instance.loadWindow.CheckSaves ();
	}

	public void OnNewGame () {
		if (TitleMenu.fading)
			return;
		SoundManager.Click ();
		TitleMenu.instance.StartCoroutine (NewGameFade ());
	}

	public void OnLoad() {
		if (TitleMenu.fading)
			return;
		SoundManager.Click ();
		TitleMenu.instance.loadWindow.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

	public void OnCredits () {
		if (TitleMenu.fading)
			return;
		SoundManager.Click ();
		TitleMenu.instance.loadWindow.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

	public void Quit() {
		if (TitleMenu.fading)
			return;
		SoundManager.Click ();
		TitleMenu.instance.StartCoroutine (QuitFade ());
	}

	private IEnumerator NewGameFade () {
		TitleMenu.fading = true;
		yield return TitleMenu.instance.StartCoroutine(GameCamera.instance.FadeOut (5));
		SceneManager.LoadScene ("Game");
	}

	private IEnumerator QuitFade () {
		TitleMenu.fading = true;
		yield return TitleMenu.instance.StartCoroutine(GameCamera.instance.FadeOut (5));
		Application.Quit ();
	}

}
