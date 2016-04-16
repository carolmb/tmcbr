using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Character))]
public class EnemyMirror : Enemy {

	public GameObject knightArmor;

	protected override void Awake () {
		base.Awake ();
	}

	void Start () {
		Spawn ();
	}

	void Update () {
		//
		if (!Player.instance.repel) {
			//
		}
	}

	void Spawn () {
		// Verificar que esta na frente
		GridPath path = PathToPlayer ();
		if (path != null && path.PreviousSteps != null) {
			Invoke ("Enemy", 3);
		}
	}

	void Enemy () {
		Instantiate (knightArmor, character.transform.position, character.transform.rotation);
	}
}
