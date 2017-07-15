using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackMask : MonoBehaviour {

	Animator animator;
	bool moving = true;

	void Start () {
		animator = GetComponent<Animator>();
		animator.SetTrigger("Enter");
	}

	public void MoveMask() {
		animator.SetTrigger("Transition");
	}
}
