using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour {

	Canvas shopCanvas;
	UpgradeManager upgradeManager;
	GameController gameController;
	MonsterManager monsterManager;
	LandManager landManager;

	public Image MonsterPortrait;
	public TextMeshProUGUI MonsterName;
	public TextMeshProUGUI MonsterStrength;
	public TextMeshProUGUI MonsterAgility;
	public TextMeshProUGUI MonsterDefence;
	public TextMeshProUGUI MonsterHealth;

	public Button BuyButton;
	public Button UnlockButton;

	int selectedIndex;
	GameObject selectedMonster;

	void Start() {
		upgradeManager = GetComponent<UpgradeManager>();
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		monsterManager = GameObject.FindGameObjectWithTag("MonsterManager").GetComponent<MonsterManager>();
		landManager = GameObject.FindGameObjectWithTag("LandManager").GetComponent<LandManager>();

		shopCanvas = GetComponent<Canvas>();
		CloseShop();
    }

	public void BuyUnit() {
		if (gameController.SpendMana(selectedMonster.GetComponent<Unit>().Mana)) {
			monsterManager.CreateUnit(selectedMonster);
		}
	}

	public void BuyLand() {
		if (landManager.landCount < landManager.landList.Length) {
			if (gameController.SpendMana(landManager.landCost)) {
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

	public void BuyUpgrade() {
		int temp = selectedIndex;
		if (gameController.SpendMana(upgradeManager.unlockCost[selectedIndex])) {
			upgradeManager.isUnlocked[selectedIndex] = true;

			ResetShop();
			SetShop(temp);
		}
	}

	public void OpenShop() {
		shopCanvas.enabled = true;
	}

	public void CloseShop() {
		ResetShop();
		shopCanvas.enabled = false;
	}

	public void SetShop(int i) {
		selectedIndex = i;
		selectedMonster = upgradeManager.UnitUpgrades[i];
		Unit monster = selectedMonster.GetComponent<Unit>();

		MonsterPortrait.sprite = monster.UnitProtrait;
		MonsterName.text = monster.UnitName;
		MonsterStrength.text = monster.UnitStats.Strength.ToString();
		MonsterAgility.text = monster.UnitStats.Agility.ToString();
		MonsterDefence.text = monster.UnitStats.Defence.ToString();
		MonsterHealth.text = monster.Health.ToString();

		if (upgradeManager.isUnlocked[i]) {
			BuyButton.enabled = true;
			BuyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Buy (" + monster.Mana.ToString() + ")";
		} else {
			bool canUnlock = true;
			foreach (int check in upgradeManager.precedingUpgrades[i]) {
				if (!upgradeManager.isUnlocked[check]) {
					canUnlock = false;
				}
			}
			if (canUnlock) {
				UnlockButton.enabled = true;
				UnlockButton.GetComponentInChildren<TextMeshProUGUI>().text = "Unlock (" + upgradeManager.unlockCost[i].ToString() + ")";
			}
		}
	}

	public void ResetShop() {
		MonsterName.text = "";
		MonsterStrength.text = "";
		MonsterAgility.text = "";
		MonsterDefence.text = "";
		MonsterHealth.text = "";

		MonsterPortrait.sprite = null;

		BuyButton.enabled = false;
		BuyButton.GetComponentInChildren<TextMeshProUGUI>().text = "---";
		UnlockButton.enabled = false;
		UnlockButton.GetComponentInChildren<TextMeshProUGUI>().text = "---";

		selectedMonster = null;
	}
}
