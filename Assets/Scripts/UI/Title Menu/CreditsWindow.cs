using UnityEngine;
using System.Collections;

public class CreditsWindow : MonoBehaviour {

	public void Return () {
		TitleMenu.ClickItemSound ();
		gameObject.SetActive (false);
		TitleMenu.instance.titleWindow.gameObject.SetActive (true);
	}

}
