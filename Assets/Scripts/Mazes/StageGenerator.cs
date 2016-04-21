using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class StageGenerator {

	// Retorna o tile final da fase
	public static void CreateStage (Maze[] mazes, MazeGenerator generator, int initialID, int initialDir) {
		//int mazeCount = UnityEngine.Random.Range(5, 7);
		int mazeCount = 1;

		for (int i = 0, id = initialID; i < mazeCount; i++, id++) {
			mazes [i] = generator.Create (id, 15, 15);
			mazes [i].Expand (2, 2);
		}

		for (int i = 0, j = 1; i < mazeCount - 1; i++) {
			initialDir = CreateTransition (
				mazes [i], 
				mazes [j], 
				GenerateDir(initialDir)
			);
		}
		/*mazes [0] = generator.Create (initialID, 15, 15, beginCoordinates);
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
		return mazes[mazeCount - 1].tiles [x, y];*/
	}

	//retorna uma direção diferente do argumento dado
	static int GenerateDir(int excusive) {
		List<int> directions = new List<int> ();
		if (excusive != Character.RIGHT) {
			directions.Add (Character.RIGHT);
		}
		if (excusive != Character.LEFT) {
			directions.Add (Character.LEFT);
		}
		if (excusive != Character.UP) {
			directions.Add (Character.UP);
		}
		if (excusive != Character.DOWN) {
			directions.Add (Character.DOWN);
		}
		return directions [UnityEngine.Random.Range (0, directions.Count + 1)];
	}

	//retorna a direção da transição criada 
	public static int CreateTransition(Maze maze1, Maze maze2, int dir) {
		Tile tile1 = GenerateFinalTile (
			maze1, 
			dir, 
			Math.Min(maze1.width, maze2.width), 
			Math.Min(maze1.height, maze2.height)
		);
		Tile tile2 = GenerateInitialTile (
			maze2, 
			tile1, 
			dir, 
			Math.Min(maze1.width, maze2.width), 
			Math.Min(maze1.height, maze2.height)
		);
		SetTransition (maze1, tile1, maze2, tile2, dir);
		return dir;
	}

	static Tile GenerateFinalTile(Maze maze, int dir, int w, int h){
		int x = 0, y = 0;
		Tile finalTile = null;
		switch (dir) {
		case Character.UP:
			x = 0;
			do {
				y = UnityEngine.Random.Range (0, maze.width);
			} while (y % 2 == 0);
			finalTile = maze.tiles [x, y];
			break;
		case Character.LEFT:
			y = 0;
			do {
				x = UnityEngine.Random.Range (0, maze.height);
			} while (x % 2 == 0);
			finalTile = maze.tiles [x, y];
			break;
		case Character.RIGHT:
			y = h - 1;
			do {
				x = UnityEngine.Random.Range (0, maze.height);
			} while (x % 2 == 0);
			finalTile = maze.tiles [x, y];
			break;
		case Character.DOWN:
			x = w - 1;
			do {
				y = UnityEngine.Random.Range (0, maze.width);
			} while (y % 2 == 0);
			finalTile = maze.tiles [x, y];
			break;
		}
		return finalTile;
	}

	static Tile GenerateInitialTile(Maze maze, Tile finalTile, int dir, int w, int h) {
		Vector2 initialTile = new Vector2();
		switch (dir) {
		case Character.UP:
			initialTile.x = w - 1;
			initialTile.y = finalTile.y;
			break;
		case Character.LEFT:
			initialTile.x = finalTile.x;
			initialTile.y = h - 1;
			break;
		case Character.RIGHT:
			initialTile.x = finalTile.x;
			initialTile.y = 0;
			break;
		case Character.DOWN:
			initialTile.x = 0;
			initialTile.y = finalTile.y;
			break;
		}
		return maze.tiles[(int)initialTile.x, (int)initialTile.y];
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
