using UnityEngine;
using System.Collections;

public static class SaveManager {

	public static GameSave currentSave; // salvo atualmente carregado

	// Carrega um arquivo de salvo
	public static void LoadSave(int id) {
		// TODO: ler do arquivo de salvo e guardar em currentSave
	}

	// Guardar num arquivo o salvo atual
	public static void SaveCurrent() {
		// TODO: ler todas as informações atuais 
	}

	// Ao criar um novo jogo
	public static GameSave NewSave() {
		// TODO: criar novo salvo
		return null;
	}

}
