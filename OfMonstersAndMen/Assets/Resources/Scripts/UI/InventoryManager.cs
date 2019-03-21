using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour {

	Canvas inventoryCanvas;
	GameController gameController;
	MonsterManager monsterManager;
	LandManager landManager;

	public TextMeshProUGUI monsterInfo;
	public Button[] monsterButtons;

	private Unit currentMonster;

    void Awake() {
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		monsterManager = GameObject.FindGameObjectWithTag("MonsterManager").GetComponent<MonsterManager>();
		landManager = GameObject.FindGameObjectWithTag("LandManager").GetComponent<LandManager>();

		inventoryCanvas = GetComponent<Canvas>();
		CloseInventory();
	}

	public void GetMonster(int i) {
		if (i < monsterManager.Inventory.monsterUnits.Count) {
			currentMonster = monsterManager.Inventory.monsterUnits[i];
			monsterInfo.text = "Name: " + currentMonster.UnitName;
		}
	}

	public void PlaceMonster() {
		if (currentMonster != null) {
			if (landManager.PlaceMonster(currentMonster)) {
				monsterManager.RetrieveUnit(currentMonster);
			}
		}
	}

	public void SellMonster() {
		if (currentMonster != null) {
			monsterManager.RetrieveUnit(currentMonster);
			currentMonster.DestroyUnit();
			gameController.mana += 10;
		}
	}

	public void OpenInventory() {
		inventoryCanvas.enabled = true;

		currentMonster = null;
		monsterInfo.text = "";
		for (int i = 0; i < monsterManager.Inventory.monsterUnits.Count; i++) {
			monsterButtons[i].GetComponent<Image>().sprite = monsterManager.Inventory.monsterUnits[i].UnitProtrait;
		}
	}

	public void CloseInventory() {
		inventoryCanvas.enabled = false;

		foreach (Button button in monsterButtons) {
			button.GetComponent<Image>().sprite = null;
		}
	}
}
