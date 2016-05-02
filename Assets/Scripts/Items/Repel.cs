using UnityEngine;
using System.Collections;

public class Repel : Item {

	public Repel(int id) : base(id, "Repel", true) {}

	public override void OnUse () {
		//Player.instance.character.PlayAnimation ("Spray", false);
		Player.instance.repelTime = 10;
	}
}
