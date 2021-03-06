﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameGraph {

	public HallStage hallStage;
	public CaveStage caveStage;
	public ForestStage forestStage;

	StaticStage entrance;
	StaticStage entrance2;
	StaticStage lockup;
	StaticStage mirrorRoom;
	StaticStage fireplace;
	StaticStage graveyard;

	public static GameGraph current = null;

	// Posição inicial do player
	public Tile.Transition StartTransition() {
		return new Tile.Transition (0, entrance.transitions [0].tileX + 0.5f, 
			entrance.transitions [0].tileY, 3 - entrance.transitions[0].dir);
	}

	// Cria as fases e seta as transições entre elas
	public GameGraph () {
		current = this;

		// OBS: sempre carregar as estáticas primeiro
		entrance = new StaticStage (0, "Entrance");
		entrance2 = new StaticStage (1, "Entrance2");
		lockup = new StaticStage (2, "Lockup");
		mirrorRoom = new StaticStage (3, "Mirror room");
		fireplace = new StaticStage (4, "Fireplace");

		hallStage = new HallStage (5, entrance.transitions [1], mirrorRoom.transitions [0]);
		caveStage = new CaveStage (hallStage.endIndex + 1, mirrorRoom.transitions[1], fireplace.transitions[0]);
		forestStage = new ForestStage (caveStage.endIndex + 1, fireplace.transitions[1]);

		graveyard = new StaticStage (forestStage.endIndex + 1, "Graveyard");

		// Da entrada para o maze com do calabouço
		SetTransitions (entrance.transitions[2], entrance2.transitions[1]);
		SetTransitions (entrance.transitions[3], entrance2.transitions[2]);

		// Da entrada do calabouço para o calabouço em si
		SetTransitions (entrance2.transitions[0], lockup.transitions[0]);

		// Da entrada para os corredores
		SetTransitions (entrance.transitions[1], hallStage.transitions[0]);

		// Dos corredores para a caverna
		SetTransitions (hallStage.transitions[1], mirrorRoom.transitions[0]);
		SetTransitions (mirrorRoom.transitions[1], caveStage.transitions[0]);

		// Da caverna para a floresta
		SetTransitions (caveStage.transitions[1], fireplace.transitions[0]);
		SetTransitions (fireplace.transitions[1], forestStage.transitions[0]);

		CreateObstacles (hallStage.mazes);
		CreateObstacles (caveStage.mazes);
		CreateObstacles (forestStage.mazes);
	}

	// Pega o array de todos os mazes do jogo
	public Maze[] ToMazeArray() {
		Maze[] mazes = new Maze[graveyard.endIndex + 1];

		CopyMazes (mazes, entrance);
		CopyMazes (mazes, entrance2);
		CopyMazes (mazes, lockup);
		CopyMazes (mazes, mirrorRoom);
		CopyMazes (mazes, fireplace);
		CopyMazes (mazes, hallStage);
		CopyMazes (mazes, caveStage);
		CopyMazes (mazes, forestStage);
		CopyMazes (mazes, graveyard);

		return mazes;
	}

	// Adiciona todos os mazes de uma stage para um array
	protected void CopyMazes(Maze[] mazeArray, Stage stage) {
		stage.GetMazes ().CopyTo (mazeArray, stage.beginIndex);
	}

	// Cria os obstáculos dos mazes de uma fase
	protected void CreateObstacles(ProceduralMaze[] mazes) {
		for (int i = 0; i < mazes.Length; i++) {
			mazes [i].CreateObstacles ();
		}
	}

	// Cria as transições entre mazes de acordo com as dadas transições de stages
	void SetTransitions(Stage.Transition t1, Stage.Transition t2) {
		Maze maze1 = t1.maze;
		Maze maze2 = t2.maze;
		Tile tile1 = maze1.tiles [(int)t1.tileX, (int)t1.tileY];
		Tile tile2 = maze2.tiles [(int)t2.tileX, (int)t2.tileY];
		ProceduralStage.SetTransitions (maze1, tile1, maze2, tile2, t1.dir, t1.size, t2.size);
	}
	
}
