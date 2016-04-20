using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameGenerator {

	public static Maze[] Create() {
		int rangeStage = 1;

		Maze[] stage0 = StaticStage.LoadStaticStage ("Entrance", 0);
		Maze[] stage1 = new Maze[rangeStage];
		Maze[] stage2 = new Maze[rangeStage];

		MazeGenerator generator1 = MazeGenerator.GetGenerator ("Cave");
		MazeGenerator generator2 = MazeGenerator.GetGenerator ("Hall");

		Vector2 stageBegin = new Vector2 (1, Random.Range(1, rangeStage - 1));
		Tile finalTile = StageGenerator.CreateStage (stage1, generator1, stageBegin, 1);
	
		Debug.Log (stage1 [0].id + " " + stage1[0].beginTile.coordinates);
		StageGenerator.SetTransitions (
			stage0 [stage0.Length - 1],
			stage0 [stage0.Length - 1].endTile, 
			stage1 [0],
			stage1 [0].beginTile, 
			Character.RIGHT
		);
			
		stageBegin = new Vector2 (1, finalTile.y);
		StageGenerator.CreateStage(stage2, generator2, stageBegin, 2);
		Debug.Log (stage2 [0].id + " " + stage2[0].beginTile.coordinates);
			StageGenerator.SetTransitions (
			stage1 [rangeStage - 1],
			finalTile, 
			stage2 [0],
			stage2 [0].beginTile, 
			Character.RIGHT
		);

		ExpandTransition (stage0[0]); //PROVISÓRIO
		ExpandMazes(stage1, generator1);
		ExpandMazes(stage2, generator2);
		Debug.Log (stage1 [0].beginTile.coordinates);
		Maze[] mazes = new Maze[stage0.Length + stage1.Length + stage2.Length];
		System.Array.Copy(stage0, mazes, stage0.Length);
		System.Array.Copy(stage1, 0, mazes, stage0.Length, stage1.Length);
		System.Array.Copy(stage2, 0, mazes, stage0.Length + stage1.Length, stage2.Length);
	
		return mazes;
	}

	public static Maze[] ExpandMazes (Maze[] mazes, MazeGenerator generator) {
		for (int i = 0; i < mazes.Length; i++) {
			mazes [i].Expand(2, 2);
			generator.CreateEnemies (mazes[i]);
		}
		return mazes;
	}

	public static void ExpandTransition (Maze maze) {
		foreach (Tile t in maze.tiles) {
			if (t.transition != null) {
				t.transition.tileX *= 2;
				t.transition.tileY *= 2;
				break;
			}
		}
	}

}
