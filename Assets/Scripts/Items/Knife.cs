using UnityEngine;
using System.Collections;

public class Knife : Item {

	private GameObject knife;
	private static GameObject knifeOnUse;

	float lastUse = 0;
	float delay = 0.01f;

	public Knife (int id) : base (id, "Knife", 50) {
		knife = Resources.Load<GameObject> ("Prefabs/Knife");
	}

	public override bool CanUse () {
		return Time.time - lastUse > delay && knifeOnUse == null;
	}

	public override void OnUse () {
		lastUse = Time.time;
		knifeOnUse = GameObject.Instantiate(knife);
		Player.instance.canMove = false;
		Player.instance.character.PlayAnimation ("Walking"); // Temporário (TODO: animação "slash")
	}

	public static void DestroyKnife() {
		if (knifeOnUse != null)
			GameObject.Destroy (knifeOnUse);
	}

}
