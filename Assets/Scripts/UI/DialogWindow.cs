using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogWindow : MonoBehaviour {

	public Image portrait;
	public Text message;

	void Start () {
		portrait.gameObject.SetActive (false);
		message.text = "";
		gameObject.SetActive (false);
	}

	public void ShowDialog (string txt, string portraitName) {
		gameObject.SetActive (true);
		message.text = txt;
		portrait.sprite = Resources.Load<Sprite> ("Images/Portraits/" + portraitName);
	}

	public void HideDialog() {
		gameObject.SetActive (false);
	}

}
