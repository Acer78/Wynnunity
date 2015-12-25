using UnityEngine;
using System.Collections;

public class TrailFX : MonoBehaviour {

	private ParticleSystemRenderer FX;

	void Start () {
		ApplyOptions ();
	}

	public void ApplyOptions () {
		FX = GetComponent<ParticleSystemRenderer> ();
		if(Options.ThreeDParticles)
			FX.renderMode = ParticleSystemRenderMode.Mesh;
		else
			FX.renderMode = ParticleSystemRenderMode.Billboard;
	}
}
