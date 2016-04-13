using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameSave {

	public string name;
	public Maze[] mazes;
	public Transition transition;
	public Bag bag;
	public int lifePoints;

	public GameSave() {
		Tile initialTile, finalTile;

		int mazeCount = 3;
		mazes = new Maze[mazeCount];

		for (int i = 0; i < mazeCount; i++) {
			mazes [i] = MazeGenerator.CreateMaze (i, "Hall", 15, 15);
		}

		int x = mazes[mazeCount - 1].width - 3, y;
		do {
			y = Random.Range (1, mazes[mazeCount -1 ].height - 1);
		} while (mazes[mazeCount - 1].tiles [x, y].isWall && y%2 == 0);
		finalTile = mazes[mazeCount-1].tiles [x + 2, y];
		mazes [mazeCount - 1].tiles [x + 2, y].isWall = false;
		mazes [mazeCount - 1].tiles [x + 2, y].obstacle = -1;
		mazes [mazeCount - 1].tiles [x + 2, y + 1].isWall = false;
		mazes [mazeCount - 1].tiles [x + 2, y + 1].obstacle = -1;
		mazes [mazeCount - 1].tiles [x + 1, y].isWall = false;
		mazes [mazeCount - 1].tiles [x + 1, y + 1].isWall = false;

		for (int i = 0; i < mazeCount; i++) {

			//pega o inicio do maze atual
			//cria o final do maze anterior com base no inicio do atual
			//faz transição
		
			initialTile = mazes [i].beginMaze;
			mazes [i].tiles [initialTile.x - 1, initialTile.y].isWall = false;
			mazes [i].tiles [initialTile.x - 1, initialTile.y].obstacle = -1;
			mazes [i].tiles [initialTile.x - 2, initialTile.y].isWall = false;
			mazes [i].tiles [initialTile.x - 2, initialTile.y].obstacle = -1;
			mazes [i].tiles [initialTile.x - 1, initialTile.y + 1].isWall = false;
			mazes [i].tiles [initialTile.x - 2, initialTile.y + 1].isWall = false;

			if (i != 0) {
				finalTile = mazes [i - 1].tiles [mazes [i].width - 1, initialTile.y];
				mazes [i - 1].tiles [mazes [i].width - 1, initialTile.y + 1].isWall = false;
				mazes [i - 1].tiles [mazes [i].width - 1, initialTile.y + 1].obstacle = -1;
				mazes [i - 1].tiles [mazes [i].width - 2, initialTile.y].isWall = false;
				mazes [i - 1].tiles [mazes [i].width - 2, initialTile.y].obstacle = -1;
				mazes [i - 1].tiles [mazes [i].width - 2, initialTile.y + 1].isWall = false;
				mazes [i - 1].tiles [mazes [i].width - 2, initialTile.y + 1].obstacle = -1;

				finalTile.isWall = false;

				MazeGenerator.SetTransition (finalTile, initialTile, mazes [i - 1], mazes [i]);
			} else {
				mazes [mazeCount - 1].tiles [mazes [i].width - 2, initialTile.y].isWall = false;
				mazes [mazeCount - 1].tiles [mazes [i].width - 2, initialTile.y].obstacle = -1;
				mazes [mazeCount - 1].tiles [mazes [i].width - 2, initialTile.y + 1].isWall = false;
				mazes [mazeCount - 1].tiles [mazes [i].width - 2, initialTile.y + 1].obstacle = -1;
				finalTile.isWall = false;

				MazeGenerator.SetTransition (finalTile, initialTile, mazes [mazeCount - 1], mazes [i]);
			}
			Debug.Log (finalTile.coordinates);
		}

		transition = new Transition(0, mazes[0].beginMaze.x, mazes[0].beginMaze.y, 0);

		bag = new Bag ();
		bag.itemIDs [0] = 0;
		lifePoints = 5;
	}

}
