using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fruit : Item {
	public int number;
	public Fruit(int id, string name, int number) : base(id, name + number, 0, 1) {
		this.number = number;
	}

	public override bool CanUse () {
		return true;
	}

	public override void OnUse () {
		SoundManager.Drink ();
		SaveManager.currentSave.lifePoints++;
		Player.instance.character.lifePoints = SaveManager.currentSave.lifePoints;
		GameHUD.instance.UpdateLife(Player.instance.character.lifePoints);
	}

}
