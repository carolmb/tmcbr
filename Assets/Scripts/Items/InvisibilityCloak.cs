using UnityEngine;
using System.Collections;

public class InvisibilityCloak : Item {

	public InvisibilityCloak(int id) : base(id, "InvisibilityCloak", 100) {}

	public override bool CanUse () {
		return true;
	}

	public override void OnUse() {
		AudioClip clip = Resources.Load<AudioClip> ("Sounds/cloth");
		GameCamera.PlayAudioClip (clip);

		Color c = Player.instance.character.spriteRenderer.color;
		if (Player.visible) {
			Player.visible = false;
			c.a = 0.5f;
		} else {
			Player.visible = true;
			c.a = 1;
		}
		Player.instance.character.spriteRenderer.color = c;
	}
}

