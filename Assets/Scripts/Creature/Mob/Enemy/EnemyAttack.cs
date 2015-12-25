using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {
	
	public Enemy enemy;
	public EnemyTargetting enemyMovement;
	public Item_Weapon Weapon;
	public int minDmg = 1;
	public int maxDmg = 1;
	public float range = 3;
	public float knockback = 3.5f;
	public float hitCooldown = 0.5f;
	private float nextHitTime = 0;
	public GameObject projectile;
	public Transform fireTransform;
	public Vector2 knockMult = Vector2.one;

	public bool hasLOS;

	void Awake () {
		if (!enemy)
			enemy = GetComponent<Enemy> ();
		Weapon = new Item_Weapon ("", minDmg, maxDmg, range, knockback, hitCooldown);
		if (!enemyMovement)
			enemyMovement = GetComponentInChildren<EnemyTargetting> ();
	}

	void Update () {

		RaycastHit hit;
		if (enemyMovement.player && !enemy.HP.dead) {
			if (Physics.Raycast (transform.position, enemyMovement.player.position - transform.position, out hit, Mathf.Infinity, Player.EnemyLOS)) {
				if (hit.transform.root.tag == "Player")
					hasLOS = true;
				else {
					hasLOS = false;
				}
			} else {
				hasLOS = false;
			}
		}


		if (!enemy.HP.dead && enemyMovement.player && enemy.player && hasLOS && Vector3.Distance (transform.position, enemyMovement.player.position) < Weapon.range && Time.time > nextHitTime && enemy.player.GetComponent<Health>().health != 0) 
		{
			nextHitTime = Time.time + hitCooldown;
			if(!enemy.HP.dead && !projectile){
				Weapon.DealDamage (enemy.player.health, transform, knockMult, GetComponent<IDInfo>());
			}
			else if(!enemy.HP.dead) {
				GameObject proj = (GameObject) Instantiate (projectile, fireTransform.position, transform.rotation);
				proj.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
				proj.GetComponent<Projectile>().stats = Weapon;
				Projectile projStats = proj.GetComponent<Projectile>();
				projStats.knockMult = knockMult;
				projStats.IDs = GetComponent<IDInfo>();
				projStats.isEnemy = true;
			}
		}
	}

	public static void shakeCam () {
		Camera.main.transform.parent.Rotate (0, 0, -5);
		Player.CamRevertTime = Time.time + 0.05f;
	}
}
