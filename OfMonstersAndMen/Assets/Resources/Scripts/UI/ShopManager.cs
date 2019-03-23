using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour {

	Canvas shopCanvas;
	GameController gameController;
	MonsterManager monsterManager;
	LandManager landManager;

	void Start() {
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		monsterManager = GameObject.FindGameObjectWithTag("MonsterManager").GetComponent<MonsterManager>();
		landManager = GameObject.FindGameObjectWithTag("LandManager").GetComponent<LandManager>();

		shopCanvas = GetComponent<Canvas>();
		CloseShop();
    }

	public void BuyUnit(GameObject monster) {
		if (gameController.SpendMana(monster.GetComponent<Unit>().Mana)) {
			monsterManager.CreateUnit(monster);
		}
	}

	public void BuyLand() {
		if (gameController.SpendMana(landManager.landCost)) {
			if (landManager.landCount < landManager.landList.Length) {
				landManager.UnlockLand(landManager.landCount);
				landManager.landCount++;
			}
		}
	}

	public void SellLand() {
		if (landManager.landCount > 1) {
			// Attempt to move any monsters out of land being sold
			if (landManager.landList[landManager.landCount - 1].monsterUnits.Count + monsterManager.Inventory.monsterUnits.Count <= monsterManager.Inventory.landLimit) {
				
				while (landManager.landList[landManager.landCount - 1].monsterUnits.Count > 0) {
					Unit monster = landManager.landList[landManager.landCount - 1].monsterUnits[0];
					landManager.landList[landManager.landCount - 1].RemoveMonster(monster);
					monsterManager.Inventory.AddMonster(monster);
				}

				gameController.GainMana(landManager.landCost * 2 / 3);
				landManager.LockLand(landManager.landCount);
				landManager.landCount--;
			}
		}
	}

	public void OpenShop() {
		shopCanvas.enabled = true;
	}

	public void CloseShop() {
		shopCanvas.enabled = false;
	}
}
