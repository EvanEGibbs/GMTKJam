using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour {

	bool harmless = false;
	bool destroyed = false;
	Animator seedAnimator;

	private void Start() {
		seedAnimator = GetComponentInChildren<Animator>();
	}

	void Update() {
		if (!destroyed) {
			transform.Translate(Vector3.left * 0.075f);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (!destroyed) {
			if (collision.tag == "Through" || collision.tag == "obstacle") {
				DestroyThis();
			} else if (collision.tag == "Shield") {
				collision.GetComponentInParent<Player>().BlockSound();
				harmless = true;
				DestroyThis();
			} else if (collision.tag == "ShieldBottom") {
				harmless = true;
				collision.GetComponentInParent<Player>().SuperJump();
				DestroyThis();
			} else if (collision.tag == "Player") {
				if (!harmless) {
					if (!collision.GetComponent<Player>().shieldSlam) {
						collision.GetComponent<Player>().DeathScene();
						DestroyThis();
					}
				}
			}
		}
	}

	private void DestroyThis() {
		destroyed = true;
		seedAnimator.SetTrigger("destroyed");
	}

	public void ActuallyDestroyThis() {
		Destroy(gameObject);
	}
}
