using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class StageGenerator {

	public static Maze[] ExpandMazes (Maze[] mazes, MazeGenerator generator) {
		for (int i = 0; i < mazes.Length; i++) {
			mazes [i].Expand(2, 2);
			generator.CreateEnemies (mazes[i]);
		}
		return mazes;
	}

	// Retorna o tile final da fase
	public static Tile CreateStage (Maze[] mazes, string theme, Vector2 beginCoordinates, int initialID) {
		int mazeCount = mazes.Length;
		MazeGenerator generator = MazeGenerator.GetGenerator (theme);

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

		/*
		do {
			y = Random.Range (1, mazes[mazeCount -1 ].height - 1);
		} while (mazes[mazeCount - 1].tiles [x, y].isWall && y%2 == 0);
		finalTile = mazes[mazeCount-1].tiles [x, y];
		mazes [mazeCount - 1].endTile = finalTile;

		initialTile = mazes [1].tiles [mazes [1].beginTile.x - 1, mazes [1].beginTile.y];

		for (int i = 1; i < mazeCount; i++) {

			//pega o inicio do maze atual
			//cria o final do maze anterior com base no inicio do atual
			//faz transição

			initialTile = mazes [i].beginTile;
			mazes [i].tiles [initialTile.x - 1, initialTile.y].wallID = 0;

			finalTile = mazes [i - 1].tiles [mazes [i].width - 1, initialTile.y];
			finalTile.wallID = 0;

			SetTransition (	mazes [i - 1], 
							finalTile, 
							mazes [i], 
							initialTile);
			SetTransition (	mazes [i], 
							mazes [i].tiles [initialTile.x - 1, initialTile.y], 
							mazes [i - 1],
							mazes[i - 1].tiles[finalTile.x - 1, finalTile.y]);
		
			else {
				finalTile.wallID = 0;
				generator.SetTransition (finalTile, initialTile, mazes [mazeCount - 1], mazes [i]);
				generator.SetTransition (mazes [i].tiles [initialTile.x - 1, initialTile.y], 
				mazes[mazeCount - 1].tiles[finalTile.x - 1, finalTile.y], mazes [i], mazes [mazeCount - 1]);

				Debug.Log (finalTile.coordinates);
				Debug.Log (initialTile.coordinates);
				Debug.Log (" " + mazes [mazeCount - 1].id);
				Debug.Log (" " + mazes [i].id);

			}

		} */
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
