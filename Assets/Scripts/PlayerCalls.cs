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
}
