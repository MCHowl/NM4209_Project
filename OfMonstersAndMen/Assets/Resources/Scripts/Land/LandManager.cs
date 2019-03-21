using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandManager : MonoBehaviour {

	public int initialLand = 3;
	public Land[] landList;

	private GameController gameController;
	private BattleManager battleManager;

	void Start() {
		for (int i = 0; i < initialLand; i++) {
			landList[i].BuyLand();
		}

		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		battleManager = GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManager>();
	}

	public void UnlockLand() {
		if (initialLand < landList.Length) {
			landList[initialLand].BuyLand();
			initialLand++;
		}
	}

	// Temporary Code
	public bool PlaceMonster(Unit monster) {
		foreach (Land land in landList) {
			if (land.isBought && GetMonsters(land).Count < 3) {
				land.AddMonster(monster);
				return true;
			}
		}
		return false;
	}

	public List<Unit> GetMonsters(Land land) {
		return land.monsterUnits;
	}

	public void StartWave(List<Unit> men) {
		StartCoroutine(RunBattle(men));
	}

	private IEnumerator RunBattle(List<Unit> men) {
		for (int i = 0; i < landList.Length; i++) {
			if (landList[i].isBought && men.Count > 0) {
				landList[i].DisplayMen(men);
				yield return StartCoroutine(battleManager.Battle(GetMonsters(landList[i]), men));
				landList[i].DisplayMonsters();
			} else {
				gameController.EndWave(men);
				landList[i].DisplayMonsters();
				yield break;
			}
		}
		gameController.EndWave(men);
		yield return null;
	}
}
