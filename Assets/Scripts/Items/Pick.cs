using UnityEngine;
using System.Collections;

public class Pick : Item {

	private GameObject pick;
	private static GameObject pickOnUse;

	float lastUse = 0;
	float delay = 0.01f;

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
