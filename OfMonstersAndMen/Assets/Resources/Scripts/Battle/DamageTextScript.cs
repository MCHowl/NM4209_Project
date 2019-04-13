using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextScript : MonoBehaviour {

	public float value;
	TMPro.TextMeshPro valueText;

    void Start() {
		valueText = GetComponent<TMPro.TextMeshPro>();
		Destroy(gameObject, 1f);
    }

    void Update() {
		valueText.text = ((int)value).ToString();
		valueText.alpha -= 0.01f;
    }
}
