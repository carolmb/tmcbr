using UnityEngine;
using System.Collections;

public static class StageGenerator {

	const int mazeCount = 3;

	public static Maze[] ExpandMazes (Maze[] mazes, MazeGenerator generator) {
		for (int i = 0; i < mazeCount; i++) {
			generator.ExpandMaze (mazes [i], 2, 2);
			generator.CreateEnemies (mazes[i]);
		}
		return mazes;
	}

	public static Maze[] CreateStage (string theme) {
		Tile initialTile, finalTile;

		Maze[] mazes = new Maze[mazeCount];
		MazeGenerator generator = MazeGenerator.GetGenerator (theme, 15, 15);

		for (int i = 0; i < mazeCount; i++) {
			mazes [i] = generator.Create(i);
		}

		int x = mazes[mazeCount - 1].width - 1, y;
		do {
			y = Random.Range (1, mazes[mazeCount -1 ].height - 1);
		} while (mazes[mazeCount - 1].tiles [x, y].isWall && y%2 == 0);
		finalTile = mazes[mazeCount-1].tiles [x, y];

		for (int i = 0; i < mazeCount; i++) {

			//pega o inicio do maze atual
			//cria o final do maze anterior com base no inicio do atual
			//faz transição

			initialTile = mazes [i].beginMaze;

			mazes [i].tiles [initialTile.x - 1, initialTile.y].wallID = 0;

			if (i != 0) {
				finalTile = mazes [i - 1].tiles [mazes [i].width - 1, initialTile.y];
				finalTile.wallID = 0;

				generator.SetTransition (finalTile, initialTile, mazes [i - 1], mazes [i]);
				generator.SetTransition (mazes [i].tiles [initialTile.x - 1, initialTile.y], 
				mazes[i - 1].tiles[finalTile.x - 1, finalTile.y], mazes [i], mazes [i - 1]);
			} else {
				finalTile.wallID = 0;

				generator.SetTransition (finalTile, initialTile, mazes [mazeCount - 1], mazes [i]);
				generator.SetTransition (mazes [i].tiles [initialTile.x - 1, initialTile.y], 
				mazes[mazeCount - 1].tiles[finalTile.x - 1, finalTile.y], mazes [i], mazes [mazeCount - 1]);
			}

		}
		return ExpandMazes(mazes, generator);
	}

}
