using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RiddleWindow : MonoBehaviour {

	public Text input;

	public void OnTry () {
		SoundManager.Click ();
		if (input.text.ToLower () == "rose") {
			//blablabla
		}
	}

	public void OnReturn () {
		SoundManager.Click ();
		gameObject.SetActive (false);
	}

	void Update () {
		if (Input.GetButtonDown ("Menu")) {
			gameObject.SetActive (false);
		}
	}

}
