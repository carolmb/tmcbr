using UnityEngine;
using System.Collections;

public class Plaque : MonoBehaviour {

	void OnInteract () {
		Player.instance.Pause ();
		SoundManager.Click ();
		GameHUD.instance.riddleWindow.gameObject.SetActive (true);
	}
}
