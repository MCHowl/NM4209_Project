using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour {

	private GameController gameController;
	private MonsterManager monsterManager;
	private LandManager landManager;

	private Vector3 originalMonsterPosition;
	private Land originalMonsterLand;
	private Unit currentMonster;

    void Start() {
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		monsterManager = GameObject.FindGameObjectWithTag("MonsterManager").GetComponent<MonsterManager>();
		landManager = GameObject.FindGameObjectWithTag("LandManager").GetComponent<LandManager>();
    }

    void Update() {
		if (currentMonster == null) {
			// On "Drag" Start
			if (Input.GetMouseButtonDown(0)) {
				currentMonster = RaycastMonster();
				if (currentMonster != null) {
					// Set invalid "Drop" position
					originalMonsterPosition = currentMonster.transform.position;

					//Verify Original Land
					originalMonsterLand = RaycastLand();
					if (originalMonsterLand == null) {
						Debug.LogWarning("Monster not tied to appropriate land");
					} else if (!originalMonsterLand.monsterUnits.Contains(currentMonster)) {
						Debug.LogWarning("Monster not tied to appropriate land");
					}
				}
			}
		} else {
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePosition.z = 0;
			currentMonster.MoveTo(mousePosition);

			if (Input.GetMouseButtonUp(0)) {
				Land land = RaycastLand();
				if (land != null) {
					if (land.canAddMonster()) {
						originalMonsterLand.RemoveMonster(currentMonster);
						land.AddMonster(currentMonster);
					} else {
						currentMonster.MoveTo(originalMonsterPosition);
					}
				} else {
					currentMonster.MoveTo(originalMonsterPosition);
				}

				// Reset Values
				currentMonster = null;
				originalMonsterLand = null;
			}
		}
    }

	RaycastHit2D[] GetRaycast() {
		Vector3 mousePostion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		return Physics2D.RaycastAll(mousePostion, Vector2.zero);
	}

	Unit RaycastMonster() {
		foreach (RaycastHit2D obj in GetRaycast()) {
			Unit monster = obj.transform.GetComponent<Unit>();
			
			if (monster != null) {
				if (monster.Type == Unit.UnitType.Monster) {
					return monster;
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
