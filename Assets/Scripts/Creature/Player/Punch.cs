using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Punch : MonoBehaviour {

	public Transform heldModel;
	public Animation punchAnim;
	public string Itemname = "";
	public int ItemdamageMin = 1;
	public int ItemdamageMax = 1;
	public float Itemrange = 3f;
	public float Itemknockback = 1f;
	public float ItemCooldown = 0.3f;
	public Item heldItem;
	public Color hitCol;
	private float nextHitTime = 0f;
	private Walk walk;
	public float WalkKnockbackMult = 1.5f;
	public float SprintKnockbackMult = 2.25f;
	public float knockMult = 1f;
	public Vector2 knockDirectionMult = Vector2.one;
	public IDInfo WpnIDInfo;
	public static AudioSource[] hitSounds;
	public LayerMask hittableLayers;

	// Use this for initialization
	void Start () {
		hitSounds = GetComponents<AudioSource> ();
		walk = GetComponent<Walk> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (1)) {
			if(heldItem is Item_Consumable) {
				Item_Consumable heldCons = (Item_Consumable) heldItem;
				InvStack ConsumableStack = Player.Inventory.Hotbar[Player.Inventory.CurrentHotbarSlot];
				foreach(effectInfo effect in heldCons.effects)
					Player.Effects.Apply (effect.type, effect.stats);
				if(ConsumableStack.item == heldCons) {
					Player.Inventory.DropItem (ConsumableStack, 1, true);
				}

			} else {
				Interact ();
			}
		}
		if (Input.GetMouseButtonDown (0)) {
			if(Time.time >= nextHitTime)
			{
				if(heldItem is Item_Weapon && ((Item_Weapon) heldItem).lvlMin <= XP.Level)
				{
					nextHitTime = Time.time + ((Item_Weapon) heldItem).hitCooldown;
					Hit ();
					punchAnim.Stop ();
					punchAnim.Play();
				} else if (!(heldItem is Item_Weapon)){
					nextHitTime = Time.time + ItemCooldown;
					Hit ();
					punchAnim.Stop ();
					punchAnim.Play();
				}
			}
		}
	}
	void Hit () {
		RaycastHit hit;
		if (heldItem is Item_Weapon) {
			Item_Weapon heldWeapon = (Item_Weapon)heldItem;
			if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit, heldWeapon.range, hittableLayers)) {
				if (hit.transform != transform) {
					if(hit.transform.GetComponent<Health>()) {
						if (!walk.moving) {
							knockMult = 1;
						} else if (walk.speed == walk.baseSpeed)
							knockMult = WalkKnockbackMult;
						else if (walk.speed == walk.sprintSpeed)
							knockMult = SprintKnockbackMult;
						if (hit.transform.GetComponent<Enemy> ()) {
							hit.transform.GetComponent<Enemy> ().disableWalk = true;
						}
						heldWeapon.DealDamage (hit.transform.GetComponent<Health>(), transform, knockDirectionMult, heldWeapon.IDs);

					}
				}
			}
		} else if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit, Itemrange, hittableLayers)) {
			if (hit.transform != transform) {
				if(hit.transform.GetComponent<Health> () && hit.transform.GetComponent<Health> ().health != 0) {
					if (!walk.moving) {
						knockMult = 1;
					} else if (walk.speed == walk.baseSpeed)
						knockMult = WalkKnockbackMult;
					else if (walk.speed == walk.sprintSpeed)
						knockMult = SprintKnockbackMult;
					if (hit.transform.GetComponent<Enemy> ()) {
						hit.transform.GetComponent<Enemy> ().disableWalk = true;
					}
					Item_Weapon heldWeapon = new Item_Weapon("", 1, 1, 0, 1, 1);
					heldWeapon.DealDamage (hit.transform.GetComponent<Health>(), transform, knockDirectionMult, null);

				}
			}
		}
	}

	void ChangeEnemyCol(Color hc, List<MeshRenderer> rends) {
		foreach (MeshRenderer rend in rends) {
			rend.material.color = hc;
			if (rend.transform.root.GetComponent<Enemy> ()) {
				rend.transform.root.GetComponent<Enemy> ().defColourTime = Time.time + 0.5f;
				if (rend.transform.root.GetComponent<Enemy> ().FX)
					rend.transform.root.GetComponent<Enemy> ().FX.Play ();
			}
		}
	}

	public static void PlayHitSound() {
		hitSounds [Random.Range (0, hitSounds.Length)].Play ();
	}

	public void changeHeldItem(Item item) {
		heldItem = item;
		Transform heldParent = heldModel.parent;
		Destroy (heldModel.gameObject);
		heldModel = ((GameObject)Instantiate (heldItem.model)).transform;
		heldModel.SetParent (heldParent);
		heldModel.localPosition = heldItem.model.transform.localPosition;
		heldModel.localRotation = heldItem.model.transform.localRotation;
		punchAnim = heldModel.GetComponent<Animation>();
		if (item is Item_Weapon) {
			heldModel.GetComponent<WeaponInfo>().weapon = heldItem as Item_Weapon;
		}
	}

	public void Interact() {
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit, 4)) {
			if(hit.collider.tag == "Interactable") {
				if(hit.collider.GetComponent<NPC>()) {
					if(hit.collider.GetComponent<QuestNPC>()) {
						if(!hit.collider.GetComponent<QuestNPC>().talking) {
							hit.collider.GetComponent<QuestNPC>().Talk (GameManager.currentQuest);
						}
					} else if(hit.collider.GetComponent<NPCTalking>()) {
						if(!hit.collider.GetComponent<NPCTalking>().talking) {
							StartCoroutine (hit.collider.GetComponent<NPCTalking>().Talk ());
						}
					} 
				}
			}
		}
	}
}
