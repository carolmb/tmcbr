using UnityEngine;
using System.Collections;

public class Tomato : Enemy {

	public GameObject tomato;
	public int maxInvokes;

	// Use this for initialization

	void Start () {
		base.Awake ();
		Spaw ();
	}
	
	void Update () {
		if (Player.instance.paused)
			return;
	}

	void Spaw () {
		if (!character.moving && !character.damaging) {
			GridPath path = PathToPlayer ();
			if (path != null && path.PreviousSteps != null) {
				Invoke ("MiniTomato", 3);
			}
		}
		Invoke ("Spaw", 6);
	}

	void MiniTomato () {
		Instantiate (tomato, character.transform.position, character.transform.rotation);
	}
}
