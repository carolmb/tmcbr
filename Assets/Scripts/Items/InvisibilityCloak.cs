using UnityEngine;
using System.Collections;

public class InvisibilityCloak : Item {

	public InvisibilityCloak(int id) : base(id, "InvisibilityCloak", false) {}

	public override void OnUse() {
		Player.instance.visible = !Player.instance.visible;
	}
}

