using UnityEngine;
using System.Collections;

public class Plaque : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
		if (MazeManager.WorldToTilePos(Player.instance.interactedPoint) == MazeManager.WorldToTilePos(transform.position)) {
			Player.instance.Pause ();
			SoundManager.Click ();
			GameHUD.instance.riddleWindow.gameObject.SetActive (true);
		}
	}
}
