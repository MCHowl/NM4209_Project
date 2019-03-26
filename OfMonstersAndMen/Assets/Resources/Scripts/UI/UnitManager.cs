using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitManager : MonoBehaviour
{
	Canvas unitManagerCanvas;
	UpgradeManager upgradeManager;
	GameController gameController;
	MonsterManager monsterManager;

	public Image MonsterPortrait;
	public TextMeshProUGUI MonsterName;
	public TextMeshProUGUI MonsterStrength;
	public TextMeshProUGUI MonsterAgility;
	public TextMeshProUGUI MonsterDefence;
	public TextMeshProUGUI MonsterHealth;

	public Button Heal;
	public Button Sell;
	public Button Upgrade1;
	public Button Upgrade2;

	List<Unit> upgradesAvailable;
	Unit selectedMonster;
	Land sourceLand;

	void Start() {
		unitManagerCanvas = GetComponent<Canvas>();

		upgradeManager = GameObject.FindGameObjectWithTag("ShopManager").GetComponent<UpgradeManager>();
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		monsterManager = GameObject.FindGameObjectWithTag("MonsterManager").GetComponent<MonsterManager>();

		upgradesAvailable = new List<Unit>();

		CloseUnitManager();
	}

	public void HealUnit() {
		if (gameController.SpendMana(selectedMonster.Mana / 2)) {
			selectedMonster.Health = selectedMonster.UnitStats.Constitution * 10;
			MonsterHealth.text = selectedMonster.Health.ToString();

			Heal.enabled = false;
			Heal.GetComponentInChildren<TextMeshProUGUI>().text = "Cannot Heal";

		}
	}

	public void SellUnit() {
		sourceLand.RemoveMonster(selectedMonster);
		gameController.ConvertToMana(selectedMonster);
		selectedMonster.DestroyUnit();
		CloseUnitManager();
	}

	public void UpgradeUnit(int i) {
		if (gameController.SpendMana(upgradesAvailable[0].Mana - selectedMonster.Mana)) {
			sourceLand.RemoveMonster(selectedMonster);
			selectedMonster.DestroyUnit();

			Unit newMonster = Instantiate(upgradesAvailable[i], monsterManager.transform).GetComponent<Unit>();
			sourceLand.AddMonster(newMonster);
			Land temp = sourceLand;

			ResetUnitManager();
			SetUnitManager(newMonster, sourceLand);
			MonsterHealth.text = (newMonster.UnitStats.Constitution * 10f).ToString();
		}
	}

	public void OpenUnitManager() {
		unitManagerCanvas.enabled = true;
	}

	public void CloseUnitManager() {
		ResetUnitManager();
		unitManagerCanvas.enabled = false;
	}

	public void SetUnitManager(Unit monster, Land land) {
		selectedMonster = monster;
		sourceLand = land;

		MonsterPortrait.sprite = selectedMonster.UnitProtrait;
		MonsterName.text = selectedMonster.UnitName;
		MonsterStrength.text = selectedMonster.UnitStats.Strength.ToString();
		MonsterAgility.text = selectedMonster.UnitStats.Agility.ToString();
		MonsterDefence.text = selectedMonster.UnitStats.Defence.ToString();
		MonsterHealth.text = selectedMonster.Health.ToString();

		Sell.GetComponentInChildren<TextMeshProUGUI>().text = "Sell (" + (int)(selectedMonster.Mana * 0.75f) + ")";

		if (selectedMonster.Health < selectedMonster.UnitStats.Constitution * 10) {
			Heal.enabled = true;
			Heal.GetComponentInChildren<TextMeshProUGUI>().text = "Heal (" + selectedMonster.Mana/2 + ")";
		}

		for (int i = 0; i < upgradeManager.UnitUpgrades.Length; i++) {
			if (upgradeManager.UnitUpgrades[i].GetComponent<Unit>().UnitName == selectedMonster.UnitName) {
				if (upgradeManager.followingUpgrades[i].Count > 0) {
					Unit upgradedMonster = upgradeManager.UnitUpgrades[upgradeManager.followingUpgrades[i][0]].GetComponent<Unit>();
					upgradesAvailable.Add(upgradedMonster);
					if (upgradeManager.isUnlocked[upgradeManager.followingUpgrades[i][0]]) {
						Upgrade1.enabled = true;
						Upgrade1.GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade to " + upgradedMonster.UnitName + " (" + (upgradedMonster.Mana - selectedMonster.Mana) + ")";
					}
				}
				if (upgradeManager.followingUpgrades[i].Count > 1) {
					Unit upgradedMonster = upgradeManager.UnitUpgrades[upgradeManager.followingUpgrades[i][1]].GetComponent<Unit>();
					upgradesAvailable.Add(upgradedMonster);
					if (upgradeManager.isUnlocked[upgradeManager.followingUpgrades[i][1]]) {
						Upgrade2.enabled = true;
						Upgrade2.GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade to " + upgradedMonster.UnitName + " (" + (upgradedMonster.Mana - selectedMonster.Mana) + ")";
					}
				}
			}
		}
	}

	public void ResetUnitManager() {
		MonsterName.text = "";
		MonsterStrength.text = "";
		MonsterAgility.text = "";
		MonsterDefence.text = "";
		MonsterHealth.text = "";

		MonsterPortrait.sprite = null;

		Heal.GetComponentInChildren<TextMeshProUGUI>().text = "---";
		Heal.enabled = false;
		Heal.GetComponentInChildren<TextMeshProUGUI>().text = "Cannot Heal";
		Upgrade1.enabled = false;
		Upgrade1.GetComponentInChildren<TextMeshProUGUI>().text = "---";
		Upgrade2.enabled = false;
		Upgrade2.GetComponentInChildren<TextMeshProUGUI>().text = "---";

		selectedMonster = null;
		upgradesAvailable.Clear();
	}
}
