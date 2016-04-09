using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.IO;

public class ChooseGame : MonoBehaviour {

	public string gameName;
	/**
	 * Stars the game.
	 */
	public void NewGame() {
		//create new game
		if(gameName == null){
			gameName = "newGame";
		}
		SaveManager.NewSave(gameName);
		SceneManager.LoadScene("Game");
	}

	public void LoadGame() {
		if (SaveManager.LoadGame (gameName)) {
			SceneManager.LoadScene ("Game");
		}
	}

	public void SetLoadGameName(string inputFieldString){
		gameName = inputFieldString;
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
