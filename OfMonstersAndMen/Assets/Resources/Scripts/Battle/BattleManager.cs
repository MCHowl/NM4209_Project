using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
	private float waitDuration = 1;

	private GameController gameController;

	private void Start() {
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}

	public static float StrengthModifier = 0.8f;
	public static float AgilityModifier = 0.3f;
	public static float DefenseModifier = 0.5f;

	private static float BonusMultiplier = 1.2f;
	private static float WeaknessMultiplier = 0.8f;

	public void Fight(Unit attacker, Unit defender) {
		float incomingDamage = attacker.GetAttackValue() * StrengthModifier;
		float criticalDamage = attacker.GetCriticalValue() * AgilityModifier;
		float damageResistance =  1f - DefenseModifier * (Mathf.Min(1, ((float)defender.GetDefenseValue() / 100f)));

		if (attacker.GetCriticalChance() <= defender.GetCriticalChance()) {
			criticalDamage = 0;
		}

		// Prevent negative damage and Round to prevent floating point errors
		float finalDamage = Mathf.Round(Mathf.Max(0, (incomingDamage + criticalDamage) * damageResistance) * 10f) / 10f;

		// Apply Bonus/Weakness multipliers
		switch(attacker.PrimaryStat){
			case (Unit.StatType.Agility):
				if (defender.PrimaryStat == Unit.StatType.Strength) {
					finalDamage *= BonusMultiplier;
				} else if (defender.PrimaryStat == Unit.StatType.Defence) {
					finalDamage *= WeaknessMultiplier;
				}
				break;
			case (Unit.StatType.Strength):
				if (defender.PrimaryStat == Unit.StatType.Defence) {
					finalDamage *= BonusMultiplier;
				} else if (defender.PrimaryStat == Unit.StatType.Agility) {
					finalDamage *= WeaknessMultiplier;
				}
				break;
			case (Unit.StatType.Defence):
				if (defender.PrimaryStat == Unit.StatType.Agility) {
					finalDamage *= BonusMultiplier;
				} else if (defender.PrimaryStat == Unit.StatType.Strength) {
					finalDamage *= WeaknessMultiplier;
				}
				break;	
		}

		defender.TakeDamage(finalDamage);
		if (attacker.Type == Unit.UnitType.Man) {
			gameController.UpdateEvent("<color=\"red\">" + attacker.UnitName + "</color> hits <color=\"green\">" + defender.UnitName + "</color> for <color=\"orange\">" + finalDamage + "</color>");
		} else {
			gameController.UpdateEvent("<color=\"green\">" + attacker.UnitName + "</color> hits <color=\"red\">" + defender.UnitName + "</color> for <color=\"orange\">" + finalDamage + "</color>");
		}
	}

	public IEnumerator Battle(List<Unit> monsters, List<Unit> men) {
		bool isMonsterAttack = true;

		while (monsters.Count > 0 && men.Count > 0) {
			Unit monsterUnit = monsters[Random.Range(0, monsters.Count)];
			Unit manUnit = men[Random.Range(0, men.Count)];

			if (isMonsterAttack) {
				Fight(monsterUnit, manUnit);
				if (manUnit.Health < 0) {
					gameController.UpdateEvent("<color=\"red\">" + manUnit.UnitName + "</color> defeated");
					gameController.ConvertToMana(manUnit);
					men.Remove(manUnit);
					manUnit.DestroyUnit();
				}
			} else {
				Fight(manUnit, monsterUnit);
				if (monsterUnit.Health < 0) {
					gameController.UpdateEvent("<color=\"green\">" + monsterUnit.UnitName + "</color> defeated");
					monsters.Remove(monsterUnit);
					monsterUnit.DestroyUnit();
				}
			}
			isMonsterAttack = !isMonsterAttack;
			yield return new WaitForSeconds(waitDuration);
		}
		yield return null;
	}

	public void SetWaitDuration(float newDuration) {
		if (waitDuration > 0) {
			waitDuration = newDuration;
		}
	}
}
