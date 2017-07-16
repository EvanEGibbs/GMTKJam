using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedLeft : Seed {

	void Update () {
		transform.Translate(Vector3.left * 0.075f);
	}
}
