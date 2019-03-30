using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {
	GameController gameController;

	[HideInInspector]
	public GameObject[] manUnits;

    void Awake() {
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

	public List<Unit> SpawnWave() {
		List<Unit> men = new List<Unit>();

		// Spawn Men based on Wave
		men.Add(GetMan());

		if (gameController.wave > 2) {
			men.Add(GetMan());
		}

		if (gameController.wave > 4) {
			men.Add(GetMan());
		}

		// Set Level
		foreach (Unit man in men) {
			man.SetLevel(gameController.wave / 2);
		}

		return men;
	}

	public List<Unit> SpawnWave2() {
		List<Unit> men = new List<Unit>();

		if (gameController.wave > 9) {
			men.Add(GetMan());
		}

		if (gameController.wave > 14) {
			men.Add(GetMan());
		}

		if (gameController.wave > 19) {
			men.Add(GetMan());
		}

		// Set Level
		foreach (Unit man in men) {
			man.SetLevel(gameController.wave / 2);
		}

		return men;
	}

	private Unit GetMan() {
		return Instantiate(manUnits[Random.Range(0, manUnits.Length)]).GetComponent<Unit>();
	}

	public void DespawnWave(List<Unit> men) {
		foreach (Unit man in men) {
			man.DestroyUnit();
		}
	}
}
