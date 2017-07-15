using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedLeft : MonoBehaviour {

	bool harmless = false;

	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.left * 0.075f);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Through" || collision.tag == "obstacle") {
			DestroyThis();
		}
		if (collision.tag == "Shield") {
			harmless = true;
			DestroyThis();
		}
		if (collision.tag == "ShieldBottom") {
			harmless = true;
			DestroyThis();
		}
		if (collision.tag == "Player") {
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
