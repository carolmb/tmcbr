using UnityEngine;
using System.Collections;

public class FinalScene : MonoBehaviour {

	public Character protagonist;
	public Character duke;
	public Character maid;

	public bool hasAllRoses {
		get { return Bag.current.roses == 3; }
	}

	void Start () {
		maid = GetComponent<Character> ();
		protagonist = GameObject.Find ("Player").GetComponent<Character> ();

		if (hasAllRoses) {
			StartCoroutine (TrueEnding ());
		} else {
			StartCoroutine (BadEnding ());
		}
	}

	IEnumerator BadEnding () {
		// Protagonista encontra a criada que acabou de se matar e se mata também
		// 
		yield return null;
	}

	IEnumerator TrueEnding () {
		// Criada sente o cheiro das rosas que estão com o protagonista e resolve esperar ele entrar
		// Ela o esfaqueia quando ele abre a porta e fala sobre como ele é um babaca
		// Ele pede desculpas
		// Ela foge e fim
		// Epílogo: uma cena dela deixando flores azuis no túmulo dele
		yield return null;
	}

	void OnInteract () {
		if (!Bag.current.HasItem (Item.DB [8])) {
			Bag.current.Add (Item.DB [8]);
			SoundManager.Coin ();
		}
	}

}
