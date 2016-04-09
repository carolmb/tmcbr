using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameSave {

	// TODO: Demais coisas que vão ser salvas

	public int id;
	public Maze[] mazes;
	Bag bag;

	public GameSave(){
		int mazeCount = 1;
		mazes = new Maze[mazeCount];
		for (int i = 0; i < mazeCount; i++) {
			mazes [i] = MazeGenerator.CreateMaze(i, "Hall", 15, 15);
		}

	}

}
