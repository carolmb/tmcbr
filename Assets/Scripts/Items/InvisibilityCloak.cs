using UnityEngine;
using System.Collections;

public class InvisibilityCloak : Item {

	private GameObject cloak;

	public InvisibilityCloak(int id) : base(id, "InvisibilityCloak", false) {
		cloak = Resources.Load<GameObject>("Prefabs/InvisibilityCloak");
	}

	public override void OnUse() {
		Player.instance.visible = !Player.instance.visible;
	}
}

