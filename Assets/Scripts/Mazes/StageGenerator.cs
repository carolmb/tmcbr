using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TransitionValues {
	public float x, y; 
	public int deltaX, deltaY;
	public double deltaToCenterX, deltaToCenterY;
}

public static class StageGenerator {

	const int expansionFactor = 2;

	// Retorna o tile final da fase
	public static void CreateStage (Maze[] mazes, MazeGenerator generator, int initialID, int initialDir) {
		//int mazeCount = UnityEngine.Random.Range(5, 7);
		int mazeCount = mazes.Length;
		for (int i = 0, id = initialID; i < mazeCount; i++, id++) {
			mazes [i] = generator.Create (id, 1 + 2 * Random.Range(5, 8), 1 + 2 * Random.Range(5, 8));
			mazes [i].Expand (expansionFactor, expansionFactor);
		}
		for (int i = 0, j = 1; j < mazeCount; i++, j++) {
			initialDir = GenerateDir (initialDir);
			SetTransitions (
				mazes [i], 
				mazes [j], 
				initialDir
			);
		}

		for (int i = 0; i < mazeCount; i++) {
			int x = UnityEngine.Random.Range (0, mazeCount);
			int y = UnityEngine.Random.Range (0, mazeCount);
			initialDir = GenerateDir (initialDir);
			SetTransitions (
				mazes [x],
				mazes [y],
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
			x = Random.Range (0, (maze.width / expansionFactor) / 2);
			x = expansionFactor + x * expansionFactor * 2; 
			break;
		
		case Character.LEFT:
			x = 0;
			y = Random.Range (0, (maze.height / expansionFactor) / 2);
			y = expansionFactor + y * expansionFactor * 2; 
			break;
		
		case Character.RIGHT:
			x = maze.width - 1;
			y = Random.Range (0, (maze.height / expansionFactor) / 2);
			y = expansionFactor + y * expansionFactor * 2; 
			break;
		
		case Character.DOWN:
			y = 0;
			x = Random.Range (0, (maze.width / expansionFactor) / 2);
			x = expansionFactor + x * expansionFactor * 2; 
			break;
		}
		finalTile = maze.tiles [x, y];
		List<Tile> n = GetNeighbours (maze, finalTile, size/2 + 1);
		foreach (Tile t in n) {
			if (t.transition != null)
				return GenerateFinalTile (maze, dir, size);
		}

		return finalTile;
	}

	static bool CheckInterval(Maze maze, int x, int y, int direction, int size){
		if (x < 0 || y < 0 || x > maze.width || y > maze.height)
			return false;
		TransitionValues values = SetValuesToSetTransitions (3 - direction, size, size, x, y);
		if (direction == Character.UP || direction == Character.DOWN) {
			int j = 0;
			Debug.Log (x +" " + y + " " + values.deltaY);
			for (int i = -(int)System.Math.Ceiling(values.deltaToCenterX) + 1; 
				i < (int)System.Math.Floor (values.deltaToCenterX) + 1; i++) {
				Debug.Log (i);
				Tile tile = maze.tiles [x + i, y + j];
				if (maze.tiles [tile.x, tile.y + values.deltaY * expansionFactor].wallID > 0)
					return false;
			}
		} else {
			int i = 0;
			Debug.Log (x + " " + y + " " + values.deltaX);
			for (int j = -(int)System.Math.Ceiling(values.deltaToCenterY) + 1; 
				j < (int)System.Math.Floor (values.deltaToCenterY) + 1; j++) {
				Debug.Log (j);
				Tile tile = maze.tiles [x + i, y + j];
				if (maze.tiles [tile.x + values.deltaX * expansionFactor, tile.y].wallID > 0)
					return false;
			}
		}
		return true;
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

		tile1 = null;
		tile2 = null;
	}

	static void SetTransitionsSide(Maze maze1, Tile tile1, Maze maze2, Tile tile2, int direction, int size1, int size2){
		TransitionValues values = SetValuesToSetTransitions(direction, size1, size2, tile2.x, tile2.y);

		Vector2 destVector = new Vector2 (values.x, values.y);
		if (direction == Character.UP || direction == Character.DOWN) {
			int j = 0;
			for (int i = -(int)System.Math.Ceiling(values.deltaToCenterX) + 1; 
				i < (int)System.Math.Floor (values.deltaToCenterX) + 1; i++) {
				Tile tile = maze1.tiles [tile1.x + i, tile1.y + j];
				SetTransition (maze1, tile, maze2, destVector, direction);
				maze1.tiles [tile.x, tile.y - values.deltaY].wallID = 0;
			}
		} else {
			int i = 0;
			for (int j = -(int)System.Math.Ceiling(values.deltaToCenterY) + 1; 
				j < (int)System.Math.Floor (values.deltaToCenterY) + 1; j++) {
				Tile tile = maze1.tiles [tile1.x + i, tile1.y + j];
				SetTransition (maze1, tile, maze2, destVector, direction);
				maze1.tiles [tile.x - values.deltaX, tile.y].wallID = 0;
			}
		}

	}

	static TransitionValues SetValuesToSetTransitions(int direction, int size1, int size2, int x, int y) {
		TransitionValues values = new TransitionValues();
		values.deltaToCenterX = 1;
		values.deltaToCenterY = 1;

		// tem que passar por toda a entrada transformando em chão 
		// Ida para todos os vizinhos
		
		if (direction == Character.UP) {
			values.deltaY = 1;
			values.deltaToCenterX = ((double)size1)/2;
			values.x = (float)(x + (size2%2 == 0 ? + 0.5 : 0));
			values.y = y + 1;
		} else if (direction == Character.LEFT) {
			values.deltaX = -1;
			values.deltaToCenterY = ((double)size1)/2;
			values.x = x - 1;
			values.y = (float)(y + (size2%2 == 0 ? + 0.5 : 0));
		} else if (direction == Character.RIGHT) {
			values.deltaX = 1;
			values.deltaToCenterY = ((double)size1)/2;
			values.x = x + 1;
			values.y = (float)(y + (size2%2 == 0 ? + 0.5 : 0));
		} else if (direction == Character.DOWN) {
			values.deltaY = -1;
			values.deltaToCenterX = ((double)size1)/2;
			values.x = (float)(x + (size2%2 == 0 ? + 0.5 : 0));
			values.y = y - 1;
		}
		return values;
	}

}
