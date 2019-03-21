using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour {

	[HideInInspector]
	public Land Inventory;

    void Start() {
		Inventory = GetComponentInChildren<Land>();
	}

	public bool CreateUnit(GameObject monster) {
		if (Inventory.monsterUnits.Count <= Inventory.landLimit) {
			Unit newMonster = Instantiate(monster, transform).GetComponent<Unit>();
			StoreUnit(newMonster);

			return true;
		} else {
			return false;
		}
	}

	public bool StoreUnit(Unit monster) {
		return Inventory.AddMonster(monster);
	}

	public bool RetrieveUnit(Unit monster) {
		return Inventory.RemoveMonster(monster);
	}

	private void RefreshInventory() {
		Inventory.DisplayMonsters();
	}
}
