using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameSave {

	public string name;
	public Maze[] mazes;
	public Tile.Transition transition;
	public Bag bag;
	public int lifePoints;
	public float playTime;

	public GameSave() {
		GameGraph gameGraph = new GameGraph ();
		transition = gameGraph.StartTransition ();
		mazes = gameGraph.ToMazeArray ();

		bag = new Bag ();
		lifePoints = 5;
		playTime = 0;
	}

}
