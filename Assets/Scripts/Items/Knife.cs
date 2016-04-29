using UnityEngine;
using System.Collections;

public class Knife : Item {

	private GameObject knife;
	private static GameObject knifeOnUse;

	float lastUse = 0;
	float delay = 0.01f;

	public Knife(int id) : base(id, "Knife", false) {
		knife = Resources.Load<GameObject>("Prefabs/Knife");
	}

	public override void OnUse () {
		if (Time.time - lastUse > delay && knifeOnUse == null) {
			lastUse = Time.time;
			knifeOnUse = GameObject.Instantiate(knife);
			Player.instance.canMove = false;
			Player.instance.character.Stop	();
		}
	}

}
