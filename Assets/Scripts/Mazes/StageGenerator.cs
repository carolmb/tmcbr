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
			initialDir = CreateTransition (
				mazes [i], 
				mazes [j], 
				GenerateDir(initialDir)
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

	//retorna a direção da transição criada 
	public static int CreateTransition(Maze maze1, Maze maze2, int dir) {
		Tile tile1 = GenerateFinalTile (
			maze1, 
			dir,
			Math.Min (maze1.width, maze2.width),
			Math.Min (maze1.height, maze2.height)
		);
		Tile tile2 = GenerateInitialTile (
			maze2, 
			tile1, 
			dir
		);
		SetTransitions (maze1, tile1, maze2, tile2, dir);
		return dir;
	}

	static Tile GenerateFinalTile(Maze maze, int dir, int w, int h){
		if (maze.endTile != null)
			return maze.endTile;

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

	static Tile GenerateInitialTile(Maze maze, Tile finalTile, int dir) { //ver funcionamento para transição entre stages estaticos e procedurais
		if(maze.beginTile != null)
			return maze.beginTile;
		
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
	public static void SetTransitions(Maze maze1, Tile tile1, Maze maze2, Tile tile2, int direction = Character.RIGHT) {
		Tile destTile, neighbor;
		int deltaX = 0;
		int deltaY = 0;

		int neighborX = 0;
		int neighborY = 0;

		switch (direction) {
		case Character.UP:
			deltaY = 1;
			neighborX = 1;
			break;
		case Character.LEFT:
			deltaX = -1;
			neighborY = 1;
			break;
		case Character.RIGHT:
			deltaX = 1;
			neighborY = 1;
			break;
		case Character.DOWN:
			deltaY = -1;
			neighborX = 1;
			break;
		}
		//ida 
		destTile = maze2.tiles [tile2.x + deltaX, tile2.y + deltaY];
		SetTransition (maze1, tile1, maze2, destTile, direction);
		tile1.wallID = 0;
		destTile.wallID = 0;

		//ida vizinho
		destTile = maze2.tiles [tile2.x + deltaX + neighborX, tile2.y + deltaY + neighborY];
		neighbor = maze1.tiles [tile1.x + neighborX, tile1.y + neighborY];
		SetTransition (maze1, neighbor, maze2, destTile, direction);
		neighbor.wallID = 0;
		destTile.wallID = 0;

		//volta
		destTile = maze1.tiles [tile1.x - deltaX, tile1.y - deltaY];
		SetTransition (maze2, tile2, maze1, destTile, 3 - direction);
		tile2.wallID = 0;
		destTile.wallID = 0;

		//volta vizinho
		destTile = maze1.tiles [tile1.x - deltaX + neighborX, tile1.y - deltaY + neighborY];
		neighbor = maze2.tiles [tile2.x + neighborX, tile2.y + neighborY];
		SetTransition (maze2, neighbor, maze1, destTile, direction);
		neighbor.wallID = 0;
		destTile.wallID = 0;
	}

}
