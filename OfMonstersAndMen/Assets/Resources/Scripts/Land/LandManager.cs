using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandManager : MonoBehaviour {

	public int landCount = 3;
	public int landCost = 30;
	public Land[] landList;

	public Sprite UnlockedSprite;
	public Sprite LockedSprite;
	public Sprite TreasureSprite;

	private GameController gameController;
	private BattleManager battleManager;

	void Start() {
		for (int i = 0; i < landCount; i++) {
			UnlockLand(i);
		}

		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		battleManager = GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManager>();
	}

	public void UnlockLand(int i) {
		landList[i].isBought = true;
		landList[i].UpdateSprite(UnlockedSprite);

		if (i < landList.Length - 1) {
			landList[i+1].UpdateSprite(TreasureSprite);
		}
	}

	public void LockLand(int i) {
		landList[i-1].isBought = false;
		landList[i-1].UpdateSprite(TreasureSprite);
		if (i < landList.Length) {
			landList[i].UpdateSprite(LockedSprite);
		}
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
