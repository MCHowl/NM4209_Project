using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour {
	public TextMeshProUGUI manaText;
	public TextMeshProUGUI waveText;

	public Canvas shopCanvas;

	private WaveManager waveManager;
	private LandManager landManager;

	[HideInInspector]
	public int mana = 30;
	[HideInInspector]
	public int wave = 0;
	[HideInInspector]
	public bool isWaveRunning = false;

	void Start() {
		waveManager = GameObject.FindGameObjectWithTag("WaveManager").GetComponent<WaveManager>();
		landManager = GameObject.FindGameObjectWithTag("LandManager").GetComponent<LandManager>();
    }

    void Update() {
		manaText.text = "Mana: " + mana;
		waveText.text = "Wave: " + wave;
    }

	public bool SpendMana(int cost) {
		if (mana - cost >= 0) {
			mana -= cost;
			return true;
		} else {
			return false;
		}
	}

	public void StartWave() {
		if (!isWaveRunning) {
			wave++;
			isWaveRunning = true;
			landManager.StartWave(waveManager.SpawnWave());
		}
	}

	public void EndWave(List<Unit> men) {
		print("Wave Ended");

		waveManager.DespawnWave(men);
		mana += 20;
		isWaveRunning = false;
	}
}
