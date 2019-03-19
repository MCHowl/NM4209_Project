using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandManager : MonoBehaviour {

	public int initialLand = 3;
	public Land[] landList;
	private int waveProgress = 0;

	void Start() {
		Initialise();
	}

	private void Initialise() {
		for (int i = 0; i < initialLand; i++) {
			landList[i].isBought = true;
		}
	}

	public void spawnWave(Unit[] manUnits) {

	}

	public void moveWave() {
		waveProgress++;
	}

	public Unit[] GetMonsters(Land land) {
		return land.monsterUnits;
	}

	public Unit[] GetMen (Land land) {
		return land.manUnits;
	}
}
