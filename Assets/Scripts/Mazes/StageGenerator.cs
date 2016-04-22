﻿using UnityEngine;
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

	public static Tile GenerateFinalTile(Maze maze1, Maze maze2, int dir, int w, int h){
		int x = 0, y = 0;
		Tile finalTile = null;
		switch (dir) {
		case Character.UP:
			y = h - 1;
			do {
				x = UnityEngine.Random.Range (0, maze1.width - 1);
			} while (maze1.tiles [x, y - 2].wallID != 0 || maze1.tiles [x + 1, y - 2].wallID != 0
					|| maze2.tiles[x, 2].wallID != 0 || maze2.tiles[x + 1, 2].wallID != 0);
			break;
		
		case Character.LEFT:
			x = 0;
			do {
				y = UnityEngine.Random.Range (0, maze1.height - 1);
			} while (maze1.tiles[x + 2, y].wallID != 0 || maze1.tiles[x + 2, y + 1].wallID != 0
					|| maze2.tiles[maze2.width - 3, y].wallID != 0 || maze2.tiles[maze2.width - 3, y + 1].wallID != 0);
			break;
		
		case Character.RIGHT:
			x = w - 1;
			do {
				y = UnityEngine.Random.Range (0, maze1.height - 1);
			} while (maze1.tiles[x - 2, y].wallID != 0 || maze1.tiles[x - 2, y + 1].wallID != 0
					|| maze2.tiles[2, y].wallID != 0 || maze2.tiles[2, y + 1].wallID != 0);
			break;
		
		case Character.DOWN:
			y = 0;
			do {
				x = UnityEngine.Random.Range (0, maze1.width - 1);
			} while (maze1.tiles[x, y + 2].wallID != 0 || maze1.tiles[x + 1, y + 2].wallID != 0
					|| maze2.tiles[x, maze2.height - 3].wallID != 0 || maze2.tiles[x + 1, maze2.height - 3].wallID != 0);
			break;
		}

		finalTile = maze1.tiles [x, y];
		List<Tile> n = GetNeighbours (maze1, finalTile, 6);
		foreach (Tile t in n) {
			if (t.transition != null)
				return GenerateFinalTile (maze1, maze2, dir, w, h);
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

	public static Tile GenerateInitialTile(Maze maze, Tile finalTile, int dir) { 
		//ver funcionamento para transição entre stages estaticos e procedurais
		Vector2 initialTile = new Vector2();
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
				maze2,
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
