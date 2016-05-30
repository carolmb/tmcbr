using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

public class Epilogue : MonoBehaviour {

	public GameObject bouquet;

	Character maid;

	// Use this for initialization
	void Start () {
		maid = GetComponent<Character> ();
		GameHUD.instance.gameObject.SetActive (false); // esconder toda a interface
		GameHUD.instance.lifeText.transform.parent.gameObject.SetActive(false);
		Player.instance.transform.position += new Vector3 (0, 128);
		GameCamera.instance.GetComponent<LampLight> ().enabled = false;
		Player.instance.gameObject.SetActive (false);

		StartCoroutine (EpilogueScene ());
	}

	IEnumerator EpilogueScene () {
		maid.TurnTo (90);
		yield return maid.Move (new Vector2 (0, 96));
		maid.Stop ();
		yield return new WaitForSeconds (0.5f);
		Instantiate (bouquet, transform.position + new Vector3 (0, 24, 24), Quaternion.identity);
		SoundManager.Cloth ();
		yield return new WaitForSeconds (0.5f);
		//yield return GameHUD.instance.dialog.ShowDialog ("I wish I could've prevent you from turning into that...", "Maid[smile]");
		yield return new WaitForSeconds (0.5f);

		Coroutine c = StartCoroutine (GameCamera.instance.FadeOut (0.5f));
		yield return new WaitForSeconds (1f);
		maid.TurnTo (270);
		yield return maid.Move (new Vector2 (0, -96));
		yield return c;
		SceneManager.LoadScene ("Title");

	}
}
