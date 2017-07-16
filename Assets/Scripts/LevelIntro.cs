using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelIntro : MonoBehaviour {

	public int levelNumber = 0;
	Animator animator;

	void Start () {
		animator = GetComponent<Animator>();
		if (levelNumber == 1) {
			animator.SetTrigger("Level1");
		}
		if (levelNumber == 2) {
			animator.SetTrigger("Level2");
		}
		if (levelNumber == 3) {
			animator.SetTrigger("Level3");
		}
		if (levelNumber == 4) {
			animator.SetTrigger("Level4");
		}
	}

	public void DestroySelf() {
		Destroy(gameObject);
	}
}
