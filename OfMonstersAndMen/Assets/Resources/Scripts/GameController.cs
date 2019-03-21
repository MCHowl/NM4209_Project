using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour {
	public TextMeshProUGUI manaText;
	public TextMeshProUGUI waveText;

	public TextMeshProUGUI eventText;

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

	public void GainMana(int gain) {
		if (gain >= 0) {
			mana += gain;
		}
	}

	public void ConvertToMana(Unit unit) {
		if (unit.Type == Unit.UnitType.Man) {
			GainMana(unit.Mana);
		} else if (unit.Type == Unit.UnitType.Monster) {
			GainMana((int)(unit.Mana * unit.Health / (unit.UnitStats.Constitution * 10f)));
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
		UpdateEvent("Wave Ended");

		waveManager.DespawnWave(men);
		mana += 20;
		isWaveRunning = false;
	}

	public void UpdateEvent(string incomingText) {
		eventText.text = incomingText;
	}

}
