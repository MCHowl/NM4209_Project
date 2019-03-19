using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour {

	public bool isBought = false;
	public bool isTreasureRoom;

	public Transform[] monsterLocation;
	public Transform[] manLocation;

	[HideInInspector]
	public List<Unit> monsterUnits;

	private void Start() {
		Initialise();
	}

	void Initialise() {
		monsterUnits = new List<Unit>();
	}

	void AddMonster(Unit monster) {

	}

	public void DisplayMen(List<Unit> men) {
		for (int i = 0; i < men.Count; i++) {
			men[i].MoveTo(manLocation[i].position);
		}
	}
}
