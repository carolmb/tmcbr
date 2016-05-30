using UnityEngine;
using System.Collections;

public class TutorialWindow : MonoBehaviour {

	public void Return () {
		SoundManager.Click ();
		gameObject.SetActive (false);
		TitleMenu.instance.titleWindow.gameObject.SetActive (true);
	}

}
