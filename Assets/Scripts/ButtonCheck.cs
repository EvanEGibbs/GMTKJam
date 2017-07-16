using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCheck : MonoBehaviour {

	public BlackMask blackMask;

	void Update () {
		if (Input.GetButtonDown("Jump")) {
			blackMask.ExitLevel();
		}
	}
}
