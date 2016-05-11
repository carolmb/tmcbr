using UnityEngine;
using System.Collections;
using System;

public class StaticStage : Stage {

	public StaticMaze[] mazes;

	public override Maze[] GetMazes() {
		return mazes;
	}

	public Maze maze {
		get { return mazes [0]; }
	}

	private static string dataPath {
		get { return "MazeData/"; } 
	}

	public StaticStage(int i, string name) : base(i) {
		ReadFromFileBase (name, i);
		ReadFromFileFloor (mazes[0]);
		ReadFromFileWalls (mazes[0]);
		ReadFromFileObstacles (mazes[0]);
		ReadFromFileTypes (mazes [0]);
		endIndex = i;
	}

	void ReadFromFileBase(string stageName, int id) {
		TextAsset file = Resources.Load<TextAsset> (dataPath + stageName + "/base");
		if (file == null) {
			Debug.Log ("The file could not be read: " + dataPath + stageName + "/base");
			return;
		}
		string[] lines = file.text.Split("\n"[0]);

		string line = lines[0];
		string[] info = line.Split(' ');
		int width = Int32.Parse(info[0]);
		int height = Int32.Parse(info[1]);
		StaticMaze maze = new StaticMaze (id, width, height, stageName);
		mazes = new StaticMaze[] { maze };

		for(int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				maze.tiles[i,j] = new Tile(i, j);
			}
		}

		for(int i = 1; i < lines.Length; i++) {
			line = lines [i];
			info = line.Split(' ');
			int x = Int32.Parse(info[0]);
			int y = Int32.Parse(info[1]);
			int d = Int32.Parse(info[2]);
			int s = Int32.Parse(info[3]);
			AddTransition (maze, x, y, d, s);
		}

	}

	void ReadFromFileFloor (Maze maze) {
		TextAsset file = Resources.Load<TextAsset> (dataPath + maze.GetTheme() + "/floor");
		if (file == null) {
			Debug.Log ("The file could not be read: " + dataPath + maze.GetTheme() + "/floor");
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
	void ReadFromFileWalls (Maze maze) {
		TextAsset file = Resources.Load<TextAsset> (dataPath  + maze.GetTheme() + "/walls");
		if (file == null) {
			Debug.Log ("The file could not be read: " + dataPath + maze.GetTheme() + "/walls");
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

	void ReadFromFileObstacles (Maze maze) {
		TextAsset file = Resources.Load<TextAsset> (dataPath  + maze.GetTheme() + "/obstacles");
		if (file == null) {
			Debug.Log ("The file could not be read: " + dataPath + maze.GetTheme() + "/obstacles");
			return;
		}
		string[] lines = file.text.Split("\n"[0]);

		for (int i = 0; i < maze.height; i++) {
			string line = lines [i];
			string [] info = line.Split(' ');
			for (int j = 0; j < maze.width; j++) {
				int id = Int32.Parse (info [j]);
				if (id > 0) {
					maze.tiles [j, (maze.height - 1) - i].obstacle = "obstacle" + id;
				} else {
					maze.tiles [j, (maze.height - 1) - i].obstacle = "";
				}
			}
		}
	}

	void ReadFromFileTypes (Maze maze) {
		TextAsset file = Resources.Load<TextAsset> (dataPath  + maze.GetTheme() + "/types");
		if (file == null) {
			Debug.Log ("The file could not be read: " + dataPath + maze.GetTheme() + "/types");
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
