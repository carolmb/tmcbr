using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameGraph {

	HallStage hallStage;
	CaveStage caveStage;
	ForestStage forestStage;

	StaticStage entrance;
	StaticStage mirrorRoom;
	StaticStage fireplace;

	public Tile.Transition StartTransition() {
		return new Tile.Transition (0, entrance.transitions [0].tileX + 0.5f, 
			entrance.transitions [0].tileY, 3 - entrance.transitions[0].dir);
	}

	public GameGraph () {
		// OBS: sempre carregar as estáticas primeiro
		entrance = new StaticStage (0, "Entrance");
		mirrorRoom = new StaticStage (1, "Mirror room");
		fireplace = new StaticStage (2, "Fireplace");

		hallStage = new HallStage (3, entrance.transitions [1], mirrorRoom.transitions [0]);
		caveStage = new CaveStage (hallStage.endIndex + 1, mirrorRoom.transitions[1], fireplace.transitions[0]);
		forestStage = new ForestStage (caveStage.endIndex + 1, fireplace.transitions[1]);

		// OBS2: Por enquanto tá repetindo código mesmo mas depois vai começar a variar as transições
		SetTransitions (entrance.transitions[1], hallStage.transitions[0]);
		SetTransitions (hallStage.transitions[1], mirrorRoom.transitions[0]);
		SetTransitions (mirrorRoom.transitions[1], caveStage.transitions[0]);
		SetTransitions (caveStage.transitions[1], fireplace.transitions[0]);
		SetTransitions (fireplace.transitions[1], forestStage.transitions[0]);

		CreateObstacles (hallStage.mazes);
		CreateObstacles (caveStage.mazes);
		CreateObstacles (forestStage.mazes);
	}

	public Maze[] ToMazeArray() {
		Maze[] mazes = new Maze[forestStage.endIndex + 1];

		CopyMazes (mazes, entrance);
		CopyMazes (mazes, mirrorRoom);
		CopyMazes (mazes, fireplace);
		CopyMazes (mazes, hallStage);
		CopyMazes (mazes, caveStage);
		CopyMazes (mazes, forestStage);

		return mazes;
	}

	protected void CopyMazes(Maze[] mazeArray, Stage stage) {
		stage.GetMazes ().CopyTo (mazeArray, stage.beginIndex);
	}

	protected void CreateObstacles(ProceduralMaze[] mazes) {
		for (int i = 0; i < mazes.Length; i++) {
			mazes [i].CreateObstacles ();
		}
	}
	
	void SetTransitions(Stage.Transition t1, Stage.Transition t2) {
		Maze maze1 = t1.maze;
		Maze maze2 = t2.maze;

		Debug.Log (t1);
		Debug.Log (t2);

		Tile tile1 = maze1.tiles [(int)t1.tileX, (int)t1.tileY];
		Tile tile2 = maze2.tiles [(int)t2.tileX, (int)t2.tileY];
		ProceduralStage.SetTransitions (maze1, tile1, maze2, tile2, t1.dir, t1.size, t2.size);
	}
	
}
