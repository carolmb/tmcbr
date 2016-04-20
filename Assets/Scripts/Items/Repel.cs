using UnityEngine;
using System.Collections;

public class Repel : Item {

	public Repel(int id) : base(id, "Repel", true) {}

	public override void OnUse () {
		Player.instance.repelTime = 10;
	}
}
