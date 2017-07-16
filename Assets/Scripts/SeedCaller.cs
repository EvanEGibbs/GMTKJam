using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedCaller : MonoBehaviour {

	public void CallActualDestruction() {
		GetComponentInParent<Seed>().ActuallyDestroyThis();
	}
}
