using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
	public static float StrengthModifier = 0.8f;
	public static float AgilityModifier = 0.6f;
	public static float DefenseModifier = 0.3f;

	public static void Fight(Unit attacker, Unit defender) {
		float incomingDamage = attacker.GetAttackValue() * StrengthModifier;
		float criticalDamage = attacker.GetCriticalValue();
		float damageResisted = defender.GetDefenseValue() * DefenseModifier;

		if (attacker.GetCriticalChance() <= defender.GetCriticalChance()) {
			criticalDamage = 0;
		}

		// Prevent negative damage
		float finalDamage = Mathf.Max(0, incomingDamage + criticalDamage - damageResisted);

		defender.TakeDamage(finalDamage);
	}

	public static void Battle(Unit[] monsterUnits, Unit[] manUnits) {
		List<Unit> monsters = new List<Unit>(monsterUnits);
		List<Unit> men = new List<Unit>(manUnits);

		bool isMonsterAttack = true;

		while (monsters.Count > 0 && men.Count > 0) {
			Unit monsterUnit = monsters[Random.Range(0, monsters.Count)];
			Unit manUnit = men[Random.Range(0, men.Count)];

			if (isMonsterAttack) {
				Fight(monsterUnit, manUnit);
				if (manUnit.Health < 0) {
					Destroy(manUnit.gameObject);
				}
			} else {
				Fight(manUnit, monsterUnit);
				if (monsterUnit.Health < 0) {
					Destroy(monsterUnit.gameObject);
				}
			}
		}
	}
}
