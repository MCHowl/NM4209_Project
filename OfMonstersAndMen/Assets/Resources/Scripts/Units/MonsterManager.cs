using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour {

	List<Unit> inventory;
	private int inventoryLimit = 10;

    void Start() {
		inventory = new List<Unit>();
    }

	public bool CreateUnit(GameObject monster) {
		if (inventory.Count <= inventoryLimit) {
			Unit newMonster = Instantiate(monster, transform).GetComponent<Unit>();
			StoreUnit(newMonster);

			return true;
		} else {
			return false;
		}
	}

	public bool StoreUnit(Unit monster) {
		if (inventory.Count <= inventoryLimit) {
			monster.GetComponent<SpriteRenderer>().enabled = false;
			inventory.Add(monster);
			return true;
		} else {
			return false;
		}
	}

	public bool RetrieveUnit(Unit monster) {
		if (inventory.Contains(monster)) {
			monster.GetComponent<SpriteRenderer>().enabled = true;
			inventory.Remove(monster);
			return true;
		} else {
			return false;
		}
	}
}
