using UnityEngine;
using System.Collections;

public class Plaque : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (MazeManager.WorldToTilePos(Player.instance.interactedPoint) == MazeManager.WorldToTilePos(transform.position)) {
			GameHUD.instance.riddleWindow.gameObject.SetActive (true);
		}
	}
}
