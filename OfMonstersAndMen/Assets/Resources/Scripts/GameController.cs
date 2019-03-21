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

	public int EndOfWaveMana = 20;
	[HideInInspector]
	public int mana = 30;
	[HideInInspector]
	public int wave = 1;
	[HideInInspector]
	public bool isWaveRunning = false;

	private List<Unit> NextWave;
	public Land WaveHoldingArea;

	void Awake() {
		waveManager = GameObject.FindGameObjectWithTag("WaveManager").GetComponent<WaveManager>();
		landManager = GameObject.FindGameObjectWithTag("LandManager").GetComponent<LandManager>();
    }

	void Start() {
		NextWave = waveManager.SpawnWave();
		WaveHoldingArea.DisplayMen(NextWave);
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
			isWaveRunning = true;
			landManager.StartWave(NextWave);
		}
	}

	public void EndWave(List<Unit> men) {
		UpdateEvent("Wave Ended");

		waveManager.DespawnWave(men);
		mana += EndOfWaveMana;
		wave++;

		NextWave = waveManager.SpawnWave();
		WaveHoldingArea.DisplayMen(NextWave);
		isWaveRunning = false;
	}

	public void UpdateEvent(string incomingText) {
		eventText.text = incomingText;
	}

}
