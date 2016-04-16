using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class StageGenerator {

	public static Maze[] ExpandMazes (Maze[] mazes, MazeGenerator generator) {
		for (int i = 0; i < mazes.Length; i++) {
			generator.ExpandMaze (mazes [i], 2, 2);
			generator.CreateEnemies (mazes[i]);
		}

		return mazes;
	}

	public static Maze[] CreateStage (string theme, Vector2 beginCoordinates, int mazeCount, int initialID) {

		Tile initialTile, finalTile;

		Maze[] mazes = new Maze[mazeCount];
		MazeGenerator generator = MazeGenerator.GetGenerator (theme);

		mazes [0] = generator.Create (initialID, 15, 15, beginCoordinates);
		for (int i = 1, id = initialID + 1; i < mazeCount; i++, id++) {
			mazes [i] = generator.Create(id, 15, 15);
		}

		//cria o final do nível
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

			mazes [i].tiles [initialTile.x - 1, initialTile.y].wallID = 0;

			if (i != 0) {
				finalTile = mazes [i - 1].tiles [mazes [i].width - 1, initialTile.y];
				finalTile.wallID = 0;

				generator.SetTransition (finalTile, initialTile, mazes [i - 1], mazes [i]);
				generator.SetTransition (mazes [i].tiles [initialTile.x - 1, initialTile.y], 
				mazes[i - 1].tiles[finalTile.x - 1, finalTile.y], mazes [i], mazes [i - 1]);
				
			} else {
				finalTile.wallID = 0;

				generator.SetTransition (finalTile, initialTile, mazes [mazeCount - 1], mazes [i]);
				generator.SetTransition (mazes [i].tiles [initialTile.x - 1, initialTile.y], 
				mazes[mazeCount - 1].tiles[finalTile.x - 1, finalTile.y], mazes [i], mazes [mazeCount - 1]);
			}

		}
		mazes [mazeCount - 1].endMaze = finalTile;

		return mazes;
	}

	public static void CreateTransitionBetweenStages (Maze initialMaze, Maze finalMaze, Tile initialTile, Tile finalTile) {
	//	float angle1 = GameManager.VectorToAngle (initialTile.coordinates - initialMaze.center);
	//	int direction1 = Character.AngleToDirection (Mathf.RoundToInt (angle1 / 90) * 90);
	//	initialTile.transition = new Transition (finalMaze.id, finalTile.x, finalTile.y, direction1);


	//	float angle2 = GameManager.VectorToAngle (finalTile.coordinates - finalMaze.center);
	//	int direction2 = Character.AngleToDirection (Mathf.RoundToInt (angle2 / 90) * 90);
	//	finalTile.transition = new Transition (initialMaze.id, 1, initialTile.y, direction2);

	}

}
