using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// Estrutura auxiliar
public class Stage {
	public Maze[] mazes;
	public Tile beginTile = null;
	public int beginDir = -1;
	public int beginSize = 2;
	public Tile endTile = null;
	public int endDir = -1;
	public int endSize = 2;
	public Stage(Maze[] mazes) {
		this.mazes = mazes;
	}
	public Stage(int size) {
		this.mazes = new Maze[size];
	}
}

public class GameGenerator {

	public static Stage Create () {
		Stage[] stages = new Stage[6];

		int size1 = 1; // Random.Range(5, 7);
		int size3 = 1;
		int size5 = 1;

		stages[0] = StaticStage.LoadStaticStage ("Entrance", 0);
		stages[1] = new Stage (size1);
		stages[2] = StaticStage.LoadStaticStage("Mirror room", size1 + 1);
		stages[3] = new Stage (size3);
		stages[4] = StaticStage.LoadStaticStage("Fireplace", size1 + size3 + 2);
		stages[5] = new Stage (size5);

		MazeGenerator[] generators = new MazeGenerator[3];

		generators[0] = MazeGenerator.GetGenerator ("Hall");
		generators[1] = MazeGenerator.GetGenerator ("Cave");
		generators[2] = MazeGenerator.GetGenerator ("Forest");

		// Cria cada uma das stages que acordo com a direção da "stage de transição" anterior
		for (int i = 0; i < generators.Length; i++) {
			StageGenerator.CreateStage (stages[2*i + 1].mazes, generators[i], GetBeginID(stages, 2*i + 1), stages[2*i].endDir);
		}

		// Seta as transições de uma stage para outra
		for (int i = 0; i < stages.Length - 1; i++) {
			int direction = stages [i].endDir; // Direção de final da stage atual
			if (direction == -1) { // Caso não haja
				direction = stages [i + 1].beginDir; // Pegar a direção do início da próxima stage
			}
			SetTransitionStages (stages[i], stages[i + 1], direction); //entrance para hall
		}
			
		// Completar os labirintos gerados com obstáculos e inimigos
		for (int i = 0; i < generators.Length; i++) {
			CreateEnemies (stages[2*i + 1].mazes, generators[i]);
		}

		Stage bigStage = new Stage(GetMazeArray(stages));
		bigStage.beginTile = stages [0].beginTile;
		bigStage.beginDir = stages [0].beginDir;
		bigStage.beginSize = stages [0].beginSize;
		return bigStage;
	}

	static int GetBeginID(Stage[] allStages, int stageID) {
		int beginID = 0;
		for(int i = 0; i < stageID; i++) {
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

		StageGenerator.SetTransitions (prevMaze, t1, nextMaze, t2, dir, stage1.endSize, stage2.beginSize);
	}

	static void CreateEnemies(Maze[] mazes, MazeGenerator generator) {
		for (int i = 0; i < mazes.Length; i++) {
			generator.CreateEnemies (mazes [i]);
		}
	}

}
