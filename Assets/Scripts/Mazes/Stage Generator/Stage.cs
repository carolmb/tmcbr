using UnityEngine;
using System.Collections.Generic;

public abstract class Stage {

	public class Transition {
		public Maze maze;
		public float tileX;
		public float tileY;
		public int dir;
		public int size;
		public override string ToString () {
			return "Transition [Maze=" + maze.id + "][X=" + tileX + "][Y=" + tileY + "][Dir=" + dir + "][Size=" + size + "]";
		}
	}

	public int beginIndex;
	public int endIndex;

	public List<Transition> transitions;

	public Stage(int beginIndex) {
		this.beginIndex = beginIndex;
		transitions = new List<Transition> ();
	}

	public abstract Maze[] GetMazes ();

	public void AddTransition(Maze maze, float x, float y, int dir, int size) {
		Transition t = new Transition ();
		t.maze = maze;
		t.tileX = x;
		t.tileY = y;
		t.dir = dir;
		t.size = size;
		transitions.Add (t);
	}

}