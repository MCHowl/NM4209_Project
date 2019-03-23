using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour {

	Canvas shopCanvas;
	GameController gameController;
	MonsterManager monsterManager;

    // Start is called before the first frame update
    void Start() {
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		monsterManager = GameObject.FindGameObjectWithTag("MonsterManager").GetComponent<MonsterManager>();

		shopCanvas = GetComponent<Canvas>();
		CloseShop();
    }


	public void BuyUnit(GameObject monster) {
		if (gameController.SpendMana(monster.GetComponent<Unit>().Mana)) {
			monsterManager.CreateUnit(monster);
		}
	}

	public void OpenShop() {
		shopCanvas.enabled = true;
	}

	public void CloseShop() {
		shopCanvas.enabled = false;
	}
}
