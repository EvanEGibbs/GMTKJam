using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Controller2D))]
[RequireComponent (typeof(PlayerInput))]
public class Player : MonoBehaviour {

	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float timeToJumpApex = .4f;
	public float accelerationTimeAirborne = .1f;
	public float accelerationTimeGrounded = .1f;
	public float decelerationTimeAirborne = 0f;
	public float decelerationTimeGrounded = 0f;
	public float moveSpeed = 6;
	public float shieldSpeedModifier = 1.5f;
	public float floatMovementModifier = 0.4f;
	public float shieldSlamMovement = -55f;

	public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallLeap;

	public GameObject sideShield;
	public GameObject topShield;
	public GameObject bottomShield;
	public GameObject currentCheckpoint;
	public BlackMask blackMask;

	BoxCollider2D sideShieldCollider;
	BoxCollider2D topShieldCollider;
	BoxCollider2D bottomShieldCollider;

	public float wallSlideSpeedMax = 3;
	public float wallStickTime = .25f;
	public float shieldSlamDelay = 0.2f;
	public float superJumpMultiplier = 1.2f;

	float timeToWallUnstick;
	float gravity; //calculated via jump height and timeToJumpApex
	float maxJumpVelocity;
	float minJumpVelocity;

	Vector3 velocity;
	float velocityXSmoothing;

	Controller2D controller;
	public Vector2 directionalInput;

	List<GameObject> allCheckpoints = new List<GameObject>();
	bool shieldButtonDown = false;
	float initialMoveSpeed;
	float initialXScale;
	bool wallSliding;
	int wallDirX;
	bool jumpInputDown;
	bool facingRight = true;
	Animator playerAnimator;
	bool floating = false;
	bool shieldSlam = false;
	float shieldSlamTimer = 0;
	bool playerControl = true;

	void Start () {
		controller = GetComponent<Controller2D>();
		playerAnimator = GetComponentInChildren<Animator>();

		//explanation of the following math: https://www.youtube.com/watch?v=PlT44xr0iW0&t=9s at around 6 minutes
		//can move the following code to update to mess with parameters in real time for when getting the right gravity, jumpping, etc.
		gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		//math here: https://www.youtube.com/watch?v=rVfR14UNNDo&t=2s
		minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
		//Debug.Log("Gravity " + gravity + " jump velocity: " + maxJumpVelocity);
		initialMoveSpeed = moveSpeed;
		initialXScale = transform.localScale.x;

		sideShieldCollider = sideShield.GetComponent<BoxCollider2D>();
		topShieldCollider = topShield.GetComponent<BoxCollider2D>();
		bottomShieldCollider = bottomShield.GetComponent<BoxCollider2D>();

		sideShieldCollider.enabled = false;
		topShieldCollider.enabled = false;
		bottomShieldCollider.enabled = false;

		foreach (GameObject checkpoint in GameObject.FindGameObjectsWithTag("CheckPoint")) {
			allCheckpoints.Add(checkpoint);
		}
	}

	void Update() {

		if (playerControl) {
			ShieldMovementChecks();
			CalculateVelocity();
			HandleWallSliding();
			ShieldActivations();

			controller.Move(velocity * Time.deltaTime, jumpInputDown, directionalInput);

			if (controller.collisions.above || controller.collisions.below) {
				if (controller.collisions.slidingDownMaxSlope) { //sliding down maximum slope
					velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
				} else { //reset velocity if hiting a block from above, or landing on one from below
					velocity.y = 0;
				}
			}

			UpdateAnimations();

			jumpInputDown = false; //so the jumpInputDown variable is only set to be true for one frame when the jump button is pressed
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "CheckPoint") {
			foreach (GameObject checkpoint in allCheckpoints) {
				CheckPoint checkPointClass = checkpoint.GetComponent<CheckPoint>();
				if (checkpoint.transform.position != collision.transform.position) {
					checkPointClass.LowerFlag();
					print("Thing");
				} else {
					checkPointClass.RaiseFlag();
					currentCheckpoint = checkpoint;
				}
			}
		}
	}
	private void OnTriggerStay2D(Collider2D collision) {
		if (collision.tag == "Through") {
			print("It's happening");
			shieldSlamTimer = .2f;
		}
	}

	private void ShieldActivations() {
		if (moveSpeed == initialMoveSpeed) {
			sideShieldCollider.enabled = false;
		}
		else if (floating || shieldSlam) {
			sideShieldCollider.enabled = false;
		}
		else{
			sideShieldCollider.enabled = true;
		}

		if (floating) {
			topShieldCollider.enabled = true;
		} else {
			topShieldCollider.enabled = false;
		}

		if (shieldSlam) {
			bottomShieldCollider.enabled = true;
		} else {
			bottomShieldCollider.enabled = false;
		}
	}

	private void ShieldMovementChecks() {
		if (!shieldButtonDown && controller.collisions.below) {
			moveSpeed = initialMoveSpeed;
		}
		if (controller.collisions.below) {
			floating = false;
			shieldSlam = false;
		}
		if (directionalInput.y == -1 && !controller.collisions.below && !wallSliding && !floating) {
			if (shieldSlamTimer <= 0) {
				shieldSlam = true;
			} else {
				shieldSlam = false;
			}
		}
		if (floating) {
			shieldSlam = false;
		}
		shieldSlamTimer -= Time.deltaTime;
	}

	void UpdateAnimations() {
		playerAnimator.SetBool("grounded", controller.collisions.below);
		playerAnimator.SetBool("movingX", (Mathf.Abs(velocity.x) > 0.1) ? true : false);
		playerAnimator.SetBool("positiveY", (velocity.y > 0.1) ? true : false);
		playerAnimator.SetBool("negativeY", (velocity.y < 0.1) ? true : false);
		playerAnimator.SetBool("wallSlide", wallSliding);
		playerAnimator.SetBool("usingShield", (moveSpeed == initialMoveSpeed) ? false : true);
		playerAnimator.SetBool("shieldFloat", floating);
		playerAnimator.SetBool("shieldBounce", shieldSlam);

		if (controller.collisions.below || floating) {
			if (directionalInput.x == 1) {
				facingRight = true;
			} else if (directionalInput.x == -1) {
				facingRight = false;
			}
		}
		if (facingRight) {
			transform.localScale = new Vector3(initialXScale, transform.localScale.y, transform.localScale.z);
		} else {
			transform.localScale = new Vector3(initialXScale * -1, transform.localScale.y, transform.localScale.z);
		}
	}

	//BUTTON INPUTS

	public void SetDirectionalInput(Vector2 input) { //from playerInput class
		directionalInput = input;
	}

	public void OnJumpInputDown() { //from playerInput class
		shieldSlamTimer = .2f;
		if (!controller.collisions.above) { //if there's nothing directly above you, you can jump
			jumpInputDown = true;
			//wall jumping
			if (wallSliding) { //Jump off of the wall
				if (wallDirX == directionalInput.x) { //Wall jump climb
					timeToWallUnstick = 0;
					velocity.x = -wallDirX * wallJumpClimb.x;
					velocity.y = wallJumpClimb.y;
					
				} else if (directionalInput.x == 0) { //jump off of wall
					timeToWallUnstick = 0;
					velocity.x = -wallDirX * wallJumpOff.x;
					velocity.y = wallJumpOff.y;
				} else { //leap off of wall
					timeToWallUnstick = 0;
					velocity.x = -wallDirX * wallLeap.x;
					velocity.y = wallLeap.y;
				}
				if (wallDirX == -1) {
					facingRight = true;
				} else {
					facingRight = false;
				}
				playerAnimator.SetTrigger("jump");
			}

			if (controller.collisions.below) {
				//jump while sliding down a steep slope
				if (controller.collisions.slidingDownMaxSlope) {
					if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x)) { //not jumping against max slope
						velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
						velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
						playerAnimator.SetTrigger("jump");
					}
				}
				//jump off of ground normally if not going to jump down a through platform
				else if (!controller.collisions.readyToFallThrough) {
					velocity.y = maxJumpVelocity;
					playerAnimator.SetTrigger("jump");
				}
			}
		}
		if (!controller.collisions.below && !wallSliding) {
			floating = true;
		}
	}
	public void SuperJump() {
		shieldSlam = false;
		shieldSlamTimer = .3f;
		velocity.y = maxJumpVelocity * superJumpMultiplier;
		playerAnimator.SetTrigger("jump");
	}
	public void OnJumpInputUp() { //for variable jump height
		if (velocity.y > minJumpVelocity) {
			velocity.y = minJumpVelocity;
		}
		floating = false;
	}

	public void OnShieldInputDown() {
		shieldButtonDown = true;
		moveSpeed = initialMoveSpeed * shieldSpeedModifier;
	}
	public void OnShieldInputUp() {
		shieldButtonDown = false;
	}

	private void CalculateVelocity() {
		float targetVelocity = directionalInput.x * moveSpeed;

		if (targetVelocity == 0 || Mathf.Sign(targetVelocity) != Mathf.Sign(velocity.x) && velocity.x != 0) { //if coming to a stop or moving in the opposite direction, use deceleration time
			velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocity, ref velocityXSmoothing, (controller.collisions.below) ? decelerationTimeGrounded : decelerationTimeAirborne);
		} else { //otherwise, use acceleration time
			velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocity, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
		}
		
		velocity.y += gravity * Time.deltaTime;
		if (floating) {
			velocity.y *= floatMovementModifier;
		}
		if (shieldSlam) {
			velocity.y = shieldSlamMovement;
		}
	}

	private void HandleWallSliding() {
		wallDirX = (controller.collisions.left) ? -1 : 1;
		wallSliding = false;
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below) {
			moveSpeed = initialMoveSpeed * shieldSpeedModifier;
			wallSliding = true;
			controller.collisions.fallingThroughPlatform = null; //if fell through a platform then started a wall slide, can jump on the platform again.
			if (timeToWallUnstick > 0) {
				if (controller.collisions.left) {
					facingRight = true;
				} else {
					facingRight = false;
				}
			}
			floating = false;
			shieldSlam = false;

			if (velocity.y < -wallSlideSpeedMax) {
				velocity.y = -wallSlideSpeedMax;
			}

			if (timeToWallUnstick > 0) { //stick to wall if not holding the button long enough
				velocityXSmoothing = 0;
				velocity.x = 0;

				if (directionalInput.x != wallDirX && directionalInput.x != 0) { //holding button to unstick from wall
					timeToWallUnstick -= Time.deltaTime;
				} else { //restick to wall if not holding button towards wall
					timeToWallUnstick = wallStickTime;
				}
			} else { //restick to wall if not pressing either direction
				timeToWallUnstick = wallStickTime;
			}
		}
	}
	public void DeathScene() {
		playerControl = false;
		playerAnimator.SetBool("Respawning", true);
	}
	public void Respawn() {
		playerAnimator.SetBool("Respawning", false);
		transform.position = currentCheckpoint.transform.position;
		playerControl = true;
	}
	public void CallBlackMask() {
		blackMask.MoveMask();
	}
}
