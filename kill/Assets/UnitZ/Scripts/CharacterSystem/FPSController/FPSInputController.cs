//----------------------------------------------
//      UnitZ : FPS Sandbox Starter Kit
//    Copyright © Hardworker studio 2015 
// by Rachan Neamprasert www.hardworkerstudio.com
//----------------------------------------------

using UnityEngine;
using System.Collections;

[RequireComponent (typeof(FPSController))]

// You can change a controller here.
public class FPSInputController : MonoBehaviour
{
	public FPSController FPSmotor;
	public CharacterDriver Driver;

	private Vector2 MousePos = Vector2.zero;

	void Start ()
	{
		FPSmotor = GetComponent<FPSController> ();	
		Driver = GetComponent<CharacterDriver> ();
		Application.targetFrameRate = 60;
	}

	void Awake ()
	{
		MouseLock.MouseLocked = false;
	}

	void Update ()
	{
		if (FPSmotor == null || FPSmotor.character == null)
			return; 
		
		// FPS object. e.g. gun in FPS view
		FPSItemEquipment FPSitem = null;
		
		if (FPSmotor.character.IsMine) {

			var Horizontal = InputManager.GetAxis ("Horizontal");
			var Vertical = InputManager.GetAxis ("Vertical");

			if ((Driver && Driver.DrivingSeat == null) || Driver == null) {
				// move input
//				FPSmotor.Move (new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical")));
				FPSmotor.Move (new Vector3 (Horizontal, 0, Vertical));
				// jump input
				FPSmotor.Jump (Input.GetButton ("Jump"));
			
			} else {
				if (Driver) {
					if (Input.GetKeyDown (KeyCode.F)) {
						Driver.OutVehicle ();
					}
					Driver.Drive (new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")), Input.GetButton ("Jump"));
				}
			}
			// sprint input
			if (Input.GetKey (KeyCode.LeftShift)) {
				FPSmotor.Boost (1.4f);	
			}
			
//			if (MouseLock.MouseLocked) {
			if (true) {
				// aim input work only when mouse is locked
				var CamearH = InputManager.GetAxis ("camear_h");
				var CamearV = InputManager.GetAxis ("camera_v");

				MousePos = new Vector2 (CamearH, CamearV) * 0.5f;

//				FPSmotor.Aim (new Vector2 (Input.GetAxis ("Mouse X"), Input.GetAxis ("Mouse Y")));
				FPSmotor.Aim (MousePos);
				
				// get FPS object
				if (FPSmotor.character.inventory != null && FPSmotor.character.inventory.FPSEquipment != null) {
					FPSitem = FPSmotor.character.inventory.FPSEquipment;
				}


				// if FPS item is exist
				if (FPSitem != null) {
					// fire input


					foreach (int cmd in InputManager.Shoot.ToArray()) {
						if (cmd == 0) {
							FPSitem.Trigger ();
						} else if (cmd == 1) {
							FPSitem.OnTriggerRelease ();
						} else if (cmd == 3) {
							// reload
							if (FPSmotor.character.inventory != null && FPSmotor.character.inventory.FPSEquipment != null)
								FPSmotor.character.inventory.FPSEquipment.Reload ();
						} else if (cmd == 4) {

						}
					}
					InputManager.Shoot.Clear ();

					/*
					if (Input.GetButton ("Fire1")) {
						// press trigger to fire
						FPSitem.Trigger ();
					} else {
						// relesed trigger.
						FPSitem.OnTriggerRelease ();
					}

					// fire 2 input e.g. Zoom
					if (Input.GetButtonDown ("Fire2")) {
						// press trigger 2
						FPSitem.Trigger2 ();
					} else {
						// release trigger 2
						FPSitem.OnTrigger2Release ();
					}
					*/
				}
			}
			

			// interactive input e.g. pickup item
			if (Input.GetKeyDown (KeyCode.F)) {
				if (FPSmotor.character.inventory != null)
					FPSmotor.character.Interactive (FPSmotor.FPSCamera.transform.position, FPSmotor.FPSCamera.transform.forward);
			}
			// reload input
			if (Input.GetKeyDown (KeyCode.R)) {
				if (FPSmotor.character.inventory != null && FPSmotor.character.inventory.FPSEquipment != null)
					FPSmotor.character.inventory.FPSEquipment.Reload ();
			}
			
			/*if (Input.GetKeyDown (KeyCode.E)) {
			// This script is older for older gui
				if (FPSmotor.character.inventory != null) {
					FPSmotor.character.inventory.Toggle = !FPSmotor.character.inventory.Toggle;
					MouseLock.MouseLocked = !FPSmotor.character.inventory.Toggle;
				}
			}*/
			
			// Checking ray. using to detect all object and read an object info. 
			// e.g. when you looking at an item and you will see it name.
			FPSmotor.character.Checking (FPSmotor.FPSCamera.transform.position, FPSmotor.FPSCamera.transform.forward);
		}
	}

}
