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
		NewGame ();
		//criar transição entre as duas fases

		transition = new Transition(0, mazes[0].beginTile.x, mazes[0].beginTile.y, 0);
		bag = new Bag ();
		lifePoints = 1000;
	}
		
	void NewGame() {
		int rangeStage = 3;

		Maze[] stage1 = StageGenerator.CreateStage ("Forest", new Vector2 (1, 5), rangeStage, 0);
	
		Vector2 nextBegin = new Vector2 (1, stage1 [rangeStage - 1].endTile.y);

		Maze[] stage2 = StageGenerator.CreateStage("Hall", nextBegin, rangeStage, rangeStage);

		StageGenerator.CreateTransitionBetweenStages (stage1 [rangeStage - 1], stage2 [0], 
			stage1 [rangeStage - 1].endTile, stage2 [0].beginTile);

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
