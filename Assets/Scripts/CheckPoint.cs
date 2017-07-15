using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

	Animator checkPointAnimator;

	void Start () {
		checkPointAnimator = GetComponentInChildren<Animator>();
	}

	public void RaiseFlag() {
		checkPointAnimator.SetBool("NotCheckpoint", false);
		checkPointAnimator.SetTrigger("RaiseFlag");
	}

	public void LowerFlag() {
		checkPointAnimator.SetBool("NotCheckpoint", true);
	}
}
