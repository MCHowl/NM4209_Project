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
	public TextMeshProUGUI MonsterDescription;
	public TextMeshProUGUI MonsterPrimaryStat;
	public TextMeshProUGUI MonsterStrength;
	public TextMeshProUGUI MonsterAgility;
	public TextMeshProUGUI MonsterDefence;
	public TextMeshProUGUI MonsterHealth;

	public Button BuyButton;
	public Button UnlockButton;
	public TextMeshProUGUI LandBuyText;
	public TextMeshProUGUI LandSellText;
	//public TextMeshProUGUI LandBonusText;

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

	private void Update() {
		if (Input.GetKeyDown(KeyCode.S) && !gameController.isWaveRunning) {
			if (shopCanvas.enabled) {
				CloseShop();
			} else {
				OpenShop();
			}
		}
	}

	public void BuyUnit() {
		if (monsterManager.Inventory.monsterUnits.Count < monsterManager.Inventory.landLimit) {
			if (gameController.SpendMana(selectedMonster.GetComponent<Unit>().Mana)) {
				monsterManager.CreateUnit(selectedMonster);
			}
		}
	}

	public void BuyLand() {
		if (landManager.landCount < landManager.landList.Length) {
			if (gameController.SpendMana(landManager.landCost[landManager.landCount - 1])) {
				landManager.UnlockLand(landManager.landCount);
				landManager.landCount++;
			}
		}
		SetLand();
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

				gameController.GainMana(landManager.landSale[landManager.landCount - 1]);
				landManager.LockLand(landManager.landCount);
				landManager.landCount--;
			}
		}
		SetLand();
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
		SetLand();
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
		MonsterDescription.text = monster.UnitDescription;
		MonsterStrength.text = monster.UnitStats.Strength.ToString();
		MonsterAgility.text = monster.UnitStats.Agility.ToString();
		MonsterDefence.text = monster.UnitStats.Defence.ToString();
		MonsterHealth.text = (monster.UnitStats.Constitution * 10f).ToString();

		switch (monster.PrimaryStat) {
			case (Unit.StatType.Agility):
				MonsterPrimaryStat.text = "Primary Stat: <b>Agility</b>\n\n";
				MonsterPrimaryStat.text += "Agility enables units to land critical hits on their opponents\n\n";
				MonsterPrimaryStat.text += "Strong versus Strength units";
				break;
			case (Unit.StatType.Strength):
				MonsterPrimaryStat.text = "Primary Stat: <b>Strength</b>\n\n";
				MonsterPrimaryStat.text += "Strength determines the amount of damage of each hit the unit is able to dish out to opponents\n\n";
				MonsterPrimaryStat.text += "Strong versus Defence units";
				break;
			case (Unit.StatType.Defence):
				MonsterPrimaryStat.text = "Primary Stat: <b>Defence</b>\n\n";
				MonsterPrimaryStat.text += "Defence helps to reduce the damage of incoming attacks, allowing units to survive longer\n\n";
				MonsterPrimaryStat.text += "Strong versus Agility units";
				break;
		}

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

	public void SetLand() {
		if (landManager.landCost[landManager.landCount - 1] == int.MaxValue) {
			LandBuyText.text = "---";
		} else {
			LandBuyText.text = "Buy Land (" + landManager.landCost[landManager.landCount - 1] + ")";
		}

		if (landManager.landSale[landManager.landCount - 1] == int.MaxValue) {
			LandSellText.text = "---";
		} else {
			LandSellText.text = "Sell Land (" + landManager.landSale[landManager.landCount - 1] + ")";
		}

		//LandBonusText.text = "Current End of Wave Bonus: " + landManager.landBonus[landManager.landCount - 1];
	}

	public void ResetShop() {
		MonsterName.text = "";
		MonsterDescription.text = "";
		MonsterStrength.text = "";
		MonsterAgility.text = "";
		MonsterDefence.text = "";
		MonsterHealth.text = "";

		MonsterPrimaryStat.text = "";

		MonsterPortrait.sprite = null;

		BuyButton.enabled = false;
		BuyButton.GetComponentInChildren<TextMeshProUGUI>().text = "---";
		UnlockButton.enabled = false;
		UnlockButton.GetComponentInChildren<TextMeshProUGUI>().text = "---";

		selectedMonster = null;
	}
}
