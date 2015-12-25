using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyTargetting : MonoBehaviour {

	public Enemy Mob;
	public Transform player;
	public Vector3 playerPos;
	public Vector3 localVel;
	public float ViewRange = 16;
	private float nextCheckTime;
	public Vector3 blockCheck;
	public float blockCheckDistance;
	public bool blockInFront;
	private float minJumpDelay = 0.05f;
	private float nextJump = 0;
	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		blockCheck.y = -GetComponent<Collider> ().bounds.extents.y * 0.975f;
		blockCheckDistance = GetComponent<Collider> ().bounds.extents.z * 1.25f;
		if (!Mob && transform.GetComponent<Enemy> ())
			Mob = transform.GetComponent<Enemy> ();
		if (!Mob && transform.root.GetComponent<Enemy> ())
			Mob = transform.root.GetComponent<Enemy>();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (Time.time > nextCheckTime && !Mob.disableWalk) {
			RaycastHit hit;
			if (Physics.Raycast (transform.position + blockCheck, transform.forward, out hit, blockCheckDistance, Player.SpawnLOS)) {
				blockInFront = true;
			} else {
				blockInFront = false;
			}
		}

		if(blockInFront && player && Mob.GroundCheck2.checkSlope(40f) && !rb.isKinematic && Time.time >= nextJump && Mob.GroundCheck.chckdist())
		{
			nextJump = Time.time + minJumpDelay;
			rb.velocity = Vector3.zero;
			rb.AddForce(Vector3.up*Mathf.Sqrt(2*-Physics.gravity.y*Mob.jumpHeight), ForceMode.VelocityChange);
		}

		if (player) {
			Mob.player = player.GetComponent<Player> ();
			if (player.root.GetComponent<Health> ().health == 0)
				player = null;
		}
		if (player && Mob.GetComponent<Health>().health != 0 && !Mob.disableWalk) {
			playerPos = player.position;
			playerPos.y = Mob.transform.position.y;
			if(!Mob.disableTurn)
			{
				Mob.transform.LookAt (playerPos);
			}
			if(Vector3.Distance(Mob.transform.position, player.position) > Mob.minDist && !Mob.inSomething && (Mob.GroundCheck2.checkSlope(40f) || Mob.GroundCheck2.compareHeights ()))
			{
				localVel = Quaternion.FromToRotation(transform.forward, Vector3.forward) * Mob.rb.velocity;
				localVel.y = 0;
				Mob.rb.AddForce(transform.right*-localVel.x + transform.forward*Mathf.Clamp (Mob.trueSpeed, 0, Mathf.Clamp (Mob.trueSpeed - localVel.z, 0, Mob.trueSpeed)), ForceMode.VelocityChange);
				if(!Mob.WalkAnim.isPlaying && Mob.trueSpeed != 0)
					Mob.WalkAnim.Play ();
			}
		} else if(Mob.GetComponent<Health>().health != 0 && !Mob.disableWalk)
		{
			Vector3 changeVel = Mob.rb.velocity;
			changeVel.y = 0;
			Mob.rb.AddForce(-changeVel, ForceMode.VelocityChange);
		}

		if (Time.time >= nextCheckTime) {
			nextCheckTime = Time.time + 0.25f;
			float Closest = ViewRange + 1;
			Player ClosestPlayer = null;
			foreach(Player checkPlayer in PlayerManager.Players) {
				if(checkPlayer.health.health != 0) {
					float Distance = Vector3.Distance(checkPlayer.transform.position, transform.position);
					if(Distance > 128 && !Mob.persistent) {
						Destroy (Mob.gameObject);
					}
					if(Distance <= ViewRange) {
						if(Distance < Closest) {
							Closest = Distance;
							ClosestPlayer = checkPlayer;
						}
					}
				}
			}
			if(ClosestPlayer != null)
				player = ClosestPlayer.transform;
			else
				player = null;
		}
	}
}
