using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandManager : MonoBehaviour {

	public int initialLand = 3;
	public Land[] landList;

	private GameController gameController;
	private BattleManager battleManager;
	private WaveManager waveManager;

	void Start() {
		Initialise();
	}

	private void Initialise() {
		for (int i = 0; i < initialLand; i++) {
			landList[i].isBought = true;
		}

		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		battleManager = GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManager>();
		waveManager = GameObject.FindGameObjectWithTag("WaveManager").GetComponent<WaveManager>();
	}

	public List<Unit> GetMonsters(Land land) {
		return land.monsterUnits;
	}

	public void StartWave(List<Unit> men) {
		StartCoroutine(RunBattle(men));
	}

	private IEnumerator RunBattle(List<Unit> men) {

		print("Start Battle!");
		print(men.Count);

		for (int i = 0; i < landList.Length; i++) {
			if (landList[i].isBought && men.Count > 0) {

				print("Battle Reaches Land " + i);

				landList[i].DisplayMen(men);

				yield return StartCoroutine(battleManager.Battle(GetMonsters(landList[i]), men));

				print("Battle in Land " + i + " complete!");

			} else {
				gameController.isWaveRunning = false;
				waveManager.DespawnWave(men);
				print("Wave Over");
				yield break;
			}
		}
		gameController.isWaveRunning = false;
		waveManager.DespawnWave(men);
		print("Wave Over");
		yield return null;
	}
}
