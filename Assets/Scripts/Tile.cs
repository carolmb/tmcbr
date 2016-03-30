using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {

	public readonly static int size = 32;

	public bool isWalkable; //false is a wall, true is a way
	public int x, y; //coordenates

	GameObject[] myObjects; //objects in the tile: player, enemys...

	void Start () {
	}
	
	void Update () {
	
	}

	public List<Tile> GetNeighbours() {
		return null;
	}

}
