using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
	public int wave = 1;
	[HideInInspector]
	public bool isWaveRunning = false;

	private bool debugMode = false;

	private AudioSource battleAudio;

	private List<Unit> NextWave;
	private List<Unit> NextWave2;
	public Land WaveHoldingArea;
	public Land WaveHoldingArea2;

	public Button ShopButton;
	public Button WaveButton;

	public Canvas GameLostCanvas;
	public Canvas GameWonCanvas;

	void Awake() {
		waveManager = GameObject.FindGameObjectWithTag("WaveManager").GetComponent<WaveManager>();
		landManager = GameObject.FindGameObjectWithTag("LandManager").GetComponent<LandManager>();
		battleAudio = GameObject.FindGameObjectWithTag("AudioManager").transform.Find("Battle").GetComponent<AudioSource>();
    }

	void Start() {
		NextWave = waveManager.SpawnWave();
		NextWave2 = waveManager.SpawnWave2();
		WaveHoldingArea.DisplayMen(NextWave);
		WaveHoldingArea2.DisplayMen(NextWave2);
	}

	void Update() {
		manaText.text = mana.ToString();
		waveText.text = "Wave: " + wave;

		if (Input.GetKeyDown(KeyCode.D)) {
			debugMode = !debugMode;
		}

		if (debugMode && Input.GetKeyDown(KeyCode.M)) {
			mana += 100;
		}
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
			GainMana((int)(unit.Mana * (unit.Health / (unit.UnitStats.Constitution * 10f)) * 0.75f));
		}
	}

	public void StartWave() {
		if (!isWaveRunning) {
			shopCanvas.enabled = false;
			WaveButton.enabled = false;
			ShopButton.enabled = false;

			isWaveRunning = true;
			battleAudio.Play();
			landManager.StartWave(NextWave);
		}
	}

	public void EndWave(List<Unit> men) {
		if (men.Count > 0) {
			UpdateEvent("Wave Failed");
			battleAudio.Stop();

			if (!debugMode)
			{
				GameLostCanvas.enabled = true;
			} else {
				waveManager.DespawnWave(men);
				if (NextWave2.Count > 0) {
					waveManager.DespawnWave(NextWave2);
				}

				mana += GetEndOfWaveMana();// + landManager.landBonus[landManager.landCount - 1];
				wave++;

				NextWave = waveManager.SpawnWave();
				NextWave2 = waveManager.SpawnWave2();

				WaveHoldingArea.DisplayMen(NextWave);
				WaveHoldingArea2.DisplayMen(NextWave2);

				isWaveRunning = false;

				WaveButton.enabled = true;
				ShopButton.enabled = true;
			}
		} else {
			waveManager.DespawnWave(men);

			// Check if second part of wave is still coming
			if (NextWave2.Count > 0) {
				landManager.StartWave(NextWave2);
			// Else end the wave
			} else {
				UpdateEvent("Wave " + wave + " Cleared!");
				battleAudio.Stop();

				// Check for final wave
				if (wave == 50) {
					GameWonCanvas.enabled = true;
				} else {
					mana += GetEndOfWaveMana();// + landManager.landBonus[landManager.landCount - 1];
					wave++;

					NextWave = waveManager.SpawnWave();
					NextWave2 = waveManager.SpawnWave2();

					WaveHoldingArea.DisplayMen(NextWave);
					WaveHoldingArea2.DisplayMen(NextWave2);

					isWaveRunning = false;

					WaveButton.enabled = true;
					ShopButton.enabled = true;
				}
			}
		}
	}

	public void UpdateEvent(string incomingText) {
		eventText.text = incomingText;
	}

	private int GetEndOfWaveMana() {
		if (wave < 25) {
			return 20;
		} else if (wave < 30) {
			return 40;
		} else if (wave < 35) {
			return 60;
		} else if (wave < 40) {
			return 80;
		} else {
			return 100;
		}
	}
}
