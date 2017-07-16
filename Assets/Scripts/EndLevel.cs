using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour {

	public BlackMask blackMask;

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player") {
			blackMask.ExitLevel();
		}
	}
}
