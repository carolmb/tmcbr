using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Character))]
public class EnemyMirror : Enemy {

	public GameObject knightArmor;

	private bool spawned;

	protected override void Awake () {
		base.Awake ();
	}

	void Start() {
		spawned = false;
	}

	void Update () {
		//
		if (!Player.instance.repelling) {
			Spawn();
		}
	}

	void Spawn () {
		if (!spawned) {
			// Verificar que esta na frente
			Tile t = ClosestToPlayer();
			if (t != null) {
				Invoke ("Enemy", 3);
			}
			spawned = true;
		}
	}

	void Enemy () {
		Instantiate (knightArmor, character.transform.position, character.transform.rotation);
	}

	protected void OnDamage() {
	}
}
