using UnityEngine;
using System.Collections;

public static class StageGenerator {

	const int mazeCount = 3;

	public static Maze[] ExpandMazes (Maze[] mazes) {
		for (int i = 0; i < mazeCount; i++) {
			MazeGenerator.ExpandMaze (mazes [i], 2, 2);
			MazeGenerator.CreateEnemies (mazes[i], mazes[i].theme);
		}
		return mazes;
	}

	public static Maze[] CreateStage (string theme) {
		Tile initialTile, finalTile;

		Maze[] mazes;

		mazes = new Maze[mazeCount];

		for (int i = 0; i < mazeCount; i++) {
			mazes [i] = MazeGenerator.CreateMaze (i, theme, 15, 15);
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
			mazes [i].tiles [initialTile.x - 1, initialTile.y].isWall = false;

			if (i != 0) {
				finalTile = mazes [i - 1].tiles [mazes [i].width - 1, initialTile.y];
				finalTile.isWall = false;

				MazeGenerator.SetTransition (finalTile, initialTile, mazes [i - 1], mazes [i]);
			} else {
				finalTile.isWall = false;

				MazeGenerator.SetTransition (finalTile, initialTile, mazes [mazeCount - 1], mazes [i]);
			}

		}
		return ExpandMazes(mazes);
	}




}
