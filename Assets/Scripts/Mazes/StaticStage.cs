using UnityEngine;
using System.Collections;
using System;
using System.IO;

public static class StaticStage {

	public static Maze[] LoadStaticStage (string stageName, int id) {
		Maze[] mazes = new Maze[1];
		mazes [0] = ReadFromFileBase (stageName, id);
		ReadFromFileFloor (mazes [0]);
		ReadFromFileWalls (mazes[0]);
		ReadFromFileObstacles (mazes[0]);
		return mazes;
	}

	public static Maze ReadFromFileBase(string stageName, int id) {
		try { 
			using (StreamReader sr = new StreamReader (Application.dataPath + "/MazeData/"  + stageName + "/base.txt")) { // "nomedafase/objectos.txt"
				string line = sr.ReadLine();
				string[] info = line.Split(' ');
				int width = Int32.Parse(info[0]);
				int height = Int32.Parse(info[1]);
				Maze maze = new Maze (id, stageName, width, height);

				for(int i = 0; i < width; i++) {
					for(int j = 0; j < height; j++) {
						maze.tiles[i,j] = new Tile(i, j);
					}
				}

				line = sr.ReadLine();
				info = line.Split(' ');
				int x = Int32.Parse(info[0]);
				int y = Int32.Parse(info[1]);
				maze.beginTile = maze.tiles[x, y];

				line = sr.ReadLine();
				info = line.Split(' ');
				x = Int32.Parse(info[0]);
				y = Int32.Parse(info[1]);
				maze.endTile = maze.tiles[x, y];

				return maze;
			}
		} catch (IOException) {
			Debug.Log ("The file could not be read:");
			return null;
		}
	}

	public static void ReadFromFileFloor (Maze maze) {
		try	{   
			using (StreamReader sr = new StreamReader (Application.dataPath + "/MazeData/"  + maze.theme + "/floor.txt")) { 
				for (int i = 0; i < maze.height; i++) {
					string line = sr.ReadLine();
					string [] info = line.Split(' ');
					for (int j = 0; j < maze.width; j++) {
						maze.tiles[j, (maze.height - 1) - i].floorID = Int32.Parse(info[j]); 
					}
				}
			}
		} catch (IOException) {
			Debug.Log ("The file could not be read:");
		}
	}

	//nome do objeto seguido das coordenadas x e y
	static void ReadFromFileWalls (Maze maze) {
		try { 
			using (StreamReader sr = new StreamReader (Application.dataPath + "/MazeData/"  + maze.theme + "/walls.txt")) { // "nomedafase/objectos.txt"
				for (int i = 0; i < maze.height; i++) {
					string line = sr.ReadLine();
					string [] info = line.Split(' ');
					for (int j = 0; j < maze.width; j++) {
						maze.tiles[j, (maze.height - 1) - i].wallID = Int32.Parse(info[j]); 
					}
				}
			}
		} catch (IOException) {
			Debug.Log ("The file could not be read:");
		}
	}

	static void ReadFromFileObstacles (Maze maze) {
		try { 
			using (StreamReader sr = new StreamReader (Application.dataPath + "/MazeData/"  + maze.theme + "/obstacles.txt")) { 
				for (int i = 0; i < maze.height; i++) {
					string line = sr.ReadLine();
					string [] info = line.Split(' ');
					for (int j = 0; j < maze.width; j++) {
						maze.tiles[j, (maze.height - 1) - i].obstacleID = Int32.Parse(info[j]); 
					}
				}
			}
		} catch (IOException) {
			Debug.Log ("The file could not be read:");
		}
	}
}
