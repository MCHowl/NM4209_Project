using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour {
	public TextMeshProUGUI manaText;
	public TextMeshProUGUI waveText;

	public Canvas shopCanvas;

	public int mana = 30;
	private int wave = 1;

	void Start() {
        
    }

    void Update() {
		manaText.text = "Mana: " + mana;
		waveText.text = "Wave: " + wave;
    }

	void InitialiseGame() {
		
	}

	public bool SpendMana(int cost) {
		if (mana - cost >= 0) {
			mana -= cost;
			return true;
		} else {
			return false;
		}
	}
}
