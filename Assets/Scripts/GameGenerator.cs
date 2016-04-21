﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameGenerator {

	public static Maze[] Create () {
		Maze[] stage0 = StaticStage.LoadStaticStage ("Entrance", 0);
		Maze[] stage1 = new Maze[1], stage2 = new Maze[1], stage3 = new Maze[1]; //provisório

		MazeGenerator generator1 = MazeGenerator.GetGenerator ("Hall");
		MazeGenerator generator2 = MazeGenerator.GetGenerator ("Cave");
		MazeGenerator generator3 = MazeGenerator.GetGenerator ("Forest");

		StageGenerator.CreateStage (stage1, generator1, 1, Character.UP);
		StageGenerator.CreateStage (stage2, generator2, 1 + stage1.Length, Character.DOWN);
		StageGenerator.CreateStage (stage3, generator3, 1 + stage1.Length + stage2.Length, Character.DOWN);

		SetTransitionStages (stage0, stage1, Character.UP); //entrance para hall
		SetTransitionStages (stage1, stage2, Character.DOWN); //hall para cave
		SetTransitionStages (stage2, stage3, Character.LEFT); //cave para forest

		Maze[] mazes = new Maze[stage0.Length + stage1.Length + stage2.Length + stage3.Length];
		System.Array.Copy(stage0, mazes, stage0.Length);
		System.Array.Copy(stage1, 0, mazes, stage0.Length, stage1.Length);
		System.Array.Copy(stage2, 0, mazes, stage0.Length + stage1.Length, stage2.Length);
		System.Array.Copy(stage3, 0, mazes, stage0.Length + stage1.Length + stage2.Length, stage3.Length);	
	
		return mazes;
	}

/*	public static Maze[] ExpandMazes (Maze[] mazes, MazeGenerator generator) {
		for (int i = 0; i < mazes.Length; i++) {
			mazes [i].Expand(2, 2);
			generator.CreateEnemies (mazes[i]);
		}
		return mazes;
	}
*/

	static void SetTransitionStages (Maze[] stage1, Maze[] stage2, int dir) {
		StageGenerator.CreateTransition (
			stage1 [stage1.Length - 1], 
			stage2 [0], 
			dir
		);
	}

}
