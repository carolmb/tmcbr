using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameSave {

	// TODO: Demais coisas que vão ser salvas

	public int id;
	public Maze[] mazes;
	Bag bag;

	public GameSave(){
		mazes = new Maze[3];
		mazes [0] = MazeGenerator.CreateMaze("hall", 7, 7);
		mazes [1] = MazeGenerator.CreateMaze("hall", 8, 8);
		mazes [2] = MazeGenerator.CreateMaze("hall", 9, 9);
	}
}
