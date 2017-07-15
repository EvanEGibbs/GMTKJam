﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour {

	public float[] shotIntervals;

	Animator plantAnimator;
	int currentShot = 0;
	float timer = 0f;

	private void Start() {
		plantAnimator = GetComponentInChildren<Animator>();
	}

	void Update () {
		timer += Time.deltaTime;
		if (timer >= shotIntervals[currentShot]) {
			Shoot();
			timer = 0;
			currentShot += 1;
			if (currentShot > shotIntervals.Length - 1) {
				currentShot = 0;
			}
		}
	}
	void Shoot() {
		plantAnimator.SetTrigger("Shoot");
	}
}