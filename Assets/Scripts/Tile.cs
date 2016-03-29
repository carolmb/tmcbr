using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	
	public bool isWalkable; //false is a wall, true is a way
	public int x, y; //coordenates

	GameObject[] myObjects; //objects in the tile: player, enemys...

	void Start () {
	}
	
	void Update () {
	
	}
}
