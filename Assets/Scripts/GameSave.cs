using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameSave {

	public string name;
	public Maze[] mazes;
	public Transition transition;
	public Bag bag;
	public int lifePoints;

	public GameSave() {
		CreateStages ();
		//criar transição entre as duas fases

		transition = new Transition(0, mazes[0].beginTile.x, mazes[0].beginTile.y, 0);
		bag = new Bag ();
		lifePoints = 1000;
	}
		
	void CreateStages() {
		int rangeStage = 3;
		Maze[] stage1 = new Maze[rangeStage];
		Maze[] stage2 = new Maze[rangeStage];

		Vector2 stageBegin = new Vector2 (1, 5);
		Tile finalTile = StageGenerator.CreateStage (stage1, "Forest", stageBegin, 0);

		stageBegin = new Vector2 (1, finalTile.y);
		StageGenerator.CreateStage(stage2, "Hall", stageBegin, rangeStage);

		StageGenerator.SetTransitions (
			stage1 [rangeStage - 1],
			finalTile, 
			stage2 [0],
			stage2 [0].beginTile, Character.RIGHT);

		/*
		StageGenerator.SetTransition (
			stage1[stage1.Length - 1],
			finalTile.coordinates,
			stage2[0],


		StageGenerator.SetTransition (	stage1 [rangeStage - 1].endTile, 
										stage2 [0].beginTile, 
										stage1 [rangeStage - 1], 
										stage2 [0]);
		*/

		MazeGenerator generator1 = MazeGenerator.GetGenerator (stage1[0].theme);
		StageGenerator.ExpandMazes(stage1, generator1);

		MazeGenerator generator2 = MazeGenerator.GetGenerator (stage2[0].theme);
		StageGenerator.ExpandMazes(stage2, generator2);

		mazes = new Maze[stage1.Length + stage2.Length];
		Array.Copy(stage1, mazes, stage1.Length);
		Array.Copy(stage2, 0, mazes, stage1.Length, stage2.Length);
	
		//Debug.Log ("IMPORTANTE1: " + stage1 [rangeStage - 1].endTile.transition.mazeID);
		//Debug.Log ("IMPORTANTE: " + mazes [rangeStage - 1].endTile.transition.mazeID);
	}

}
