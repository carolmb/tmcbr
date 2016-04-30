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

	public static Tile GenerateFinalTile(Maze maze, int dir, int size = 2){
		int x = 0, y = 0;
		Tile finalTile = null;
		switch (dir) {
		case Character.UP:
			y = maze.height - 1;
			do {
				x = UnityEngine.Random.Range (size/2, maze.width - size/2);
			} while (maze.tiles [x, y - 2].wallID != 0 || maze.tiles [x + 1, y - 2].wallID != 0);
			break;
		
		case Character.LEFT:
			x = 0;
			do {
				y = UnityEngine.Random.Range (size/2, maze.height - size/2);
			} while (maze.tiles[x + 2, y].wallID != 0 || maze.tiles[x + 2, y + 1].wallID != 0);
			break;
		
		case Character.RIGHT:
			x = maze.width - 1;
			do {
				y = UnityEngine.Random.Range (size/2, maze.height - size/2);
			} while (maze.tiles[x - 2, y].wallID != 0 || maze.tiles[x - 2, y + 1].wallID != 0);
			break;
		
		case Character.DOWN:
			y = 0;
			do {
				x = UnityEngine.Random.Range (size/2, maze.width - size/2);
			} while (maze.tiles[x, y + 2].wallID != 0 || maze.tiles[x + 1, y + 2].wallID != 0);
			break;
		}

		finalTile = maze.tiles [x, y];
		List<Tile> n = GetNeighbours (maze, finalTile, 6);
		foreach (Tile t in n) {
			if (t.transition != null)
				return GenerateFinalTile (maze, dir, size);
		}

		return finalTile;
	}

	//código duplicado
	static List<Tile> GetNeighbours(Maze maze, Tile tile, int delta){
		List<Tile> neighbours = new List<Tile> ();
		for (int i = tile.x - delta; i < tile.x + delta; i++) {
			for (int j = tile.y - delta; j < tile.y + delta; j++) {
				if (i >= 0 && i < maze.width && j >= 0 && j < maze.height) {
					neighbours.Add (maze.tiles [i, j]);
				}
			}
		}
		return neighbours;
	} 

	public static Tile GenerateInitialTile(Maze maze, int dir, int size) { 
		//ver funcionamento para transição entre stages estaticos e procedurais
		/*Vector2 initialTile = new Vector2();
		switch (dir) {
		case Character.UP:
			initialTile.x = finalTile.x;
			initialTile.y = 0;
			if (maze.tiles [(int)initialTile.x, (int)initialTile.y + 2].isWall)
				return GenerateFinalTile (maze, maze, Character.DOWN, maze.width, maze.height);
			break;
		case Character.LEFT:
			initialTile.x = maze.width - 1;
			initialTile.y = finalTile.y;
			if (maze.tiles [(int)initialTile.x - 2, (int)initialTile.y].isWall)
				return GenerateFinalTile (maze, maze, Character.RIGHT, maze.width, maze.height);
			break;
		case Character.RIGHT:
			initialTile.x = 0;
			initialTile.y = finalTile.y;
			if (maze.tiles [(int)initialTile.x + 2, (int)initialTile.y].isWall)
				return GenerateFinalTile (maze, maze, Character.LEFT, maze.width, maze.height);
			break;
		case Character.DOWN:
			initialTile.x = finalTile.x;
			initialTile.y = maze.height - 1;
			if (maze.tiles [(int)initialTile.x, (int)initialTile.y - 2].isWall)
				return GenerateFinalTile (maze, maze, Character.UP, maze.width, maze.height);
			break;
		}
		return maze.tiles[(int)initialTile.x, (int)initialTile.y];*/
		return GenerateFinalTile (maze, 3 - dir, size);
	}

	// Cria uma transição de um tile de um labirinto para outro
	private static void SetTransition (Maze origMaze, Tile origTile, Maze destMaze, Vector2 destVector, int direction) {
		Transition transition = new Transition (destMaze.id, destVector.x, destVector.y, direction);
		origTile.transition = transition;
		origTile.wallID = 0;
	}

	// Transições de ida e volta para os mazes 
	// Tile1, Tile2: os tiles ANTERIORES às transições
	public static void SetTransitions(Maze maze1, Maze maze2, int direction, int size1 = 2, int size2 = 2) {
		SetTransitions (maze1, null, maze2, null, direction, size1, size2);
	}

	// Transições de ida e volta para os mazes
	// Tile1, Tile2: os tiles ANTERIORES às transições
	// size1: tamanho da saída no maze1
	// size2: tamanho da entrada no maze2
	public static void SetTransitions(Maze maze1, Tile tile1, Maze maze2, Tile tile2, int direction, int size1 = 2, int size2 = 2) {
		//Debug.Log (maze1.theme + " to " + maze2.theme); 

		if (tile1 == null) {
			tile1 = GenerateFinalTile (
				maze1,
				direction,
				size1
			);
		}
		//Debug.Log (tile1.coordinates);
		if (tile2 == null) {
			tile2 = GenerateInitialTile (
				maze2, 
				direction,
				size2
			);
		}
		//Debug.Log (tile2.coordinates);
		//ida
		SetTransitionsSide (maze1, tile1, maze2, tile2, direction, size1, size2);

		//volta
		SetTransitionsSide (maze2, tile2, maze1, tile1, 3 - direction, size2, size1);
	}

	static void SetTransitionsSide(Maze maze1, Tile tile1, Maze maze2, Tile tile2, int direction, int size1, int size2){
		Debug.Log (maze1.theme + " to " + maze2.theme); 
		Debug.Log (tile1.coordinates + " entrance");
		int deltaX = 0;
		int deltaY = 0;

		double neighborX = 1;
		double neighborY = 1;

		// tem que passar por toda a entrada transformando em chão 
		// Ida para todos os vizinhos
		float x = 0, y = 0;

		/*if (direction == Character.UP) {
			deltaY = 1;
			neighborX = (double)size1/2;
			x = (float)(tile2.x + size2 * 0.5 - 0.5);
			y = tile2.y + 1;
		
		} else if (direction == Character.LEFT) {
			deltaX = -1;
			neighborY = (double)size1/2;
			x = tile2.x - 1;
			y = (float)(tile2.y + size2 * 0.5 - 0.5);
		} else if (direction == Character.RIGHT) {
			deltaX = 1;
			neighborY = (double)size1/2;
			x = tile2.x + 1;
			y = (float)(tile2.y + size2 * 0.5 - 0.5);
		} else if (direction == Character.DOWN) {
			deltaY = -1;
			neighborX = (double)size1/2;
			x = (float)(tile2.x + size2 * 0.5 - 0.5);
			y = tile2.y - 1;
		}*/

		if (direction == Character.UP) {
			deltaY = 1;
			neighborX = ((double)size1)/2;
			x = (float)(tile2.x + (size2%2 == 0 ? + 0.5 : 0));
			y = tile2.y + 1;
		} else if (direction == Character.LEFT) {
			deltaX = -1;
			neighborY = ((double)size1)/2;
			x = tile2.x - 1;
			y = (float)(tile2.y + (size2%2 == 0 ? + 0.5 : 0));
		} else if (direction == Character.RIGHT) {
			deltaX = 1;
			neighborY = ((double)size1)/2;
			x = tile2.x + 1;
			y = (float)(tile2.y + (size2%2 == 0 ? + 0.5 : 0));
		} else if (direction == Character.DOWN) {
			deltaY = -1;
			neighborX = ((double)size1)/2;
			x = (float)(tile2.x + (size2%2 == 0 ? + 0.5 : 0));
			y = tile2.y - 1;
		}

		Vector2 destVector = new Vector2 (x, y);
		if (direction == Character.UP || direction == Character.DOWN) {
			int j = 0;
			for (int i = -(int)System.Math.Ceiling(neighborX) + 1; 
				i < (int)System.Math.Floor (neighborX) + 1; i++) {
				Tile tile = maze1.tiles [tile1.x + i, tile1.y + j];
				SetTransition (maze1, tile, maze2, destVector, direction);
				maze1.tiles [tile.x, tile.y - deltaY].wallID = 0;
			}
		} else {
			int i = 0;
			for (int j = -(int)System.Math.Ceiling(neighborY) + 1; 
				j < (int)System.Math.Floor (neighborY) + 1; j++) {
				Tile tile = maze1.tiles [tile1.x + i, tile1.y + j];
				SetTransition (maze1, tile, maze2, destVector, direction);
				maze1.tiles [tile.x - deltaX, tile.y].wallID = 0;
			}
		}

	} 

}
