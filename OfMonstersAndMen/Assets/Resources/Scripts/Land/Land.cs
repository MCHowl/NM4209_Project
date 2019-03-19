using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour {

	Unit[] monsterUnits;
	Unit[] manUnits;

	public bool isBought = false;
	public bool isTreasureRoom;

	private void Start() {
		Initialise();
	}

	void Initialise() {
		monsterUnits = new Unit[3];
	}

	void Battle() {
		BattleManager.Battle(monsterUnits, manUnits);
	}

	void PlaceMonster(Unit[] monster) {

	}
}
