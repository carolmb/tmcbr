using UnityEngine;
using System.Collections;

public class Pick : Item {

	public int damage = 2;

	private GameObject pick;
	private static GameObject pickOnUse;

	float lastUse = 0;
	float delay = 0.01f;
	public float speed = 5;

	public Pick(int id) : base(id, "Pick", 75) {
		pick = Resources.Load<GameObject>("Prefabs/Pick");
	}

	public override bool CanUse () {
		return Time.time - lastUse > delay && pickOnUse == null;
	}

	public override void OnUse () {
		lastUse = Time.time;
		pickOnUse = GameObject.Instantiate(pick);
		Player.instance.canMove = false;
		Player.instance.character.PlayAnimation ("Walking"); // Temporário (TODO: animação "slash")
	}
}
