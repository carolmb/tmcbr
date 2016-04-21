using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class StageGenerator {

	// Retorna o tile final da fase
	public static void CreateStage (Maze[] mazes, MazeGenerator generator, int initialID, int initialDir) {
		//int mazeCount = UnityEngine.Random.Range(5, 7);
		int mazeCount = mazes.Length;
		for (int i = 0, id = initialID; i < mazeCount; i++, id++) {
			mazes [i] = generator.Create (id, 15, 15);
			mazes [i].Expand (2, 2);
		}
		for (int i = 0, j = 1; j < mazeCount; i++, j++) {
			initialDir = GenerateDir (initialDir);
			SetTransitions (
				mazes [i], 
				mazes [j], 
				initialDir
			);
		}
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
		return directions [UnityEngine.Random.Range (0, directions.Count)];
	}

	public static Tile GenerateFinalTile(Maze maze, int dir, int w, int h){
		int x = 0, y = 0;
		Tile finalTile = null;
		switch (dir) {
		case Character.UP:
			y = h - 1;
			do {
				x = UnityEngine.Random.Range (0, maze.width - 1);
			} while (maze.tiles[x, y - 2].wallID != 0 || maze.tiles[x + 1, y - 2].wallID != 0);

			break;
		case Character.LEFT:
			x = 0;
			do {
				y = UnityEngine.Random.Range (0, maze.height - 1);
			} while (maze.tiles[x + 2, y].wallID != 0 || maze.tiles[x + 2, y + 1].wallID != 0);
			break;
		case Character.RIGHT:
			x = w - 1;
			do {
				y = UnityEngine.Random.Range (0, maze.height - 1);
			} while (maze.tiles[x - 2, y].wallID != 0 || maze.tiles[x - 2, y + 1].wallID != 0);
			break;
		case Character.DOWN:
			y = 0;
			do {
				x = UnityEngine.Random.Range (0, maze.width - 1);
			} while (maze.tiles[x, y + 2].wallID != 0 || maze.tiles[x + 1, y + 2].wallID != 0);
			break;
		}
		finalTile = maze.tiles [x, y];
		if (finalTile.transition != null) //que deselegante 
			return GenerateFinalTile (maze, dir, w, h);
		else
			return finalTile;
	}

	public static Tile GenerateInitialTile(Maze maze, Tile finalTile, int dir) { 
		//ver funcionamento para transição entre stages estaticos e procedurais
		Vector2 initialTile = new Vector2();
		switch (dir) {
		case Character.UP:
			initialTile.x = finalTile.x;
			initialTile.y = 0;
			break;
		case Character.LEFT:
			initialTile.x = maze.width - 1;
			initialTile.y = finalTile.y;
			break;
		case Character.RIGHT:
			initialTile.x = 0;
			initialTile.y = finalTile.y;
			break;
		case Character.DOWN:
			initialTile.x = finalTile.x;
			initialTile.y = maze.height - 1;
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
	public static void SetTransitions(Maze maze1, Maze maze2, int direction, int size = 2) {
		SetTransitions (maze1, null, maze2, null, direction, size);
	}

	// Transições de ida e volta para os mazes
	// Tile1, Tile2: os tiles ANTERIORES às transições
	public static void SetTransitions(Maze maze1, Tile tile1, Maze maze2, Tile tile2, int direction, int size = 2) {
		if (tile1 == null) {
			tile1 = GenerateFinalTile (
				maze1, 
				direction,
				Math.Min (maze1.width, maze2.width),
				Math.Min (maze1.height, maze2.height)
			);
		}
		if (tile2 == null) {
			tile2 = GenerateInitialTile (
				maze2, 
				tile1, 
				direction
			);
		}

		int deltaX = 0;
		int deltaY = 0;

		int neighborX = 1;
		int neighborY = 1;

		switch (direction) {
		case Character.UP:
			deltaY = 1;
			neighborX = size;
			break;
		case Character.LEFT:
			deltaX = -1;
			neighborY = size;
			break;
		case Character.RIGHT:
			deltaX = 1;
			neighborY = size;
			break;
		case Character.DOWN:
			deltaY = -1;
			neighborX = size;
			break;
		}

		// Ida para todos os vizinhos
		for(int i = 0; i < neighborX; i++) {
			for (int j = 0; j < neighborY; j++) {
				Tile tile = maze1.tiles [tile1.x + i, tile1.y + j];
				Tile destTile = maze2.tiles [tile2.x + deltaX + i, tile2.y + deltaY + j];
				SetTransition (maze1, tile, maze2, destTile, direction);
				tile.wallID = 0;
				destTile.wallID = 0;
			}
		}

		// Volta para todos os vizinhos
		for(int i = 0; i < neighborX; i++) {
			for (int j = 0; j < neighborY; j++) {
				Tile tile = maze2.tiles [tile2.x + i, tile2.y + j];
				Tile destTile = maze1.tiles [tile1.x - deltaX + i, tile1.y - deltaY + j];
				SetTransition (maze2, tile, maze1, destTile, 3 - direction);
				tile.wallID = 0;
				destTile.wallID = 0;
			}
		}
	}

}
