using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIndicator : MonoBehaviour {
	Unit parentUnit;
	SpriteRenderer healthColor;
	Color newColor;

    void Start() {
		parentUnit = transform.parent.GetComponent<Unit>();
		healthColor = GetComponent<SpriteRenderer>();
		newColor = new Color(0, 0, 0);
    }

    // Update is called once per frame
    void Update() {
		float healthPercentage = parentUnit.Health / (parentUnit.UnitStats.Constitution * 10f);

		if (healthPercentage >= 0.5f) {
			newColor.r = 1f - (healthPercentage - 0.5f) * 2f;
			newColor.g = 1f;
		} else {
			newColor.r = 1f;
			newColor.g = healthPercentage * 2f;
		}

		//print(healthColor.r + "," + healthColor.g);
		healthColor.color = newColor;
    }
}
