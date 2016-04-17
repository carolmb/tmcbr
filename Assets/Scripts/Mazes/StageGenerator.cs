using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class StageGenerator {

	// Retorna o tile final da fase
	public static Tile CreateStage (Maze[] mazes, MazeGenerator generator, Vector2 beginCoordinates, int initialID) {
		int mazeCount = mazes.Length;

		mazes [0] = generator.Create (initialID, 15, 15, beginCoordinates);
		for (int i = 1, id = initialID + 1; i < mazeCount; i++, id++) {
			mazes [i] = generator.Create(id, 15, 15);

			Tile previousTile = mazes [i - 1].tiles [mazes [i - 1].width - 2, mazes [i].beginTile.y];

			SetTransitions ( 
				mazes [i - 1],
				previousTile,
				mazes[i],
				mazes [i].beginTile,
				Character.RIGHT
			);

		}

		// Cria o final do nível
		int x = mazes[mazeCount - 1].width - 2, y;
		do {
			y = Random.Range (1, mazes[mazeCount - 1 ].height - 1);
		} while (mazes[mazeCount - 1].tiles [x, y].isWall || y%2 == 0);
		Debug.Log (new Vector2 (x, y));
		return mazes[mazeCount - 1].tiles [x, y];
	}

	// Cria uma transição de um tile de um labirinto para outro
	private static void SetTransition (Maze origMaze, Tile origTile, Maze destMaze, Tile destTile, int direction) {
		Transition transition = new Transition (destMaze.id, (int)destTile.x, (int)destTile.y, direction);
		origTile.transition = transition;
	}

	// Transições de ida e volta para os mazes
	// Tile1, Tile2: os tiles ANTERIORES às transições
	public static void SetTransitions(Maze maze1, Tile tile1, Maze maze2, Tile tile2, int direction = Character.RIGHT) {
		Tile origTile;
		int deltaX = 0;
		int deltaY = 0;
		switch (direction) {
		case Character.UP:
			deltaY = 1;
			break;
		case Character.LEFT:
			deltaX = -1;
			break;
		case Character.RIGHT:
			deltaX = 1;
			break;
		case Character.DOWN:
			deltaY = -1;
			break;
		}

		origTile = maze1.tiles [tile1.x + deltaX, tile1.y + deltaY];
		origTile.wallID = 0;
		SetTransition (maze1, origTile, maze2, tile2, direction);

		origTile = maze2.tiles [tile2.x - deltaX, tile2.y - deltaY];
		origTile.wallID = 0;
		SetTransition (maze2, origTile, maze1, tile1, 3 - direction);
	}

}
