using UnityEngine;
using System.Collections;

public class Walk : MonoBehaviour {

	public Player player;
	public float baseSpeed = 3.5f;
	public float sprintSpeed = 4.2f;
	public float speed = 3.5f;
	public float jumpHeight = 1.25f;
	public Rigidbody rb;
	public GroundDetect GD;
	public GroundDetect GD2;
	public float minJumpDelay = 0.05f;
	public float nextJump = 0;
	public float ButtonCooler;
	public float ButtonCount;
	public float SprintTapTime = 0.375f;
	public float defFOV = 70;
	public float sprintFOV = 80;
	public bool moving = false;
	private float defFric = 1;
	public bool CanTrip;
	private float nextCheck;
	private Effects effects;
	public float speedMult;
	public float trueSpeed;

	// Use this for initialization
	void Start () {
		if (!player)
			player = GetComponent<Player> ();
		if(!rb)
			rb = GetComponent<Rigidbody>();
		effects = Player.Effects;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (effects.Stun.y != 0) {
			speedMult = 0;
		} else if (effects.Slowness.y != 0 || effects.Speed.y != 0) {
			speedMult = 1 - effects.Slowness.y*0.1f + effects.Speed.y*0.1f;
		} else {
			speedMult = 1;
		}

		trueSpeed = speed * speedMult;

		if (trueSpeed == 0) {
			GetComponent<Collider> ().material.staticFriction = 20;
			GetComponent<Collider> ().material.dynamicFriction = 20;
		}
		else if (moving) {
			GetComponent<Collider> ().material.staticFriction = 0;
			GetComponent<Collider> ().material.dynamicFriction = 0;
		} else {
			GetComponent<Collider> ().material.staticFriction = defFric;
			GetComponent<Collider> ().material.dynamicFriction = defFric;
		}
		Vector3 localVel = transform.InverseTransformDirection(rb.velocity);
		float ClampedVelchangeNX = Mathf.Clamp (-trueSpeed - localVel.x, -trueSpeed, 0);
		float ClampedVelchangePX = Mathf.Clamp (trueSpeed - localVel.x, 0, trueSpeed);
		float ClampedVelchangeNZ = Mathf.Clamp (-trueSpeed - localVel.z, -trueSpeed, 0);
		float ClampedVelchangePZ = Mathf.Clamp (trueSpeed - localVel.z, 0, trueSpeed);
		if (moving && GD2.checkSlope (40f))
			rb.AddRelativeForce (Mathf.Clamp (Input.GetAxis ("Horizontal") * trueSpeed, ClampedVelchangeNX, ClampedVelchangePX), 0, Mathf.Clamp (Input.GetAxis ("Vertical") * trueSpeed, ClampedVelchangeNZ, ClampedVelchangePZ), ForceMode.VelocityChange);

		if (Input.GetKey ("space") && GD.chckdist () && effects.Stun.y == 0 && GD2.checkSlope (40f) && Time.time >= nextJump) {
			nextJump = Time.time + minJumpDelay;
			rb.velocity = Vector3.zero;
			rb.AddForce (Vector3.up * Mathf.Sqrt (2 * -Physics.gravity.y * jumpHeight), ForceMode.VelocityChange);
		}
	}
	void Update() {

		if (Time.time >= nextCheck) {
			nextCheck = Time.time + 0.375f;
			if (GetComponentInChildren<TrippyEffect> ())
				CanTrip = true;
			else
				CanTrip = false;
		}

		if (Input.GetKeyDown (KeyCode.W)){
			if (ButtonCooler > 0 && ButtonCount == 1 && player.mana.MP > player.mana.StopSprintThreshold){
				speed = sprintSpeed;
				if(!CanTrip || CanTrip && (!TrippyEffect.Trippy.activated || !TrippyEffect.Trippy.changeFOV))
					Camera.main.fieldOfView = sprintFOV;
			}else{
				ButtonCooler = SprintTapTime; 
				ButtonCount += 1;
				speed = baseSpeed;
				if(!CanTrip || CanTrip && (!TrippyEffect.Trippy.activated || !TrippyEffect.Trippy.changeFOV))
					Camera.main.fieldOfView = defFOV;
			}
		}

		if (player.mana.MP <= player.mana.StopSprintThreshold) {
			ButtonCooler = 0;
			speed = baseSpeed;
			if(!CanTrip || CanTrip && (!TrippyEffect.Trippy.activated || !TrippyEffect.Trippy.changeFOV))
				Camera.main.fieldOfView = defFOV;
		}
		
		if ( ButtonCooler > 0 )
		{
			
			ButtonCooler -= Time.deltaTime ;
			
		}else if (ButtonCount != 0) {
			ButtonCount = 0 ;
		}

		if(Input.GetKeyUp (KeyCode.W)) {
			if(!CanTrip || CanTrip && (!TrippyEffect.Trippy.activated || !TrippyEffect.Trippy.changeFOV))
				Camera.main.fieldOfView = defFOV;
			speed = baseSpeed;
		}

		if (Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0) {
			moving = true;
		} else {
			moving = false;
		}
	}
}
