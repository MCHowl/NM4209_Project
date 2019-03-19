using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour {

	public List<Unit> Inventory;
	private int inventoryLimit = 10;

    void Awake() {
		Inventory = new List<Unit>();
    }

	public bool CreateUnit(GameObject monster) {
		if (Inventory.Count <= inventoryLimit) {
			Unit newMonster = Instantiate(monster, transform).GetComponent<Unit>();
			StoreUnit(newMonster);

			return true;
		} else {
			return false;
		}
	}

	public bool StoreUnit(Unit monster) {
		if (Inventory.Count <= inventoryLimit) {
			monster.GetComponent<SpriteRenderer>().enabled = false;
			Inventory.Add(monster);
			return true;
		} else {
			return false;
		}
	}

	public bool RetrieveUnit(Unit monster) {
		if (Inventory.Contains(monster)) {
			monster.GetComponent<SpriteRenderer>().enabled = true;
			Inventory.Remove(monster);
			return true;
		} else {
			return false;
		}
	}
}
