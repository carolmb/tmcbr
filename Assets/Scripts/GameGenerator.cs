using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// Estrutura auxiliar
public class Stage {
	public Maze[] mazes;
	public Tile beginTile = null;
	public int beginDir;
	public Tile endTile = null;
	public int endDir;
	public Stage(Maze[] mazes) {
		this.mazes = mazes;
	}
	public Stage(int size) {
		this.mazes = new Maze[size];
	}
}

public class GameGenerator {

	public static Stage Create () {
		Stage[] stages = new Stage[4];

		stages[0] = StaticStage.LoadStaticStage ("Entrance", 0);
		stages[1] = new Stage (1); // new Stage(Random.Range(5, 7));
		stages[2] = new Stage (1);
		stages[3] = new Stage (1);

		MazeGenerator generator1 = MazeGenerator.GetGenerator ("Hall");
		MazeGenerator generator2 = MazeGenerator.GetGenerator ("Cave");
		MazeGenerator generator3 = MazeGenerator.GetGenerator ("Forest");

		StageGenerator.CreateStage (stages[1].mazes, generator1, GetBeginID(stages, 1), Character.UP);
		StageGenerator.CreateStage (stages[2].mazes, generator2, GetBeginID(stages, 2), Character.DOWN);
		StageGenerator.CreateStage (stages[3].mazes, generator3, GetBeginID(stages, 3), Character.DOWN);

		SetTransitionStages (stages[0], stages[1], Character.UP); //entrance para hall
		SetTransitionStages (stages[1], stages[2], Character.DOWN); //hall para cave
		SetTransitionStages (stages[2], stages[3], Character.LEFT); //cave para forest

		CreateEnemies (stages[1].mazes, generator1);
		CreateEnemies (stages[2].mazes, generator2);
		CreateEnemies (stages[3].mazes, generator3);

		Stage bigStage = new Stage(GetMazeArray(stages));
		bigStage.beginTile = stages [0].beginTile;
		bigStage.beginDir = stages [0].beginDir;
		return bigStage;
	}

	static int GetBeginID(Stage[] allStages, int mazeID) {
		int beginID = 0;
		for(int i = 0; i < mazeID; i++) {
			beginID += allStages [i].mazes.Length;
		}
		return beginID;
	}

	static Maze[] GetMazeArray(Stage[] stages) {
		int mazeCount = 0;
		for (int i = 0; i < stages.Length; i++) {
			mazeCount += stages [i].mazes.Length;
		}
		Maze[] mazes = new Maze[mazeCount];
		int beginID = 0;
		for(int i = 0; i < stages.Length; i++) {
			System.Array.Copy (stages [i].mazes, 0, mazes, beginID, stages [i].mazes.Length);
			beginID += stages [i].mazes.Length;
		}
		return mazes;
	}

	static void SetTransitionStages (Stage stage1, Stage stage2, int dir) {
		Maze prevMaze = stage1.mazes [stage1.mazes.Length - 1];
		Maze nextMaze = stage2.mazes [0];

		Tile t1 = stage1.endTile;
		Tile t2 = stage2.beginTile;

		StageGenerator.SetTransitions (prevMaze, t1, nextMaze, t2, dir);
	}

	static void CreateEnemies(Maze[] mazes, MazeGenerator generator) {
		for (int i = 0; i < mazes.Length; i++) {
			generator.CreateEnemies (mazes [i]);
		}
	}

}
