//----------------------------------------------
//      UnitZ : FPS Sandbox Starter Kit
//    Copyright © Hardworker studio 2015 
// by Rachan Neamprasert www.hardworkerstudio.com
//----------------------------------------------

/// <summary>
/// AI character controller.
/// Just A basic AI Character controller 
/// will looking for a Target and moving to and Attacking
/// </summary>
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterSystem))]

public class AICharacterController : MonoBehaviour
{
	public string[] TargetTag = {"Player"};
	public GameObject ObjectTarget;
	[HideInInspector]
	public Vector3 PositionTarget;
	[HideInInspector]
	public CharacterSystem character;
	[HideInInspector]
	public float DistanceAttack = 2;
	public float DistanceMoveTo = 20;
	public float TurnSpeed = 10.0f;
	public bool BrutalMode;
	public bool RushMode;
	public float PatrolRange = 10;
	[HideInInspector]
	public Vector3 positionTemp;
	[HideInInspector]
	public int aiTime = 0;
	[HideInInspector]
	public int aiState = 0;
	private float attackTemp = 0;
	public float AttackDelay = 0.5f;
	public float LifeTime = 30;
	public float IdleSoundDelay = 10;
	private float lifeTimeTemp = 0;
	private float jumpTemp, jumpTime, soundTime, soundTimeDuration;
	public int JumpRate = 20;
	private AIManager AImange;
	
	void Start ()
	{
		character = gameObject.GetComponent<CharacterSystem> ();
		positionTemp = this.transform.position;
		aiState = 0;
		attackTemp = Time.time;
		lifeTimeTemp = Time.time;
		jumpTemp = Time.time;
		soundTime = Time.time;
		soundTimeDuration = Random.Range (0, IdleSoundDelay);
		character.ID = "";
	}
	
	public void Attack (Vector3 targetDirectiom)
	{
		if (Time.time > attackTemp + AttackDelay) {
			Vector3[] dirs = new Vector3[1];
			dirs [0] = targetDirectiom.normalized;
			character.DoDamage (this.transform.position + character.DamageOffset, dirs, character.Damage, character.DamageLength, character.Penetrate, "", character.Team);
			character.AttackAnimation ();
			attackTemp = Time.time;	
		}
	}
	
	void FrontObstacleChecker ()
	{
		// jump when hit something in front
		Vector3 fwd = this.transform.TransformDirection (Vector3.forward);
		if (Physics.Raycast (this.transform.position, fwd, 1)) {
			if (Time.time >= jumpTemp + jumpTime) {
				character.Motor.inputJump = true;
				jumpTime = Random.Range (0, JumpRate) * 0.1f;
				jumpTemp = Time.time;
			}else{
				character.Motor.inputJump = false;		
			}
		}
		
	}

	void Update ()
	{
		if (character == null)
			return;
		
		// random play an idle sound.
		if (Time.time > soundTime + soundTimeDuration) {
			character.PlayIdleSound ();	
			soundTimeDuration = Random.Range (0, IdleSoundDelay);
			soundTime = Time.time;
		}
		// ai live within a duration
		if (Time.time > lifeTimeTemp + LifeTime) {
			character.HP = 0;
			character.dieByLifeTime = true;
		}

		bool offlineMode = (!Network.isServer && !Network.isClient);
		
		if (Network.isServer || offlineMode) {
			// get attack distance from primary weapon.
			DistanceAttack = character.PrimaryWeaponDistance;	

			float distance = Vector3.Distance (PositionTarget, this.gameObject.transform.position);
			Vector3 targetDirectiom = (PositionTarget - this.transform.position);
			Quaternion targetRotation = this.transform.rotation;
			float str = TurnSpeed * Time.time;
			// rotation to look at a target
			if (targetDirectiom != Vector3.zero) {
				targetRotation = Quaternion.LookRotation (targetDirectiom);
				targetRotation.x = 0;
				targetRotation.z = 0;
				transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, str);
			}
			
			FrontObstacleChecker ();
			
			// if Target is exist
			if (ObjectTarget != null) {
				lifeTimeTemp = Time.time;
				PositionTarget = ObjectTarget.transform.position;
				if (aiTime <= 0) {
					aiState = Random.Range (0, 4);
					aiTime = Random.Range (10, 100);
				} else {
					aiTime--;
				}
				
				// attack only distance.
				if (distance <= DistanceAttack) {
					if (aiState == 0 || BrutalMode) {
						Attack (targetDirectiom);
					}
				} else {
					if (distance <= DistanceMoveTo) {
						// rotation facing to a target.
						transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, str);
					} else {
						// if target is out of distance
						ObjectTarget = null;
						if (aiState == 0) {
							aiState = 1;
							aiTime = Random.Range (10, 500);
							PositionTarget = positionTemp + new Vector3 (Random.Range (-PatrolRange, PatrolRange), 0, Random.Range (-PatrolRange, PatrolRange));
						}
					}
				}

			} else {
	
				float length = float.MaxValue;

				for (int t=0; t< TargetTag.Length; t++) {
					// Finding all the targets by Tags.
					GameObject[] targets = (GameObject[])GameObject.FindGameObjectsWithTag (TargetTag [t]);
					if (targets != null && targets.Length > 0) {
						for (int i=0; i<targets.Length; i++) {
							float distancetargets = Vector3.Distance (targets [i].gameObject.transform.position, this.gameObject.transform.position);
							if ((distancetargets <= length && (distancetargets <= DistanceMoveTo || distancetargets <= DistanceAttack || RushMode)) && ObjectTarget != targets [i].gameObject) {
								length = distancetargets;
								ObjectTarget = targets [i].gameObject;
							}
						}
					}
				}
				if (aiState == 0) {
					// AI state == 0 mean AI is free, so moving to anywhere
					aiState = 1;
					aiTime = Random.Range (10, 200);
					PositionTarget = positionTemp + new Vector3 (Random.Range (-PatrolRange, PatrolRange), 0, Random.Range (-PatrolRange, PatrolRange));
				}
				if (aiTime <= 0) {
					// random AI state
					aiState = Random.Range (0, 4);
					aiTime = Random.Range (10, 200);
				} else {
					aiTime--;
				}
			
			
			}
			if (!offlineMode) {
				character.networkViewer.RPC ("MoveToPosition", RPCMode.All, PositionTarget);
			} else {
				character.MoveToPosition (PositionTarget);
			}
		}
	}
}
