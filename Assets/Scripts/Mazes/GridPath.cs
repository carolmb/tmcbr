using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GridPath : IEnumerable<Tile> {

	public Tile LastStep { get; private set; }
	public GridPath PreviousSteps { get; private set; }
	public float TotalCost { get; private set; }
	
	private GridPath(Tile lastStep, GridPath previousSteps, float totalCost) {
		LastStep = lastStep;
		PreviousSteps = previousSteps;
		TotalCost = totalCost;
	}
	
	public GridPath(Tile start) : this(start, null, 0) { }
	
	public GridPath AddStep(Tile step, float stepCost) {
		return new GridPath(step, this, TotalCost + stepCost);
	}

	public IEnumerator<Tile> GetEnumerator() {
		for (var p = this; p != null; p = p.PreviousSteps)
			yield return p.LastStep;
	}

	public Tile FirstStep {
		get {
			GridPath p = this;
			while (p.PreviousSteps != null) {
				p = p.PreviousSteps;
			}
			return p.LastStep;
		}
	}

	IEnumerator IEnumerable.GetEnumerator() {
		return this.GetEnumerator();
	}

}
