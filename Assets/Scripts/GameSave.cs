using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameSave {

	public static readonly int maxHP = 9999;

	public string name;
	public Maze[] mazes;
	public Tile.Transition start;
	public Tile.Transition transition;
	public Bag bag;
	public float playTime;

	public bool golemBossFirstTime;
	public bool golemRose = false;

	public int currentFruit = 0;

	private int _lifePoints;
	public int lifePoints {
		get {
			return _lifePoints;
		}
		set {
			_lifePoints = Mathf.Min (maxHP, value);
		}
	}

	public GameSave () {
		GameGraph gameGraph = new GameGraph ();
		start = transition = gameGraph.StartTransition ();
		mazes = gameGraph.ToMazeArray ();

		bag = new Bag ();

		lifePoints = maxHP;
		playTime = 0;

		golemBossFirstTime = true; 
	}
}
