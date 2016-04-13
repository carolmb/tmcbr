using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameSave {

	public string name;
	public Maze[] mazes;
	public Transition transition;
	public Bag bag;
	public int lifePoints;

	void CreateTransition(Maze maze, Tile tile) {
		tile.isWall = false;
		tile.obstacle = -1;

		maze.tiles [tile.x, tile.y + 1].isWall = false;
		maze.tiles [tile.x, tile.y + 1].obstacle = -1;
		maze.tiles [tile.x - 1, tile.y].isWall = false;
		maze.tiles [tile.x - 1, tile.y].obstacle = -1;
	}

	public GameSave() {
		Tile initialTileTop, initialTileBottom, finalTileTop, finalTileBottom;

		int mazeCount = 3;
		mazes = new Maze[mazeCount];

		for (int i = 0; i < mazeCount; i++) {
			mazes [i] = MazeGenerator.CreateMaze (i, "Hall", 15, 15);
		}

		int x = mazes[mazeCount - 1].width - 3, y;
		do {
			y = Random.Range (1, mazes[mazeCount -1 ].height - 1);
		} while (mazes[mazeCount - 1].tiles [x, y].isWall && y%2 == 0);
		finalTileBottom = mazes[mazeCount-1].tiles [x + 2, y];
		CreateTransition (mazes [mazeCount - 1], finalTileBottom);

		for (int i = 0; i < mazeCount; i++) {

			//pega o inicio do maze atual
			//cria o final do maze anterior com base no inicio do atual
			//faz transição
		
			initialTileBottom = mazes [i].beginMaze;
			mazes [i].tiles [initialTileBottom.x - 1, initialTileBottom.y].isWall = false;
			mazes [i].tiles [initialTileBottom.x - 1, initialTileBottom.y].obstacle = -1;
			mazes [i].tiles [initialTileBottom.x - 2, initialTileBottom.y].isWall = false;
			mazes [i].tiles [initialTileBottom.x - 2, initialTileBottom.y].obstacle = -1;
			mazes [i].tiles [initialTileBottom.x - 1, initialTileBottom.y + 1].isWall = false;
			mazes [i].tiles [initialTileBottom.x - 2, initialTileBottom.y + 1].isWall = false;

			if (i != 0) {
				finalTileBottom = mazes [i - 1].tiles [mazes [i].width - 1, initialTileBottom.y];
				mazes [i - 1].tiles [mazes [i].width - 1, initialTileBottom.y + 1].isWall = false;
				mazes [i - 1].tiles [mazes [i].width - 1, initialTileBottom.y + 1].obstacle = -1;
				mazes [i - 1].tiles [mazes [i].width - 2, initialTileBottom.y].isWall = false;
				mazes [i - 1].tiles [mazes [i].width - 2, initialTileBottom.y].obstacle = -1;
				mazes [i - 1].tiles [mazes [i].width - 2, initialTileBottom.y + 1].isWall = false;
				mazes [i - 1].tiles [mazes [i].width - 2, initialTileBottom.y + 1].obstacle = -1;

				finalTileBottom.isWall = false;

				MazeGenerator.SetTransition (finalTileBottom, initialTileBottom, mazes [i - 1], mazes [i]);
				//MazeGenerator.SetTransition ();
			} else {
				mazes [mazeCount - 1].tiles [mazes [i].width - 2, initialTileBottom.y].isWall = false;
				mazes [mazeCount - 1].tiles [mazes [i].width - 2, initialTileBottom.y].obstacle = -1;
				mazes [mazeCount - 1].tiles [mazes [i].width - 2, initialTileBottom.y + 1].isWall = false;
				mazes [mazeCount - 1].tiles [mazes [i].width - 2, initialTileBottom.y + 1].obstacle = -1;
				finalTileBottom.isWall = false;

				MazeGenerator.SetTransition (finalTileBottom, initialTileBottom, mazes [mazeCount - 1], mazes [i]);
			}
			Debug.Log (finalTileBottom.coordinates);
		}

		transition = new Transition(0, mazes[0].beginMaze.x, mazes[0].beginMaze.y, 0);

		bag = new Bag ();
		lifePoints = 5;
	}

}
