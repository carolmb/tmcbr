using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ProceduralStage : Stage {

	public ProceduralMaze[] mazes;

	public const int expansionFactor = 2;

	public ProceduralStage(int i) : base(i) {}

	public override Maze[] GetMazes() {
		return mazes;
	}

	public static Tile GenerateBorderTile(Maze maze, int dir, int size = 2){
		int x = 0, y = 0;
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
		if (dir == Character.DOWN || dir == Character.UP) {
			for (int i = 0; i < size; i++) {
				if (maze.tiles [x + i, y].transition != null) {
					if (System.Math.Abs(maze.tiles [x + i, y].transition.tileX) > 0.0001 || System.Math.Abs(maze.tiles [x + i, y].transition.tileY) > 0.0001) {
						return GenerateBorderTile (maze, dir, size);
					}
				}
			}
		} else {
			for (int i = 0; i < size; i++) {
				if (maze.tiles [x, y + 1].transition != null) {
					if (System.Math.Abs(maze.tiles [x, y + i].transition.tileX) > 0.0001 || System.Math.Abs(maze.tiles [x, y + i].transition.tileY) > 0.0001) {
						return GenerateBorderTile (maze, dir, size);
					}
				}
			}
		}

		return maze.tiles [x, y];
	}

	//retorna uma direção diferente do argumento dado
	protected int GenerateDir(int excusive) {
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
		return directions [Random.Range (0, directions.Count)];
	}

	// Transições de ida e volta para os mazes 
	public static void SetTransitions (Maze maze1, Maze maze2, int direction, int size1 = 2, int size2 = 2) {
		SetTransitions (maze1, null, maze2, null, direction, size1, size2);
	}

	// Transições de ida e volta para os mazes
	public static void SetTransitions (Maze maze1, Tile tile1, Maze maze2, Tile tile2, int direction, int size1 = 2, int size2 = 2) {

		if (tile1 == null) {
			tile1 = GenerateBorderTile (
				maze1,
				direction,
				size1
			);
		}

		if (tile2 == null) {
			tile2 = GenerateBorderTile (
				maze2, 
				3 - direction,
				size2
			);
		}

		//ida
		SetTileTransition (maze1, tile1, maze2, tile2, direction, size1, size2);

		//volta
		SetTileTransition (maze2, tile2, maze1, tile1, 3 - direction, size2, size1);
	}
		
	protected static void SetTileTransition (Maze maze1, Tile tile1, Maze maze2, Tile tile2, int direction, int size1, int size2){

		//Debug.Log ("Transition tiles: " + maze1.GetTheme() + " to " + maze2.GetTheme());
		//Debug.Log (tile1.coordinates + " " + tile2.coordinates);
		//Debug.Log ("dir: " + direction);

		Vector2 delta = Vector2.zero;
		Vector2 destVector = tile2.coordinates;

		switch (direction) {
		case Character.UP:
			delta.y = 1;
			destVector.x += 0.5f * (size2 - 1);
			destVector.y = tile2.y + 1;
			break;
		case Character.LEFT:
			delta.x = -1;
			destVector.x = tile2.x - 1;
			destVector.y += 0.5f * (size2 - 1);
			break;
		case Character.RIGHT:
			delta.x = 1;
			destVector.x = tile2.x + 1;
			destVector.y += 0.5f * (size2 - 1);
			break;
		case Character.DOWN:
			delta.y = -1;
			destVector.x += 0.5f * (size2 - 1);
			destVector.y = tile2.y - 1;
			break;
		}

		if (direction == Character.UP || direction == Character.DOWN) {
			for (int i = 0; i < size1; i++) {
				Tile tile = maze1.tiles [tile1.x + i, tile1.y];
				tile.SetTransition (maze2.id, destVector.x, destVector.y, direction);

				//if (tile.y - (int)delta.y >= maze1.height)
				//	Debug.Log (maze1.id + " " + (tile.y - (int)delta.y) + " " + maze1.height + " " + delta.y);
				maze1.tiles [tile.x, tile.y - (int)delta.y].wallID = 0; // Remove a parede do vizinho
			}
		} else {
			for (int j = 0;	j < size1; j++) {
				Tile tile = maze1.tiles [tile1.x, tile1.y + j];
				tile.SetTransition (maze2.id, destVector.x, destVector.y, direction);
				maze1.tiles [tile.x - (int)delta.x, tile.y].wallID = 0; // Remove a parede do vizinho
			}
		}

	}

}
