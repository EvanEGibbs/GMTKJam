using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour {

	public float[] shotIntervals;
	public float offset = 0;
	public Seed seed;

	Animator plantAnimator;
	int currentShot = 0;
	float timer = 0f;

	private void Start() {
		plantAnimator = GetComponentInChildren<Animator>();
	}

	void Update () {
		if (offset > 0) {
			offset -= Time.deltaTime;
		} else {
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
	}
	void Shoot() {
		plantAnimator.SetTrigger("Shoot");
		GameObject.Instantiate(seed, transform);
	}
}
