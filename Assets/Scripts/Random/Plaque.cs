using UnityEngine;
using System.Collections;

public class Plaque : MonoBehaviour {

	public string message;

	void OnInteract () {
		Player.instance.Pause ();
		//SoundManager.Click ();
		StartCoroutine (ShowMessage ());
	}

	IEnumerator ShowMessage() {
		yield return GameHUD.instance.dialog.ShowDialog (message, "");
		Player.instance.Resume ();
	}

}
