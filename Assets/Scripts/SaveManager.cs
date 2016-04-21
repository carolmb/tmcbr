using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

public static class SaveManager {

	public static GameSave currentSave; // salvo atualmente carregado

	public static GameSave[] allSaves;
	public static int maxSaves = 3;

	// o tempo (em segundos) que se passou desde o início da aplicação até quando o salvo foi carregado
	public static float loadTime = 0f; 

	// o tempo de jogo total até agora
	public static float currentPlayTime {
		get {
			return currentSave.playTime + (Time.time - loadTime); 
		}
	}

	// Guardar num arquivo o salvo atual
	public static void SaveGame(int id, string name) {
		currentSave.name = name;
		currentSave.playTime = currentPlayTime;
		allSaves [id] = currentSave;
		StoreSaves ();
	}

	// Carregar um jogo
	public static void LoadGame(int id) {
		currentSave = allSaves [id];
		loadTime = Time.time;
	}

	// Ao criar um novo jogo
	public static void NewGame() {
		currentSave = new GameSave();
		loadTime = Time.time;
	}
				
	public static void LoadSaves() {
		string namePath = Application.persistentDataPath + "/" + "saves";
		if (File.Exists (namePath)) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(namePath, FileMode.Open);
			try {
				allSaves = (GameSave[]) bf.Deserialize(file);
			} catch(SerializationException) {
				allSaves = new GameSave[maxSaves];
			}
			file.Close();
		} else {
			allSaves = new GameSave[maxSaves];
		}
	}

	private static void StoreSaves() {
		BinaryFormatter bf = new BinaryFormatter();
		string namePath = Application.persistentDataPath + "/" + "saves";
		FileStream file = File.Create (namePath);
		bf.Serialize (file, allSaves);
		file.Close ();
	}

}
