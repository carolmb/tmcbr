using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RiddleWindow : MonoBehaviour {

	public Text input;

	public void OnTry () {
		GameHUD.ClickItemSound ();
		if (input.text.ToLower () == "rose") {
			//blablabla
		}
	}

	public void OnReturn () {
		GameHUD.ClickItemSound ();
		gameObject.SetActive (false);
	}

	void Update () {
		if (Input.GetButtonDown ("Menu")) {
			gameObject.SetActive (false);
		}
	}

}
