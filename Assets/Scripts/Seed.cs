using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour {

	bool harmless = false;

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Through" || collision.tag == "obstacle") {
			DestroyThis();
		}
		else if (collision.tag == "Shield") {
			harmless = true;
			DestroyThis();
		}
		else if (collision.tag == "ShieldBottom") {
			harmless = true;
			collision.GetComponentInParent<Player>().SuperJump();
			DestroyThis();
		}
		else if (collision.tag == "Player") {
			if (!harmless) {
				collision.GetComponent<Player>().DeathScene();
				DestroyThis();
			}
		}
	}

	private void DestroyThis() {
		DestroyObject(gameObject);
	}
}
