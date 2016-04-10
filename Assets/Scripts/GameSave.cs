using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameSave {

	public string name;
	public Maze[] mazes;
	public Transition transition;
	public Bag bag;

	public GameSave() {
		int mazeCount = 1;
		mazes = new Maze[mazeCount];
		for (int i = 0; i < mazeCount; i++) {
			mazes [i] = MazeGenerator.CreateMaze(i, "Hall", 15, 15);
		}
		transition = mazes [0].FindTransitionInX (mazes [0].width - 1);

		bag = new Bag ();
	}

}
