using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour {

	//private Rigidbody rigidbody = GetComponent<Rigidbody>();

	public Inventory inventory;

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	private float sensitivityX = 15F;
	private float sensitivityY = 15F;
	public float baseSensitivityX = 15F;
	public float baseSensitivityY = 15F;
	
	public float minimumX = -360F;
	public float maximumX = 360F;
	
	public float minimumY = -60F;
	public float maximumY = 60F;
	
	public float rotationX = 0F;
	public float rotationY = 0F;
	
	Quaternion originalRotation;

	public bool LockCursor = true;

	void Start ()
	{
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
		originalRotation = transform.localRotation;
		ApplyOptions ();
	}

	void Update ()
	{
		originalRotation.z = transform.localRotation.z;
		if (Input.GetKeyDown (KeyCode.E)) {
			LockCursor = !LockCursor;
			if(inventory)
				inventory.toggleInv(!inventory.InvPanel.activeSelf);
		}

		if (!LockCursor) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		} else {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;

			if (axes == RotationAxes.MouseXAndY) {
				// Read the mouse input axis
				rotationX += Input.GetAxis ("Mouse X") * sensitivityX;
				rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;
				
				rotationX = ClampAngle (rotationX, minimumX, maximumX);
				rotationY = ClampAngle (rotationY, minimumY, maximumY);
				
				Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
				Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, -Vector3.right);
				
				transform.localRotation = originalRotation * xQuaternion * yQuaternion;
			} else if (axes == RotationAxes.MouseX) {
				rotationX += Input.GetAxis ("Mouse X") * sensitivityX;
				rotationX = ClampAngle (rotationX, minimumX, maximumX);
				
				Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
				transform.localRotation = originalRotation * xQuaternion;
			} else {
				rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;
				rotationY = ClampAngle (rotationY, minimumY, maximumY);
				
				Quaternion yQuaternion = Quaternion.AngleAxis (-rotationY, Vector3.right);
				transform.localRotation = originalRotation * yQuaternion;
			}
		}
	}
	
	public static float ClampAngle (float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp (angle, min, max);
	}

	public void ApplyOptions() {
		sensitivityX = Options.Sensitivity * baseSensitivityX;
		sensitivityY = Options.Sensitivity * baseSensitivityY;
	}
}
