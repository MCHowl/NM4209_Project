using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
	public class Stats {
		public int Strength;
		public int Dexterity;
		public int Defence;
		public int Constitution;

		public Stats(int str, int dex, int def, int con) {
			Strength = str;
			Dexterity = dex;
			Defence = def;
			Constitution = con;
		}
	}

	public int Health { get; }
	public Stats UnitStats { get; }
	public Sprite UnitProtrait;
}
