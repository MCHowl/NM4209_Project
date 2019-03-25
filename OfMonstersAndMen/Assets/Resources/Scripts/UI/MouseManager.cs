using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MouseManager : MonoBehaviour {

	private GameController gameController;
	private MonsterManager monsterManager;
	private LandManager landManager;
	private UnitManager unitManager;

	private Vector3 originalMonsterPosition;
	private Land originalMonsterLand;
	private Unit currentMonster;

	public TextMeshProUGUI UnitNameField;
	public TextMeshProUGUI UnitStrengthField;
	public TextMeshProUGUI UnitAgilityField;
	public TextMeshProUGUI UnitDefenceField;
	public TextMeshProUGUI UnitHealthField;

    void Awake() {
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		monsterManager = GameObject.FindGameObjectWithTag("MonsterManager").GetComponent<MonsterManager>();
		landManager = GameObject.FindGameObjectWithTag("LandManager").GetComponent<LandManager>();
		unitManager = GameObject.FindGameObjectWithTag("UnitManager").GetComponent<UnitManager>();
    }

    void Update() {
		if (!gameController.isWaveRunning) {
			DragAndDrop();
		}

		UpdateStats();
    }

	void DragAndDrop() {
		if (currentMonster == null) {
			// On "Drag" Start
			if (Input.GetMouseButtonDown(0)) {
				currentMonster = RaycastUnit();
				if (currentMonster != null) {
					// Set invalid "Drop" position
					originalMonsterPosition = currentMonster.transform.position;

					//Verify Source Land
					originalMonsterLand = RaycastLand();
					if (originalMonsterLand == null) {
						Debug.LogWarning("Monster not tied to appropriate land");
					} else if (!originalMonsterLand.monsterUnits.Contains(currentMonster)) {
						Debug.LogWarning("Monster not tied to appropriate land");
					}
				}
			}
			if (Input.GetMouseButtonDown(1)) {
				// Open Unit Manager
				currentMonster = RaycastUnit();
				originalMonsterLand = RaycastLand();

				if (currentMonster != null && originalMonsterLand != null) {
					unitManager.OpenUnitManager();
					unitManager.ResetUnitManager();
					unitManager.SetUnitManager(currentMonster, originalMonsterLand);
				}

				// Reset Values
				currentMonster = null;
				originalMonsterLand = null;
			}
		} else {
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePosition.z = 0;
			currentMonster.MoveTo(mousePosition);

			// On Drop
			if (Input.GetMouseButtonUp(0)) {
				Land land = RaycastLand();
				if (land != null) {
					// Check Shop
					if (land.CompareTag("Shop")) {
						originalMonsterLand.RemoveMonster(currentMonster);
						gameController.ConvertToMana(currentMonster);
						currentMonster.DestroyUnit();
					}

					// Find Valid Land
					if (land.canAddMonster()) {
						originalMonsterLand.RemoveMonster(currentMonster);
						land.AddMonster(currentMonster);
					} else {
						currentMonster.MoveTo(originalMonsterPosition);
					}
					//Reset Position
				} else {
					currentMonster.MoveTo(originalMonsterPosition);
				}

				// Reset Values
				currentMonster = null;
				originalMonsterLand = null;
			}
		}
	}

	void UpdateStats() {
		Unit unit = RaycastUnit(false);
		if (unit == null) {
			UnitNameField.text = "";
			UnitStrengthField.text = "";
			UnitAgilityField.text = "";
			UnitDefenceField.text = "";
			UnitHealthField.text = "";
		} else {
			UnitNameField.text = unit.UnitName;
			UnitStrengthField.text = unit.UnitStats.Strength.ToString();
			UnitAgilityField.text = unit.UnitStats.Agility.ToString();
			UnitDefenceField.text = unit.UnitStats.Defence.ToString();
			UnitHealthField.text = unit.Health.ToString();
		}
	}

	RaycastHit2D[] GetRaycast() {
		Vector3 mousePostion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		return Physics2D.RaycastAll(mousePostion, Vector2.zero);
	}

	Unit RaycastUnit(bool isOnlyMonster = true) {
		foreach (RaycastHit2D obj in GetRaycast()) {
			Unit unit = obj.transform.GetComponent<Unit>();
			
			if (unit != null) {
				if (unit.Type == Unit.UnitType.Monster) {
					return unit;
				} else if (unit.Type == Unit.UnitType.Man && !isOnlyMonster) {
					return unit;
				}
			}
		}

		return null;
	}

	Land RaycastLand() {
		foreach (RaycastHit2D obj in GetRaycast()) {
			Land land = obj.transform.GetComponent<Land>();

			if (land != null) {
				return land;
			}
		}

		return null;
	}
}
