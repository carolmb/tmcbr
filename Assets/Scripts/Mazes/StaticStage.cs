using UnityEngine;
using System.Collections;
using System;
using System.IO;

public static class StaticStage {

	public static Maze[] LoadStaticStage (string stageName, int id) {
		Maze[] mazes = ReadFromFileBase (stageName, id);
		//ReadFromFileObjects (maze, stageName);
		return mazes;
	}

	//primeira linha tem width e height
	//em seguida 0 para paredes, 1 para caminho livre
	public static Maze[] ReadFromFileBase (string fileName, int id) {
		Maze[] mazes;
		try	{   
			using (StreamReader sr = new StreamReader (Application.dataPath + "/MazeData/"  + fileName + "/base.txt")) { //posteriormente colocar nome relativo do caminho "nomedafase/base.txt"
				int numberOfMazes, width = 0, height = 0;

				string line;
				string[] info;
				line = sr.ReadLine();

				numberOfMazes = Int32.Parse(line);
				mazes = new Maze[numberOfMazes];

				line = sr.ReadLine();
				info = line.Split(' ');
				width = Int32.Parse(info[0]);
				height = Int32.Parse(info[1]);

				for(int k = 0; k < numberOfMazes; k++, id++) {
					Tile[,] tiles = new Tile[width, height];
					for (int i = 0; i < width; i++) {
						line = sr.ReadLine();
						info = line.Split(' ');
						for (int j = 0; j < height; j++) {
							tiles[i, j] = new Tile(i, j);
							tiles[i, j].floorID = Int32.Parse(info[j]); 
						}
					}
					mazes[k] = new Maze(id, fileName, width, height);
					mazes[k].tiles = tiles;
					mazes[k].beginTile = mazes[k].tiles[1, 1];
				}
				width = mazes[mazes.Length - 1].width;
				height = mazes[mazes.Length - 1].height;
				mazes[mazes.Length - 1].endTile = mazes[mazes.Length - 1].tiles[width - 2, height - 2];
		
			}
			return mazes;
		}
		catch (Exception e) {
			Debug.Log ("The file could not be read:");
			Debug.Log (e.Message);
			return null;
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
