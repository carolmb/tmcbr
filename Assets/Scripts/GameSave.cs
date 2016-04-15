using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameSave {

	public string name;
	public Maze[] mazes;
	public Transition transition;
	public Bag bag;
	public int lifePoints;

	public GameSave() {
		mazes = StageGenerator.CreateStage ("Forest");

		transition = new Transition(0, mazes[0].beginMaze.x, mazes[0].beginMaze.y, 0);
		bag = new Bag ();
		lifePoints = 1000;
	}

}
