using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCheck2 : MonoBehaviour {

	public BlackMask blackMask;

	void Update() {
		if (Input.GetButtonDown("Jump")) {
			blackMask.ExitLevel();
		}
		if (Input.GetKeyDown("space")) {
			blackMask.ExitLevel();
		}
	}
}
