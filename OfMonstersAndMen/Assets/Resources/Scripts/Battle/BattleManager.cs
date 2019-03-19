using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
	private GameController gameController;

	private void Start() {
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}

	public static float StrengthModifier = 0.8f;
	public static float AgilityModifier = 0.6f;
	public static float DefenseModifier = 0.3f;

	public void Fight(Unit attacker, Unit defender) {
		float incomingDamage = attacker.GetAttackValue() * StrengthModifier;
		float criticalDamage = attacker.GetCriticalValue();
		float damageResisted = defender.GetDefenseValue() * DefenseModifier;

		if (attacker.GetCriticalChance() <= defender.GetCriticalChance()) {
			criticalDamage = 0;
		}

		// Prevent negative damage
		float finalDamage = Mathf.Max(0, incomingDamage + criticalDamage - damageResisted);

		defender.TakeDamage(finalDamage);

		gameController.UpdateEvent(attacker.UnitName + " hits " + defender.UnitName + " for " + finalDamage);
	}

	public IEnumerator Battle(List<Unit> monsters, List<Unit> men) {
		bool isMonsterAttack = true;

		while (monsters.Count > 0 && men.Count > 0) {
			Unit monsterUnit = monsters[Random.Range(0, monsters.Count)];
			Unit manUnit = men[Random.Range(0, men.Count)];

			if (isMonsterAttack) {
				Fight(monsterUnit, manUnit);
				if (manUnit.Health < 0) {
					gameController.UpdateEvent(manUnit.UnitName + " defeated");
					men.Remove(manUnit);
					manUnit.DestroyUnit();
				}
			} else {
				Fight(manUnit, monsterUnit);
				if (monsterUnit.Health < 0) {
					gameController.UpdateEvent(monsterUnit.UnitName + " defeated");
					monsters.Remove(monsterUnit);
					monsterUnit.DestroyUnit();
				}
			}
			isMonsterAttack = !isMonsterAttack;
			yield return new WaitForSeconds(1);
		}
		yield return null;
	}
}
