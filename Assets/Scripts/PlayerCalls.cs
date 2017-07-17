using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCalls : MonoBehaviour {

	public void CallRespawn() {
		GetComponentInParent<Player>().Respawn();
	}
	public void CallBlackMask() {
		GetComponentInParent<Player>().CallBlackMask();
	}
	public void CallFootstepSound1() {
		GetComponentInParent<Player>().footstepSound1();
	}
	public void CallFootstepSound2() {
		GetComponentInParent<Player>().footstepSound2();
	}
	public void CallDeathSound() {
		GetComponentInParent<Player>().DeathSound();
	}
}
