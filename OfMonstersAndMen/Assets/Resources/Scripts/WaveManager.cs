using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {
	GameController gameController;

	public GameObject Man_Att;
	public GameObject Man_Agi;
	public GameObject Man_Def;

    void Start() {
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

	public List<Unit> SpawnWave() {
		List<Unit> men = new List<Unit>();

		// Replace Later
		men.Add(Instantiate(Man_Att, transform).GetComponent<Unit>());
		men.Add(Instantiate(Man_Agi, transform).GetComponent<Unit>());
		men.Add(Instantiate(Man_Def, transform).GetComponent<Unit>());

		// Set Level
		foreach (Unit man in men) {
			man.SetLevel(gameController.wave / 2);
		}

		return men;
	}

	public void DespawnWave(List<Unit> men) {
		foreach (Unit man in men) {
			man.DestroyUnit();
		}
	}
}
