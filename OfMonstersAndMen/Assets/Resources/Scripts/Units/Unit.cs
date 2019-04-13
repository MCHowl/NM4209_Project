using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
	public enum UnitType { Monster, Man };
	public enum StatType { Strength, Agility, Defence };

	[System.Serializable]
	public class Stats {
		public int Strength;
		public int Agility;
		public int Defence;
		public int Constitution;

		public Stats(int str, int agi, int def, int con) {
			Strength = str;
			Agility = agi;
			Defence = def;
			Constitution = con;
		}
	}

	// Initialisation Values
	public string UnitName;
	public string UnitDescription;
	public int Mana = 10;
	public UnitType Type;
	public Stats UnitStats;
	public Sprite UnitProtrait;
	public StatType PrimaryStat;

	// The only really important thing
	public float Health { get; set; }

	// Other maybe important things
	private SpriteRenderer spriteRenderer;

	void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = UnitProtrait;

		Health = UnitStats.Constitution * 10;
	}

	public void DestroyUnit() {
		Destroy(gameObject);
	}

	public void MoveTo(Vector3 position) {
		transform.SetPositionAndRotation(position, Quaternion.identity);
	}

	public void SetLevel(int level) {
		if (level >= 0 && Type == UnitType.Man) {
			switch (PrimaryStat) {
				case (StatType.Agility):
					UnitStats.Strength += level;
					UnitStats.Agility += level * 2;
					UnitStats.Defence += level;
					break;
				case (StatType.Defence):
					UnitStats.Strength += level;
					UnitStats.Agility += level;
					UnitStats.Defence += level * 2;
					break;
				case (StatType.Strength):
					UnitStats.Strength += level * 2;
					UnitStats.Agility += level;
					UnitStats.Defence += level;
					break;
			}

			UnitStats.Constitution += level;
			Health = UnitStats.Constitution * 10;
		}
	}

	// Helper Functions
	public int GetAttackValue()
	{
		return GetD6Value() + UnitStats.Strength;
	}

	public int GetDefenseValue() {
		return GetD6Value() + UnitStats.Defence;
	}

	public int GetCriticalValue() {
		return UnitStats.Agility;
	}

	public int GetCriticalChance() {
		return GetD100Value() + UnitStats.Agility;
	}

	private int GetD6Value() {
		return Random.Range(1, 7);
	}

	private int GetD100Value() {
		return Random.Range(1, 100);
	}

	public void TakeDamage(float damage) {
		Health -= damage;
		Health = Mathf.Round(Health * 10) / 10f;
	}
}
