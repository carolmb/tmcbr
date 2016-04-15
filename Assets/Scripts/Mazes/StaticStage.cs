using UnityEngine;
using System.Collections;
using System;
using System.IO;

public static class StaticStage {

	public static Maze LoadStaticStage (string stageName, int id) {
		Maze maze = new Maze (id, stageName, 0, 0);
		ReadFromFileBase (maze, stageName);
		//ReadFromFileObjects (maze, stageName);
		return maze;
	}

	//primeira linha tem width e height
	//em seguida 0 para paredes, 1 para caminho livre
	public static void ReadFromFileBase (Maze maze, string fileName) {
		try	{   
			using (StreamReader sr = new StreamReader (Application.dataPath + "/MazeData/"  + fileName + "/base.txt")) { //posteriormente colocar nome relativo do caminho "nomedafase/base.txt"
				string line;
				string[] info;
				line = sr.ReadLine();
				info = line.Split(' ');
				int width = Int32.Parse(info[0]);
				int height = Int32.Parse(info[1]);
				Tile[,] tiles = new Tile[width,height];
				for (int i = 0; i < width; i++) {
					line = sr.ReadLine();
					info = line.Split(' ');
					for (int j = 0; j < height; j++) {
						tiles[i, j] = new Tile(i, j);
						tiles[i, j].wallID = Int32.Parse(info[j]); 
					}
				}
			}
		}
		catch (Exception e) {
			Debug.Log ("The file could not be read:");
			Debug.Log (e.Message);
		}
	}

	//nome do objeto seguido das coordenadas x e y
	static void ReadFromFileObjects (Maze maze, string fileName) {
		try { 
			using (StreamReader sr = new StreamReader (Application.dataPath + "/MazeData/"  + fileName + "/objects.txt")) { // "nomedafase/objectos.txt"
				string line, objectName;
				string[] info;
				int x, y;
				while (!sr.EndOfStream) {
					line = sr.ReadLine();
					info = line.Split(' ');
					objectName = info[0];
					x = Int32.Parse(info[1]);
					y = Int32.Parse(info[2]);
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
			using (StreamReader sr = new StreamReader (Application.dataPath + "/MazeData/"  + fileName + "/obstacles.txt")) { // "nomedafase/objectos.txt"
				int obstacle, x, y;
				string line;
				string[] info;
				while (!sr.EndOfStream) {
					line = sr.ReadLine();
					info = line.Split(' ');
					obstacle = Int32.Parse(info[0]);
					x = Int32.Parse(info[1]);
					y = Int32.Parse(info[2]);
					maze.tiles[x, y].obstacleID = obstacle;
				}
			}
		}
		catch (Exception e)	{
			Debug.Log ("The file could not be read:");
			Debug.Log (e.Message);
		}
	}
}
