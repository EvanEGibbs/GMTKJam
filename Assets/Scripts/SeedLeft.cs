using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedLeft : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.left * 0.075f);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Through" || collision.tag == "obstacle") {
			DestroyObject(gameObject);
		}
	}
}
