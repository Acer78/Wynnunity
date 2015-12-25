using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections;

public class TrippyEffect : MonoBehaviour {

	public static TrippyEffect Trippy;
	private Camera cam;
	public bool changeFOV;
	public float nextFOV;
	public float prevFOV;
	public float minFOV = 50;
	public float maxFOV = 90;
	public float FOVchangeSpeed = 0.5f;
	public float TripTime = 120;
	public float TripEnd;
	public float nextFovTime;
	private float timeForChange;

	public bool activated;
	private float resetTime;
	private int comboPlace = 0;

	public float maxVignette = 15;
	public float maxChromatic = 750;
	public float maxVortex = 90;
	public float maxColours = 0.075f;
	public float maxFisheye = 1;
	private float durationBase = 120;
	private float vignetteBase = 15;
	private float chromeBase = 750;
	private float vortexBase = 90;
	private float coloursBase = 0.075f;
	private float fisheyeBase = 1;
	private float fovBase = 0.5f;
	private float currentMult;
	private float timeElapsed;

	// Use this for initialization
	void Start () {
		vignetteBase = maxVignette;
		chromeBase = maxChromatic;
		vortexBase = maxVortex;
		durationBase = TripTime;
		fovBase = FOVchangeSpeed;
		fisheyeBase = maxFisheye;
		Trippy = this;
		cam = GetComponent<Camera> ();
		prevFOV = cam.fieldOfView;
	}
	
	// Update is called once per frame
	void Update () {
		if (comboPlace != 0 && resetTime < Time.time) {
			resetTime = Time.time + 1.5f;
			comboPlace = 0;
		}
		if (comboPlace == 0 && Input.GetKeyDown (KeyCode.UpArrow)) {
			comboPlace ++;
			resetTime = Time.time + 1.5f;
		}
		if (comboPlace == 1 && Input.GetKeyDown (KeyCode.UpArrow)) {
			comboPlace ++;
			resetTime = Time.time + 1.5f;
		}
		if (comboPlace == 2 && Input.GetKeyDown (KeyCode.DownArrow)) {
			comboPlace ++;
			resetTime = Time.time + 1.5f;
		}
		if (comboPlace == 3 && Input.GetKeyDown (KeyCode.DownArrow)) {
			comboPlace ++;
			resetTime = Time.time + 1.5f;
		}
		if (comboPlace == 4 && Input.GetKeyDown (KeyCode.LeftArrow)) {
			comboPlace ++;
			resetTime = Time.time + 1.5f;
		}
		if (comboPlace == 5 && Input.GetKeyDown (KeyCode.RightArrow)) {
			comboPlace ++;
			resetTime = Time.time + 1.5f;
		}
		if (comboPlace == 6 && Input.GetKeyDown (KeyCode.LeftArrow)) {
			comboPlace ++;
			resetTime = Time.time + 1.5f;
		}
		if (comboPlace == 7 && Input.GetKeyDown (KeyCode.RightArrow)) {
			comboPlace ++;
			resetTime = Time.time + 1.5f;
		}
		if (comboPlace == 8 && Input.GetKeyDown (KeyCode.B)) {
			comboPlace ++;
			resetTime = Time.time + 1.5f;
		}
		if (comboPlace == 9 && Input.GetKeyDown (KeyCode.A)) {
			comboPlace ++;
			resetTime = Time.time + 1.5f;
		}
		if (comboPlace == 10 && Input.GetKeyDown (KeyCode.Return)) {
			if(!activated) {
				vignetteBase = maxVignette;
				chromeBase = maxChromatic;
				vortexBase = maxVortex;
				durationBase = TripTime;
				coloursBase = maxColours;
				fisheyeBase = maxFisheye;
				fovBase = maxFOV;
			}

			comboPlace = 0;
			resetTime = 0;
			ToggleTrip (!activated, TripTime);

		}

		if (activated && Time.time >= TripEnd) {
			ToggleTrip (false);
		} else if (activated) {
			timeElapsed += Time.deltaTime;
			currentMult = Mathf.PingPong ((timeElapsed / TripTime) * 2, 1);
			if(changeFOV) {
				FOVchangeSpeed = (currentMult*maxVortex)/45f;
				if (Time.time >= nextFovTime) {
					prevFOV = nextFOV;
					if (prevFOV == 0) {
						prevFOV = cam.fieldOfView;
					}
					nextFOV = Random.Range (minFOV, maxFOV);
					nextFovTime = Time.time + Mathf.Abs (prevFOV - nextFOV) / FOVchangeSpeed;
					timeForChange = nextFovTime - Time.time;
				}
				cam.fieldOfView = Mathf.Lerp (prevFOV, nextFOV, 1 - (nextFovTime - Time.time) / timeForChange);
			}
			GetComponent<VignetteAndChromaticAberration>().intensity = currentMult*maxVignette;
			GetComponent<VignetteAndChromaticAberration>().chromaticAberration = currentMult*maxChromatic;
			GetComponent<Twirl>().angle = currentMult*maxVortex;
			GetComponent<ColorCorrectionRamp>().Intensity = currentMult*maxColours;
			GetComponent<ContrastEnhance> ().intensity = currentMult*maxColours*2;
			GetComponent<Fisheye>().strengthX = currentMult*maxFisheye;
			GetComponent<Fisheye>().strengthY = currentMult*maxFisheye;
		}
	}

	public void ToggleTrip(bool Activated, float duration = 0) {

		activated = Activated;
		timeElapsed = 0;
		if (Activated) {
			TripEnd = Time.time + duration;
			TripTime = duration;
			GetComponent<VignetteAndChromaticAberration> ().enabled = true;
			GetComponent<Twirl> ().enabled = true;
			GetComponent<ColorCorrectionRamp>().enabled = true;
			GetComponent<ContrastEnhance> ().enabled = true;
			GetComponent<Fisheye>().enabled = true;
		} else if (!Activated) {
			GetComponent<VignetteAndChromaticAberration>().enabled = false;
			GetComponent<Twirl>().enabled = false;
			GetComponent<ColorCorrectionRamp>().enabled = false;
			GetComponent<ContrastEnhance> ().enabled = false;
			GetComponent<Fisheye>().enabled = false;
			maxVignette = vignetteBase;
			maxChromatic = chromeBase;
			maxVortex = vortexBase;
			maxColours = coloursBase;
			maxFisheye = fisheyeBase;
			TripTime = durationBase;
		}
	}

	public void ToggleTrip(float[] parameters, bool FoVChange = true) {
		
		activated = true;
		timeElapsed = 0;
		GetComponent<VignetteAndChromaticAberration> ().enabled = true;
		GetComponent<Twirl> ().enabled = true;
		GetComponent<ColorCorrectionRamp>().enabled = true;
		GetComponent<ContrastEnhance> ().enabled = true;
		GetComponent<Fisheye> ().enabled = true;
		TripTime = parameters [0];
		TripEnd = Time.time + parameters[0];
		maxVignette = parameters[1];
		maxChromatic = parameters[2];
		maxVortex = parameters[3];
		maxColours = parameters[4];
		maxFisheye = parameters [5];
		changeFOV = FoVChange;

	}
}