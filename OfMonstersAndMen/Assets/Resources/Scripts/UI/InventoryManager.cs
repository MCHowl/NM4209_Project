using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour {

	Canvas inventoryCanvas;
	GameController gameController;
	MonsterManager monsterManager;

	public TextMeshProUGUI monsterInfo;
	public Button[] monsterButtons;

	private Unit currentMonster;

    void Awake() {
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		monsterManager = GameObject.FindGameObjectWithTag("MonsterManager").GetComponent<MonsterManager>();

		inventoryCanvas = GetComponent<Canvas>();
		CloseInventory();
	}

	/*private void OnEnable() {
		//if (monsterManager == null) {
		//	return;
		//}

		for (int i = 0; i < monsterManager.Inventory.Count; i++) {
			monsterButtons[i].GetComponent<Image>().sprite = monsterManager.Inventory[i].UnitProtrait;
		}
	}

	private void OnDisable() {
		foreach (Button button in monsterButtons) {
			button.GetComponent<Image>().sprite = null;
		}
	}*/

	public void GetMonster(int i) {
		if (monsterManager.Inventory.Count < i) {
			currentMonster = monsterManager.Inventory[i];
			monsterInfo.text = "Name: " + currentMonster.UnitName;
		}
	}

	public void OpenInventory() {
		inventoryCanvas.enabled = true;
	}

	public void CloseInventory() {
		inventoryCanvas.enabled = false;
	}
}
