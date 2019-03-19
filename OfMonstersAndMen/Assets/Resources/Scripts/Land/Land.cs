using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour {

	public Sprite unlockedSprite;
	public bool isBought = false;

	public Transform[] monsterLocation;
	public Transform[] manLocation;

	//[HideInInspector]
	public List<Unit> monsterUnits;
	private int landLimit = 3;

	private SpriteRenderer spriteRenderer;

	void Awake() {
		Initialise();
	}

	void Update() {
		DisplayMonsters(monsterUnits);
	}

	void Initialise() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		monsterUnits = new List<Unit>();
	}

	public void BuyLand() {
		isBought = true;
		spriteRenderer.sprite = unlockedSprite;
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
		for (int i = 0; i < monsters.Count; i++) {
			monsters[i].GetComponent<SpriteRenderer>().enabled = true;
			monsters[i].MoveTo(monsterLocation[i].position);
		}
	}

	public void DisplayMen(List<Unit> men) {
		for (int i = 0; i < men.Count; i++) {
			men[i].MoveTo(manLocation[i].position);
		}
	}
}
