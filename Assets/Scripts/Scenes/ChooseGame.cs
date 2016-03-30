using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChooseGame : MonoBehaviour {

	/**
	 * Stars the game.
	 */
	public void Play() {
		SceneManager.LoadScene("Game");
	}

	/**
	 * Back to main menu.
	 */
	public void Back() {
		SceneManager.LoadScene("MainMenu");
	}
	
	public void MouseEnter() {
		//
	}

	public void MouseExit() {
		//
	}
}
