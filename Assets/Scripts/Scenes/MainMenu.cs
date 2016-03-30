using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	/**
	 * Starts the game.
	 */
	public void Play() {
		SceneManager.LoadScene("ChooseGame");
	}

	/**
	 * Achievements.
	 */
	public void Achievements() {
		SceneManager.LoadScene("Achievements");
	}

	/**
	 * Credits.
	 */
	public void Credits() {
		SceneManager.LoadScene("Credits");
	}

	/**
	 * Ends the game.
	 */
	public void Quit() {
		Application.Quit();
	}

	public void MouseEnter() {
		//
	}

	public void MouseExit() {
		//
	}
}
