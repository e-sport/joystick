﻿//----------------------------------------------
//      UnitZ : FPS Sandbox Starter Kit
//    Copyright © Hardworker studio 2015 
// by Rachan Neamprasert www.hardworkerstudio.com
//----------------------------------------------

using UnityEngine;
using System.Collections;

public class CharacterAnimation : MonoBehaviour {

	private Animator animator;
	public Transform upperSpine;
	public Transform headCamera;
	public Quaternion CameraRotation;
	private CharacterSystem character;
	
	// *************************
	// For legacy animation to rotation upper part along with camera.
	
	void Start () {
		animator = this.GetComponent<Animator>();
		character = this.GetComponent<CharacterSystem>();
	}
	

	void Update () {
		if(animator == null || character == null)
			return;
		
		// this is for legacy animation
		// if you using Mecanim in unity Pro, 
		// you can use animator.SetLookAtPosition (target position) instead.
		
		if(upperSpine){
			
			if(character.IsMine){
				// get rotation from Upper Spin
				CameraRotation = upperSpine.localRotation;
				CameraRotation.eulerAngles = new Vector3(upperSpine.localRotation.eulerAngles.x,upperSpine.localRotation.eulerAngles.y,-headCamera.transform.rotation.eulerAngles.x);
				
				// update rotation to other
				if(Network.isServer || Network.isClient){
					GetComponent<NetworkView>().RPC ("UpdateHeadRotation", RPCMode.Others, CameraRotation);
				}
			}
			
			// rotation Upper spin along with camera angle
			upperSpine.transform.localRotation = CameraRotation;
			// update animation transform
			if(animator.GetComponent<Animation>() && animator.GetComponent<Animation>()[animator.GetComponent<Animation>().clip.name])
				animator.GetComponent<Animation>()[animator.GetComponent<Animation>().clip.name].AddMixingTransform(upperSpine);

		}
		
	}
	[RPC]
	void UpdateHeadRotation(Quaternion rotation){
		CameraRotation = rotation;
	}

}
