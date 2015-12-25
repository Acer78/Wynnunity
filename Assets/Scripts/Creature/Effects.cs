using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections;
using System.Collections.Generic;

public class Effects : MonoBehaviour {

	//0 = duration, 1 = power, 2+ = extras (Regen Speed, etc.)
	public Vector3 Regen;
	public Vector3 ManaRegen;
	public Vector2 Speed;
	public Vector2 Slowness;
	public Vector2 Stun;
	[HideInInspector]
	public float[] Tripping = new float[6];
	public Vector3 Drunk;

	//Health Variables
	private float nextRegen = 0;
	private Health Health;

	//Mana Variables
	private float nextManaRegen = 0;
	private Mana Mana;

	//Drunkness Variables
	private MouseLook LookX;
	private MouseLook LookY;
	private MotionBlur blur;
	public float DrunkStartTime;
	public float DrunkEndTime;
	public float DrunkScale;
	private Vector2 DrunkDir;
	private float DrunkDirChange;
	

	void Start() {
		Tripping = new float[6];
		Health = GetComponent<Health> ();
		if (GetComponent<Mana>()) {
			Mana = GetComponent<Mana>();
		}
		if (GetComponentInChildren<MotionBlur> ()) {
			blur = GetComponentInChildren<MotionBlur>();
			LookX = Player.TurnBody;
			LookY = Player.TurnHead;
			DrunkEndTime = Drunk.x;
		}
	}

	void Update() {

		//Regenerate Health
		if(Regen.y > 0 && Time.time >= nextRegen && Health.health < Health.maxHealth) {
			nextRegen = Time.time + Regen.z;
			Health.health += Regen.y;
			Health.health = Mathf.Clamp (Health.health, 0f, Health.maxHealth);
		}

		//Regenerate Mana
		if(Mana && ManaRegen.y > 0 && Time.time >= nextManaRegen && Mana.MP < Mana.maxMP) {
			nextManaRegen = Time.time + ManaRegen.z;
			Mana.MP += ManaRegen.y;
			Mana.MP = Mathf.Clamp (Mana.MP, 0f, Mana.maxMP);
		}

		//Trip Reset
		if (!TrippyEffect.Trippy.activated) {
			Tripping[0] = 0;
			Tripping [1] = 0;
			Tripping [2] = 0;
			Tripping [3] = 0;
			Tripping [4] = 0;
		}

		//Drunk Effects
		if (blur) {
			if (Drunk.x > 0) {
				float timeElapsed = DrunkEndTime - DrunkStartTime - Drunk.x;
				DrunkScale = Mathf.PingPong ((timeElapsed / (DrunkEndTime - DrunkStartTime)) * 2, 1);
				if (blur.enabled) {
					blur.blurAmount = DrunkScale * Drunk.y;
					if(Time.time > DrunkDirChange) {
						DrunkDirChange = Time.time + Random.Range (2f, 4.5f);
						DrunkDir.x = Random.Range (0, 2);
						DrunkDir.y = Random.Range (0, 2);
						if(DrunkDir.x == 0)
							DrunkDir.x = -1;
						if(DrunkDir.y == 0)
							DrunkDir.y = -1;
					}
					float rotX;
					float rotY;
					rotX = Random.Range (0, DrunkScale * Drunk.z * 0.05f)*DrunkDir.x;
					rotY = Random.Range (0, DrunkScale * Drunk.z * 0.05f)*DrunkDir.y;
					LookX.rotationX += rotX;
					LookY.rotationY += rotY;
				} else {
					blur.enabled = true;
				}
			} else {
				if (blur.enabled) {
					blur.enabled = false;
				}
			}
		}

		//Health Time
		if (Regen.x > 0)
			Regen.x -= Time.deltaTime;
		else if (Regen.y != 0)
			Regen.y = 0;

		//Mana Time
		if (ManaRegen.x > 0)
			ManaRegen.x -= Time.deltaTime;
		else if (ManaRegen.y != 0)
			ManaRegen.y = 0;

		//Speed Time
		if (Speed.x > 0)
			Speed.x -= Time.deltaTime;
		else if (Speed.y != 0)
			Speed.y = 0;

		//Slowness Time
		if (Slowness.x > 0)
			Slowness.x -= Time.deltaTime;
		else if (Slowness.y != 0)
			Slowness.y = 0;

		//Stun Time
		if (Stun.x > 0)
			Stun.x -= Time.deltaTime;
		else if (Stun.y != 0)
			Stun.y = 0;

		//Trip Time
		if (Tripping[0] > 0)
			Tripping[0] -= Time.deltaTime;

		//Drunk Time
		if (Drunk.x > 0)
			Drunk.x -= Time.deltaTime;
		else if (Drunk.y != 0) {
			Drunk.y = 0;
			Drunk.z = 0;
		}
	}

	public void Apply(string effect, float[] parameters) {

		if (effect == "Heal") {
			if (parameters [1] >= Regen.y) {
				Regen.x = parameters [0];
				Regen.y = parameters [1];
				Regen.z = parameters [2];
			}
		} else if (effect == "Mana") {
			if (parameters [1] >= ManaRegen.y) {
				ManaRegen.x = parameters [0];
				ManaRegen.y = parameters [1];
				ManaRegen.z = parameters [2];
			}
		} else if (effect == "Speed") {
			if (parameters [1] >= Speed.y) {
				Speed.x = parameters [0];
				Speed.y = parameters [1];
			}
		} else if (effect == "Slowness") {
			if (parameters [1] >= Slowness.y) {
				Slowness.x = parameters [0];
				Slowness.y = parameters [1];
			}
		} else if (effect == "Stun") {
			if (parameters [1] >= Stun.y) {
				Stun.x = parameters [0];
				Stun.y = parameters [1];
			}
		} else if (effect == "Trip") {
			if (parameters [2] > Tripping [2] || parameters [4] > Tripping [4]) {
				Tripping [0] = parameters [0];
				Tripping [1] = parameters [1];
				Tripping [2] = parameters [2];
				Tripping [3] = parameters [3];
				Tripping [4] = parameters [4];
				Tripping [5] = parameters [5];
				TrippyEffect.Trippy.ToggleTrip (false);
				TrippyEffect.Trippy.ToggleTrip (parameters);
			}
		} else if (effect == "TripNoFOV") {
			if (parameters [2] > Tripping [2] || parameters [4] > Tripping [4]) {
				Tripping [0] = parameters [0];
				Tripping [1] = parameters [1];
				Tripping [2] = parameters [2];
				Tripping [3] = parameters [3];
				Tripping [4] = parameters [4];
				Tripping [5] = parameters [5];
				TrippyEffect.Trippy.ToggleTrip (false);
				TrippyEffect.Trippy.ToggleTrip (parameters, false);
			}
		} else if (effect == "Drunkness") {
			if(parameters[1] > Drunk.y) {
				Drunk.x = parameters[0];
				Drunk.y = parameters[1];
				Drunk.z = parameters[2];
				DrunkStartTime = Time.time;
				DrunkEndTime = Time.time + parameters[0];

			}
		}
	}
}
