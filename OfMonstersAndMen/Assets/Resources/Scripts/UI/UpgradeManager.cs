using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
	public GameObject[] UnitUpgrades;
	public bool[] isUnlocked;
	public int[] unlockCost;
	public List<int>[] precedingUpgrades;
	public List<int>[] followingUpgrades;

	private void Awake() {
		InitialiseUnlocks();
		InitialiseUnlockCosts();
		InitialisePrecedingUpgrades();
		InitialiseFollowingUpgrades();
	}

	void InitialiseUnlocks() {
		isUnlocked = new bool[15];
		for (int i = 0; i < isUnlocked.Length; i++) {
			if (i % 5 == 0) {
				isUnlocked[i] = true;
			} else {
				isUnlocked[i] = false;
			}
		}
	}

	void InitialiseUnlockCosts() {
		unlockCost = new int[15];
		for (int i = 0; i < unlockCost.Length; i++) {
			if (i % 5 == 0) {
				unlockCost[i] = 0;
			} else if (i % 5 == 1 || i % 5 == 2) {
				unlockCost[i] = 50;
			} else {
				unlockCost[i] = 100;
			}
		}
	}

	void InitialisePrecedingUpgrades() {
		precedingUpgrades = new List<int>[15];
		precedingUpgrades[0] = new List<int> { };
		precedingUpgrades[1] = new List<int> {0};
		precedingUpgrades[2] = new List<int> {0};
		precedingUpgrades[3] = new List<int> {1};
		precedingUpgrades[4] = new List<int> {2};
		precedingUpgrades[5] = new List<int> { };
		precedingUpgrades[6] = new List<int> {5};
		precedingUpgrades[7] = new List<int> {5};
		precedingUpgrades[8] = new List<int> {6};
		precedingUpgrades[9] = new List<int> {7};
		precedingUpgrades[10] = new List<int> { };
		precedingUpgrades[11] = new List<int> {10};
		precedingUpgrades[12] = new List<int> {10};
		precedingUpgrades[13] = new List<int> {11};
		precedingUpgrades[14] = new List<int> {12};
	}

	void InitialiseFollowingUpgrades() {
		followingUpgrades = new List<int>[15];
		followingUpgrades[0] = new List<int> {1, 2};
		followingUpgrades[1] = new List<int> {3};
		followingUpgrades[2] = new List<int> {4};
		followingUpgrades[3] = new List<int> { };
		followingUpgrades[4] = new List<int> { };
		followingUpgrades[5] = new List<int> {6, 7};
		followingUpgrades[6] = new List<int> {8};
		followingUpgrades[7] = new List<int> {9};
		followingUpgrades[8] = new List<int> { };
		followingUpgrades[9] = new List<int> { };
		followingUpgrades[10] = new List<int> {11, 12};
		followingUpgrades[11] = new List<int> {13};
		followingUpgrades[12] = new List<int> {14};
		followingUpgrades[13] = new List<int> { };
		followingUpgrades[14] = new List<int> { };
	}
}
