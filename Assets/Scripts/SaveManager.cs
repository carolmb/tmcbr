using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System;

public static class SaveManager {

	public static GameSave currentSave; // salvo atualmente carregado

	public static string[] saveList;
	public static int maxSaves = 3;

	// o tempo (em segundos) que se passou desde o início da aplicação até quando o salvo foi carregado
	public static float loadTime = 0f; 

	// o tempo de jogo total até agora
	public static float currentPlayTime {
		get {
			return currentSave.playTime + (Time.time - loadTime);
		}
	}

	// Ao criar um novo jogo
	public static void NewGame () {
		currentSave = new GameSave ();
		currentSave.name = "";
		loadTime = Time.time;
	}

	// Guardar num arquivo o salvo atual
	public static void SaveGame (int id, string name) {
		currentSave.name = name;
		currentSave.playTime = currentPlayTime;
		saveList [id] = name;
		StoreSave (id);
		StoreList ();
	}

	// Restaurar a lista de salvos
	public static void LoadList () {
		string namePath = Application.persistentDataPath + "/saveList";
		if (File.Exists (namePath)) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(namePath, FileMode.Open);
			try {
				saveList = (string[]) bf.Deserialize (file);
			} catch(SerializationException) {
				saveList = new string[maxSaves];
			} catch(TypeLoadException) {
				saveList = new string[maxSaves];
			}
			file.Close ();
		} else {
			saveList = new string[maxSaves];
		}
	}

	// Restaurar um salvo		
	public static void LoadSave (int id) {
		string namePath = Application.persistentDataPath + "/save" + id;
		if (File.Exists (namePath)) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(namePath, FileMode.Open);
			try {
				currentSave = (GameSave) bf.Deserialize (file);
			} catch(SerializationException) {
				currentSave = new GameSave ();
			} catch(TypeLoadException) {
				currentSave = new GameSave ();
			}
			file.Close ();
		} else {
			currentSave = new GameSave ();
		}
		loadTime = Time.time;
	}

	// Guardar a lista num arquivo
	private static void StoreList () {
		BinaryFormatter bf = new BinaryFormatter ();
		string namePath = Application.persistentDataPath + "/savelist";
		FileStream file = File.Create (namePath);
		bf.Serialize (file, saveList);
		file.Close ();
	}

	// Guardar um salvo num arquivo
	private static void StoreSave (int id) {
		BinaryFormatter bf = new BinaryFormatter ();
		string namePath = Application.persistentDataPath + "/save" + id;
		FileStream file = File.Create (namePath);
		bf.Serialize (file, currentSave);
		file.Close ();
	}

}
