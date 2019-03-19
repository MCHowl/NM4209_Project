using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour {

	public bool isBought = false;
	public bool isTreasureRoom;

	public Transform[] monsterLocation;
	public Transform[] manLocation;

	//[HideInInspector]
	public List<Unit> monsterUnits;
	private int landLimit = 3;

	void Start() {
		Initialise();
	}

	void Update() {
		DisplayMonsters(monsterUnits);
	}

	void Initialise() {
		monsterUnits = new List<Unit>();
	}

	public bool AddMonster(Unit monster) {
		if (monsterUnits.Count < landLimit) {
			monsterUnits.Add(monster);
			return true;
		} else {
			return false;
		}
	}

	void DisplayMonsters(List<Unit> monsters) {
		//for (int i = 0; i < monsters.Count; i++) {
		//	monsters[i].GetComponent<SpriteRenderer>().enabled = true;
		//	monsters[i].MoveTo(monsterLocation[i].position);
		//}
		int i = 0;
		foreach(Unit monster in monsters) {
			monster.GetComponent<SpriteRenderer>().enabled = true;
			monster.MoveTo(monsterLocation[i].position);
			i++;
		}
	}

	public void DisplayMen(List<Unit> men) {
		//for (int i = 0; i < men.Count; i++) {
		//	men[i].MoveTo(manLocation[i].position);
		//}

		int i = 0;
		foreach (Unit man in men) {
			man.GetComponent<SpriteRenderer>().enabled = true;
			man.MoveTo(manLocation[i].position);
			i++;
		}
	}
}
