using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	/**
	 * Starts the game.
	 */
	public void Play() {
		Application.LoadLevelAdditive("ChooseGame");
	}

	/**
	 * Achievements.
	 */
	public void Achievements() {
		Application.LoadLevelAdditive("Achievements");
	}

	/**
	 * Credits.
	 */
	public void Credits() {
		Application.LoadLevelAdditive("Credits");
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
