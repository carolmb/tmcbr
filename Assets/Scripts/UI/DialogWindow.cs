using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogWindow : MonoBehaviour {

	public Image portrait;
	public Text message;

	public Coroutine ShowDialog (string txt, string portraitName) {
		gameObject.SetActive (true);
		message.text = txt;
		portrait.sprite = Resources.Load<Sprite> ("Images/Portraits/" + portraitName);
		return Player.instance.StartCoroutine (WaitForDialog ());
	}

	private IEnumerator WaitForDialog () {
		while (gameObject.activeSelf) {
			yield return null;
		}
	}

	public void HideDialog() {
		gameObject.SetActive (false);
	}

}
