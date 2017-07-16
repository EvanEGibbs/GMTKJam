using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackMask : MonoBehaviour {

	Animator animator;

	void Start () {
		animator = GetComponent<Animator>();
		animator.SetTrigger("Enter");
	}

	public void MoveMask() {
		animator.SetTrigger("Transition");
	}
	public void ExitLevel() {
		animator.SetTrigger("Exit");
	}
	public void LoadLevel(string levelName) {
		SceneManager.LoadScene(levelName);
	}
}
