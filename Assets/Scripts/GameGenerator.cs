using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameGenerator {

	public static Maze[] Create() {
		int rangeStage = 1;
		Maze[] stage1 = new Maze[rangeStage];
		Maze[] stage2 = new Maze[rangeStage];

		MazeGenerator generator1 = MazeGenerator.GetGenerator ("Cave");
		MazeGenerator generator2 = MazeGenerator.GetGenerator ("Hall");

		Vector2 stageBegin = new Vector2 (1, Random.Range(1, rangeStage - 1));
		Tile finalTile = StageGenerator.CreateStage (stage1, generator1, stageBegin, 0);

		stageBegin = new Vector2 (1, finalTile.y);
		StageGenerator.CreateStage(stage2, generator2, stageBegin, rangeStage);

			StageGenerator.SetTransitions (
			stage1 [rangeStage - 1],
			finalTile, 
			stage2 [0],
			stage2 [0].beginTile, 
			Character.RIGHT
		);

		ExpandMazes(stage1, generator1);
		ExpandMazes(stage2, generator2);

		Maze[] mazes = new Maze[stage1.Length + stage2.Length];
		System.Array.Copy(stage1, mazes, stage1.Length);
		System.Array.Copy(stage2, 0, mazes, stage1.Length, stage2.Length);

		return mazes;
	}

	public static Maze[] ExpandMazes (Maze[] mazes, MazeGenerator generator) {
		for (int i = 0; i < mazes.Length; i++) {
			mazes [i].Expand(2, 2);
			generator.CreateEnemies (mazes[i]);
		}
		return mazes;
	}

}
