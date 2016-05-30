﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameSave {

	public string name;
	public Maze[] mazes;
	public Tile.Transition start;
	public Tile.Transition transition;
	public Bag bag;
	public int lifePoints;
	public float playTime;

	public bool golemBossFirstTime;

	public GameSave() {
		GameGraph gameGraph = new GameGraph ();
		start = transition = gameGraph.StartTransition ();
		mazes = gameGraph.ToMazeArray ();

		bag = new Bag ();

		lifePoints = 99;
		playTime = 0;

		golemBossFirstTime = true; 
	}
}
