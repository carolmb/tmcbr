using UnityEngine;
using System.Collections;
using System;
using System.IO;

public static class StaticStage {

	private static string dataPath {
		get { return "MazeData/"; } 
	}

	public static Stage LoadStaticStage (string stageName, int id) {
		Stage stage = ReadFromFileBase (stageName, id);
		ReadFromFileFloor (stage.mazes[0]);
		ReadFromFileWalls (stage.mazes[0]);
		ReadFromFileObstacles (stage.mazes[0]);
		ReadFromFileTypes (stage.mazes [0]);
		return stage;
	}

	public static Stage ReadFromFileBase(string stageName, int id) {
		TextAsset file = Resources.Load<TextAsset> (dataPath + stageName + "/base");
		if (file == null) {
			Debug.Log ("The file could not be read: " + dataPath + stageName + "/base");
			return null;
		}
		string[] lines = file.text.Split("\n"[0]);

		string line = lines[0];
		string[] info = line.Split(' ');
		int width = Int32.Parse(info[0]);
		int height = Int32.Parse(info[1]);
		Maze maze = new Maze (id, stageName, width, height);
		Stage stage = new Stage(new Maze[] {maze} );

		for(int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				maze.tiles[i,j] = new Tile(i, j);
			}
		}

		line = lines[1];
		info = line.Split(' ');
		int x = Int32.Parse(info[0]);
		int y = Int32.Parse(info[1]);
		int d = Int32.Parse(info[2]);
		int s = Int32.Parse(info[3]);
		stage.beginTile = maze.tiles[x, y];
		stage.beginDir = d;
		stage.beginSize = s;

		line = lines[2];
		info = line.Split(' ');
		x = Int32.Parse(info[0]);
		y = Int32.Parse(info[1]);
		d = Int32.Parse(info[2]);
		s = Int32.Parse(info[3]);
		stage.endTile = maze.tiles[x, y];
		stage.endDir = d;
		stage.endSize = s;

		return stage;
	}

	public static void ReadFromFileFloor (Maze maze) {
		TextAsset file = Resources.Load<TextAsset> (dataPath + maze.theme + "/floor");
		if (file == null) {
			Debug.Log ("The file could not be read: " + dataPath + maze.theme + "/floor");
			return;
		}
		string[] lines = file.text.Split("\n"[0]);

		for (int i = 0; i < maze.height; i++) {
			string line = lines [i];
			string [] info = line.Split(' ');
			for (int j = 0; j < maze.width; j++) {
				maze.tiles[j, (maze.height - 1) - i].floorID = Int32.Parse(info[j]); 
			}
		}
	}

	//nome do objeto seguido das coordenadas x e y
	static void ReadFromFileWalls (Maze maze) {
		TextAsset file = Resources.Load<TextAsset> (dataPath  + maze.theme + "/walls");
		if (file == null) {
			Debug.Log ("The file could not be read: " + dataPath + maze.theme + "/walls");
			return;
		}
		string[] lines = file.text.Split("\n"[0]);

		for (int i = 0; i < maze.height; i++) {
			string line = lines [i];
			string [] info = line.Split(' ');
			for (int j = 0; j < maze.width; j++) {
				maze.tiles[j, (maze.height - 1) - i].wallID = Int32.Parse(info[j]); 
			}
		}
	}

	static void ReadFromFileObstacles (Maze maze) {
		TextAsset file = Resources.Load<TextAsset> (dataPath  + maze.theme + "/obstacles");
		if (file == null) {
			Debug.Log ("The file could not be read: " + dataPath + maze.theme + "/obstacles");
			return;
		}
		string[] lines = file.text.Split("\n"[0]);

		for (int i = 0; i < maze.height; i++) {
			string line = lines [i];
			string [] info = line.Split(' ');
			for (int j = 0; j < maze.width; j++) {
				maze.tiles[j, (maze.height - 1) - i].obstacleID = Int32.Parse(info[j]); 
			}
		}
	}

	static void ReadFromFileTypes (Maze maze) {
		TextAsset file = Resources.Load<TextAsset> (dataPath  + maze.theme + "/types");
		if (file == null) {
			Debug.Log ("The file could not be read: " + dataPath + maze.theme + "/types");
			return;
		}
		string[] lines = file.text.Split("\n"[0]);

		for (int i = 0; i < maze.height; i++) {
			string line = lines [i];
			string [] info = line.Split(' ');
			for (int j = 0; j < maze.width; j++) {
				maze.tiles[j, (maze.height - 1) - i].type = Int32.Parse(info[j]); 
			}
		}
	}

}
