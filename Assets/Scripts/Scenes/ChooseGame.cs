using UnityEngine;
using System.Collections;

public class ChooseGame : MonoBehaviour {

	/**
	 * Stars the game.
	 */
	public void Play() {
		Application.LoadLevel("Game");
	}

	/**
	 * Back to main menu.
	 */
	public void Back() {
		Application.LoadLevel("MainMenu");
	}
	
	public void MouseEnter() {
		//
	}

	public void MouseExit() {
		//
	}
}
