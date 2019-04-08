using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour {
	public Sprite lockedSprite;
	public Sprite unlockedSprite;
	public bool isBought = false;

	public Transform[] monsterLocation;
	public Transform[] manLocation;

	//[HideInInspector]
	public List<Unit> monsterUnits;
	public int landLimit = 3;

	private SpriteRenderer spriteRenderer;

	void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		monsterUnits = new List<Unit>();
	}

	public void UpdateSprite(Sprite newSprite) {
		spriteRenderer.sprite = newSprite;
	}

	public bool canAddMonster() {
		return (monsterUnits.Count < landLimit && isBought);
	}

	public bool AddMonster(Unit monster) {
		if (canAddMonster()) {
			monsterUnits.Add(monster);
			DisplayMonsters();
			return true;
		} else {
			return false;
		}
	}

	public bool RemoveMonster(Unit monster) {
		if (monsterUnits.Contains(monster)) {
			monsterUnits.Remove(monster);
			DisplayMonsters();
			return true;
		} else {
			return false;
		}
	}

	public void DisplayMonsters() {
		for (int i = 0; i < monsterUnits.Count; i++) {
			monsterUnits[i].GetComponent<SpriteRenderer>().enabled = true;
			monsterUnits[i].MoveTo(monsterLocation[i].position);
		}
	}

	public void DisplayMen(List<Unit> men) {
		for (int i = 0; i < men.Count; i++) {
			men[i].MoveTo(manLocation[i].position);
		}
	}
}
