using UnityEngine;
using System.Collections;

public class Options : MonoBehaviour {

	public static bool ThreeDSmoke = false;
	public static bool ThreeDParticles = true;
	public static bool AdvancedShadows = true;
	public static float Sensitivity = 0.5f;
	public static float chatWidth = 0.325f;

	public void ThreeDSmokeFX (bool isEnabled) {
		ThreeDSmoke = isEnabled;
	}

	public void ToggleThreeDParticles (bool isEnabled) {
		ThreeDParticles = isEnabled;
		TrailFX[] Trails = FindObjectsOfType<TrailFX> ();
		foreach (TrailFX trail in Trails)
			trail.ApplyOptions ();
	}

	public void AdvShadows (bool isEnabled) {
		AdvancedShadows = isEnabled;
		AdvancedLight[] Lights = FindObjectsOfType<AdvancedLight> ();
		foreach (AdvancedLight light in Lights)
			light.ApplyOptions ();
	}

	public void ChangeSensitivity(float factor) {
		Sensitivity = factor/10;
		MouseLook[] MouseInputs = FindObjectsOfType<MouseLook> ();
		foreach (MouseLook mouseInput in MouseInputs)
			mouseInput.ApplyOptions ();
	}

	public void ChangeChatWidth(float factor) {
		chatWidth = factor/10*500;
		FindObjectOfType<Chat>().ChangeWidth (chatWidth);
	}
}
