using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

	public string visible_name;
	public int level = 1;
	public float speed = 2;
	public float jumpHeight = 1.25f;
	public float trueSpeed;
	public float minDist = 1f;
	public Health HP;
	public Transform inSomething;
	public Vector3 inSmthPos;
	public float repelForce = 25;
	public Collider groundCol;
	private float defDFric;
	private float defSFric;
	public Rigidbody rb;
	public GroundDetect GroundCheck;
	public GroundDetect GroundCheck2;
	public bool disableWalk = false;
	public float disableTurnTime;
	public bool disableTurn = false;
	public float minWalkRecoverTime = 0.05f;
	public float walkRecoverTime = 0f;
	public float notFallingThreshold = 0.1f;
	public Color defColour;
	public float defColourTime;
	public ParticleSystem FX;
	public Player player;
	public List<MeshRenderer> rends;
	public bool colourChanged = false;
	public Animation WalkAnim;
	public float nextRepelTime;
	private EnemyTargetting view;
	public Effects effects;
	public float speedMult;
	public ParticleSystem StunFX;
	public bool persistent;

	// Use this for initialization
	void Start () {
		if (!GroundCheck2) {
			GameObject instObj = (GameObject) Instantiate(Player.EnemyGC2, GroundCheck.transform.position + GetComponent<Collider>().bounds.extents.z*transform.forward*0.475f, GroundCheck.transform.rotation);
			instObj.transform.SetParent (transform);
			GroundCheck2 = instObj.GetComponent<GroundDetect>();
		}
		trueSpeed = speed;
		if(transform.FindChild ("StunFX")) {
			StunFX = transform.FindChild ("StunFX").GetComponent<ParticleSystem>();
		}
		if (!effects && GetComponent<Effects> ())
			effects = GetComponent<Effects> ();
		else if (!effects)
			effects = (Effects) gameObject.AddComponent<Effects> ();
		if (!view)
			view = GetComponentInChildren<EnemyTargetting> ();
		if (!WalkAnim)
			WalkAnim = GetComponent<Animation> ();
		BodyMarker[] rendMarkers = GetComponentsInChildren<BodyMarker> ();
		foreach (BodyMarker marker in rendMarkers) {
			if(marker.ColourSample) {
				defColour = marker.GetComponent<MeshRenderer>().materials[marker.submat].color;
			}
			rends.Add(marker.GetComponent<MeshRenderer>());
		}

		if (!HP)
			HP = GetComponent<Health> ();
		if (groundCol) {
			defDFric = groundCol.material.dynamicFriction;
			defSFric = groundCol.material.staticFriction;
		}
		if (!rb) {
			rb = GetComponent<Rigidbody>();
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (effects.Stun.y != 0) {
			speedMult = 0;
			if(StunFX && effects.Stun.y == 1)
				StunFX.Play ();
		} else if (effects.Slowness.y != 0 || effects.Speed.y != 0) {
			speedMult = 1 - effects.Slowness.y*0.1f + effects.Speed.y*0.1f;
		} else {
			speedMult = 1;
			if(StunFX)
				StunFX.Stop ();
		}

		trueSpeed = speed * speedMult;

		bool tooFast = (view.localVel.x > speed || view.localVel.z > speed);

		if (Time.time <= defColourTime)
			colourChanged = true;
		else {
			colourChanged = false;
			if (HP.health != 0) {
				revertColour ();
			}
		}
		if (inSomething) {
			if(nextRepelTime <= Time.time && !tooFast) {
				nextRepelTime = Time.time + 0.25f;
				rb.AddForce ((transform.position - inSmthPos).normalized * repelForce);
				if (!WalkAnim.isPlaying)
					WalkAnim.Play ();
				if (groundCol) {
					groundCol.material.staticFriction = 0;
					groundCol.material.dynamicFriction = 0;
				}
			}
		} else if (!disableWalk && GroundCheck.chckdist () && !disableWalk && trueSpeed != 0 && !tooFast) {
			if (groundCol) {
				groundCol.material.staticFriction = 0;
				groundCol.material.dynamicFriction = 0;
			}
		} else if (!disableWalk && GroundCheck.chckdist () && disableWalk || trueSpeed == 0 || tooFast) {
			if (groundCol) {
				groundCol.material.staticFriction = defSFric;
				groundCol.material.dynamicFriction = defDFric;
			}
		} else if (!disableWalk && !GroundCheck.chckdist () && disableWalk && trueSpeed != 0 && !tooFast) {
			if (groundCol) {
				groundCol.material.staticFriction = 1;
				groundCol.material.dynamicFriction = 1;
			}
		}

		if(disableWalk && walkRecoverTime < Time.time - 0.25f)
		{
			walkRecoverTime = Time.time + minWalkRecoverTime;
		}
		if(disableWalk && Time.time >= walkRecoverTime && GetComponent<Rigidbody> ().velocity.y < notFallingThreshold && GroundCheck.chckdist ())
		{
			disableWalk = false;
		}
		if (disableTurnTime < Time.time) {
			disableTurn = false;
		} else {
			disableTurn = true;
		}
	}

	void OnTriggerStay(Collider col) {
		if (!inSomething && HP.health != 0 && (col.tag == "Push" || col.tag == "Player")) {
			if (!col.transform.root.GetComponent<Health> () || col.transform.root.GetComponent<Health> () && col.transform.root.GetComponent<Health> ().health != 0) {
				inSomething = col.transform;
				inSmthPos = transform.position;
				inSmthPos.x = inSomething.position.x;
				inSmthPos.z = inSomething.position.z;
			}
		} else if(inSomething && HP.health != 0 && (col.tag == "Push" || col.tag == "Player") && col.transform.root.GetComponent<Health> () && col.transform.root.GetComponent<Health> ().health == 0)
			inSomething = null;
	}

	void OnTriggerExit(Collider col) {
		if(inSomething && col.transform.root == inSomething.root)
			inSomething = null;
	}

	void revertColour() {
		foreach (MeshRenderer rend in rends) {
			rend.materials[rend.GetComponent<BodyMarker>().submat].color = defColour;
		}
	}
}
