using UnityEngine;
using System.Collections;

public class FinalScene : MonoBehaviour {

	public Character protagonist;
	public Character duke;
	public Character maid;

	public bool hasAllRoses {
		get { return true; }
	}

	void Start () {
		if (hasAllRoses) {
			StartCoroutine (TrueEnding ());
		} else {
			StartCoroutine (BadEnding ());
		}
	}

	IEnumerator BadEnding () {
		/* O protagonista chega e encontra o duque numa sala bizarra. 
		 * Um pequeno diálogo acontece entre eles.
		 * O protagonista mata o duque.
		 * O duque magicamente se transforma na criada.
		 * 
		 */
		yield return null;
	}

	IEnumerator TrueEnding () {
		yield return null;
	}

}
