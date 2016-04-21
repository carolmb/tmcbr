using UnityEngine;
using System.Collections;
using System;
using System.IO;

public static class StaticStage {

	public static Maze[] LoadStaticStage (string stageName, int id) {
		Maze[] mazes = new Maze[1];
		mazes [0] = new Maze (id, stageName, 0, 0);

		ReadFromFileBase (mazes[0]);
		ReadFromFileWalls (mazes[0]);
		return mazes;
	}

	public static void ReadFromFileBase (Maze maze) {
		try	{   
			using (StreamReader sr = new StreamReader (Application.dataPath + "/MazeData/"  + maze.theme + "/base.txt")) { 
				string line = sr.ReadLine();
				string[] info = line.Split(' ');
				int width = Int32.Parse(info[0]);
				int height = Int32.Parse(info[1]);

				Tile[,] tiles = new Tile[width, height];
				for (int i = 0; i < width; i++) {
					line = sr.ReadLine();
					info = line.Split(' ');
					for (int j = 0; j < height; j++) {
						tiles[j, i] = new Tile(j, i);
						tiles[j, i].floorID = Int32.Parse(info[j]); 
					}
				}
				maze.tiles = tiles;
				line = sr.ReadLine();
				info = line.Split(' ');
				int x = Int32.Parse(info[0]);
				int y = Int32.Parse(info[1]);
				maze.beginTile = maze.tiles[x, y];
			}
		}
		catch (Exception e) {
			Debug.Log ("The file could not be read:");
			Debug.Log (e.Message);
		}
	}

	//nome do objeto seguido das coordenadas x e y
	static void ReadFromFileWalls (Maze maze) {
		try { 
			using (StreamReader sr = new StreamReader (Application.dataPath + "/MazeData/"  + maze.theme + "/walls.txt")) { // "nomedafase/objectos.txt"
				string line = sr.ReadLine();
				string[] info = line.Split(' ');
				int width = Int32.Parse(info[0]);
				int height = Int32.Parse(info[1]);

				for (int i = 0; i < width; i++) {
					line = sr.ReadLine();
					info = line.Split(' ');
					for (int j = 0; j < height; j++) {
						maze.tiles[j, i].wallID = Int32.Parse(info[j]); 
					}
				}
			}
		}
		catch (Exception e)	{
			Debug.Log ("The file could not be read:");
			Debug.Log (e.Message);
		}
	}

	static void ReadFromFileObstacles (Maze maze) {
		try { 
			using (StreamReader sr = new StreamReader (Application.dataPath + "/MazeData/"  + maze.theme + "/obstacles.txt")) { 
				string line = sr.ReadLine();
				string[] info = line.Split(' ');
				int width = Int32.Parse(info[0]);
				int height = Int32.Parse(info[1]);

				for (int i = 0; i < width; i++) {
					line = sr.ReadLine();
					info = line.Split(' ');
					for (int j = 0; j < height; j++) {
						maze.tiles[j, i].obstacleID = Int32.Parse(info[j]); 
					}
				}
			}
		}
		catch (Exception e)	{
			Debug.Log ("The file could not be read:");
			Debug.Log (e.Message);
		}
	}
}
