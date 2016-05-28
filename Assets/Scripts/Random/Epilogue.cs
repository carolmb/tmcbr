using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Epilogue : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameHUD.instance.transform.parent.gameObject.SetActive (false); // esconder toda a interface
		Player.instance.gameObject.SetActive (false);

		StartCoroutine (EpilogueScene ());
	}

	IEnumerator EpilogueScene () {
		// Epílogo: cemitério. A criada com roupas normais.
		// Ela se aproxima e deixa um buquê de flores na lápide
		// - I wish I could've prevent you from turning into that... [triste com sorriso leve]
		// Ela vai embora.
		// Fim.
		yield return null;
		SceneManager.LoadScene ("Title");

	}
}
