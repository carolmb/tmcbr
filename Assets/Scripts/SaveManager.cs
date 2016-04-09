using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveManager {

	public static GameSave currentSave; // salvo atualmente carregado
	public static string currentSaveName;

	// Carrega um arquivo de salvo
	public static void LoadSave(int id) {
		// TODO: ler do arquivo de salvo e guardar em currentSave
	}

	// Guardar num arquivo o salvo atual
	public static void SaveGame(string gsName, GameSave gs) { 
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/" + gsName + ".maze");
		bf.Serialize (file, gs);
		file.Close ();
		currentSave = gs;
	}

	public static bool LoadGame(string loadGameName) {
		GameSave gs = null;
		if(File.Exists(Application.persistentDataPath + "/" + loadGameName + ".maze")) {
			BinaryFormatter bf = new BinaryFormatter();
			string namePath = Application.persistentDataPath + "/" + loadGameName + ".maze";
			FileStream file = File.Open(namePath, FileMode.Open);
			gs = (GameSave)bf.Deserialize(file);
			file.Close();
			currentSave = gs;
			return true;
		}
		return false;
	}

	// Ao criar um novo jogo
	public static void NewSave(string gameName) {
		GameSave gs = new GameSave();
		SaveGame (gameName, gs);
		// TODO: criar novo salvo
	}

}
