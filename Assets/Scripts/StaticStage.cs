using UnityEngine;
using System.Collections;
using System;
using System.IO;

public static class StaticStage {



	static Maze LoadStaticStage (string nameStage, int id) {
		Maze maze = new Maze (id, nameStage, 0, 0);
		ReadFromFileBase (maze, nameStage);
		ReadFromFileObjects (maze, nameStage);
		return maze;
	}

	//primeira linha tem width e height
	//em seguida 0 para paredes, 1 para caminho livre
	static void ReadFromFileBase (Maze maze, string nameFile) {
		try
		{   // Open the text file using a stream reader.
			using (StreamReader sr = new StreamReader("base" + nameFile + ".txt"))
			{
				// Read the stream to a string, and write the string to the console.
				string line = sr.ReadToEnd();
			}
		}
		catch (Exception e)
		{
			Debug.Log ("The file could not be read:");
			Debug.Log (e.Message);
		}
	}

	static void ReadFromFileObjects (Maze maze, string nameFile) {
	
	}
}
