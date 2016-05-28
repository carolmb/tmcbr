using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RiddleWindow : MonoBehaviour {

	public Text input;
	bool correctAnswer; 

	public void Start() {
		correctAnswer = false;
	}

	public void OnTry () {
		SoundManager.Click ();
		if (input.text.ToLower () == "rose" && !correctAnswer) {
			GameObject statue = GameObject.FindGameObjectWithTag ("Statue");
			statue.GetComponent<Statue> ().Explosion ();
			OnReturn ();
			correctAnswer = true;
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
