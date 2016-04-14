using UnityEngine;
using System.Collections;
using System;
using System.IO;

public static class StaticStage {



	static Maze LoadStaticStage (string stageName, int id) {
		Maze maze = new Maze (id, stageName, 0, 0);
		ReadFromFileBase (maze, stageName);
		//ReadFromFileObjects (maze, stageName);
		return maze;
	}

	//primeira linha tem width e height
	//em seguida 0 para paredes, 1 para caminho livre
	static void ReadFromFileBase (Maze maze, string fileName) {
		try	{   
			using (StreamReader sr = new StreamReader("base" + fileName + ".txt")) { //posteriormente colocar nome relativo do caminho "nomedafase/base.txt"
				int width = sr.Read();
				int height = sr.Read();
				int value;
				Tile[,] tiles = new Tile[width,height];
				for (int i = 0; i < width; i++) {
					for (int j = 0; j < height; j++) {
						value = sr.Read();
						tiles[i, j] = new Tile(i, j);
						tiles[i, j].isWall = value == 0 ? false : true; 
					}
				}
			}
		}
		catch (Exception e) {
			Debug.Log ("The file could not be read:");
			Debug.Log (e.Message);
		}
	}

	//nome do objeto seguido na proxima linha das coordenadas x e y
	static void ReadFromFileObjects (Maze maze, string fileName) {
		try { 
			using (StreamReader sr = new StreamReader ("objects" + fileName + ".txt")) { // "nomedafase/objectos.txt"
				string objectName;
				int x, y;
				while (!sr.EndOfStream) {
					objectName = sr.ReadLine();
					x = sr.Read();
					y = sr.Read();
					maze.tiles[x, y].objectName = objectName;
				}

			}
		}
		catch (Exception e)	{
			Debug.Log ("The file could not be read:");
			Debug.Log (e.Message);
		}
	}

	static void ReadFromFileObstacles (Maze maze, string fileName) {
		try { 
			using (StreamReader sr = new StreamReader ("obstacles" + fileName + ".txt")) { // "nomedafase/objectos.txt"
				int obstacle, x, y;
				while (!sr.EndOfStream) {
					obstacle = sr.Read();
					x = sr.Read();
					y = sr.Read();
					maze.tiles[x, y].obstacle = obstacle;
				}
			}
		}
		catch (Exception e)	{
			Debug.Log ("The file could not be read:");
			Debug.Log (e.Message);
		}
	}
}
