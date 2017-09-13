﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//[ExecuteInEditMode]

public class InverseKinematics : NetworkBehaviour {

	public Transform leftUpperArm;
	public Transform leftForearm;
	public Transform leftHand;
	public Transform leftElbow;
	public Transform leftTarget;
	[Space(20)]
	public Vector3 leftUpperArm_OffsetRotation;
	public Vector3 leftForearm_OffsetRotation;
	public Vector3 leftHand_OffsetRotation;
	[Space(20)]
	public bool lefHandMatchesTargetRotation = true;
	[Space(20)]
	public bool leftDebug;


	float leftAngle;
	float leftUpperArm_Length;
	float leftForearm_Length;
	float leftArm_Length;
	float leftTargetDistance;
	float leftAdyacent;

    [Space(50)]

    public Transform upperArm;
    public Transform forearm;
    public Transform hand;
    public Transform elbow;
    public Transform target;
    [Space(20)]
    public Vector3 uppperArm_OffsetRotation;
    public Vector3 forearm_OffsetRotation;
    public Vector3 hand_OffsetRotation;
    [Space(20)]
    public bool handMatchesTargetRotation = true;
    [Space(20)]
    public bool debug;

    float angle;
    float upperArm_Length;
    float forearm_Length;
    float arm_Length;
    float targetDistance;
    float adyacent;

    [SyncVar]
    public bool runIK = false;


    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (!runIK) {
            return;
        }
        LeftAction();
        RightAction();
	}



    void LeftAction() {
        if (leftUpperArm != null && leftForearm != null && leftHand != null && leftElbow != null && leftTarget != null) {
            leftUpperArm.LookAt(leftTarget, leftElbow.position - leftUpperArm.position);
            leftUpperArm.Rotate(leftUpperArm_OffsetRotation);

            Vector3 leftCross = Vector3.Cross(leftElbow.position - leftUpperArm.position, leftForearm.position - leftUpperArm.position);



            leftUpperArm_Length = Vector3.Distance(leftUpperArm.position, leftForearm.position);
            leftForearm_Length = Vector3.Distance(leftForearm.position, leftHand.position);
            leftArm_Length = leftUpperArm_Length + leftForearm_Length;
            leftTargetDistance = Vector3.Distance(leftUpperArm.position, leftTarget.position);
            leftTargetDistance = Mathf.Min(leftTargetDistance, leftArm_Length - leftArm_Length * 0.001f);

            leftAdyacent = ((leftUpperArm_Length * leftUpperArm_Length) - (leftForearm_Length * leftForearm_Length) + (leftTargetDistance * leftTargetDistance)) / (2 * leftTargetDistance);

            leftAngle = Mathf.Acos(leftAdyacent / leftUpperArm_Length) * Mathf.Rad2Deg;

            leftUpperArm.RotateAround(leftUpperArm.position, leftCross, -leftAngle);

            leftForearm.LookAt(leftTarget, leftCross);
            leftForearm.Rotate(leftForearm_OffsetRotation);

            if (lefHandMatchesTargetRotation) {
                leftHand.rotation = leftTarget.rotation;
                leftHand.Rotate(leftHand_OffsetRotation);
            }

            if (leftDebug) {
                if (leftForearm != null && leftElbow != null) {
                    Debug.DrawLine(leftForearm.position, leftElbow.position, Color.blue);
                }

                if (leftUpperArm != null && leftTarget != null) {
                    Debug.DrawLine(leftUpperArm.position, leftTarget.position, Color.red);
                }
            }

        }
    }

    void RightAction() {
        if (upperArm != null && forearm != null && hand != null && elbow != null && target != null) {
            upperArm.LookAt(target, elbow.position - upperArm.position);
            upperArm.Rotate(uppperArm_OffsetRotation);

            Vector3 cross = Vector3.Cross(elbow.position - upperArm.position, forearm.position - upperArm.position);



            upperArm_Length = Vector3.Distance(upperArm.position, forearm.position);
            forearm_Length = Vector3.Distance(forearm.position, hand.position);
            arm_Length = upperArm_Length + forearm_Length;
            targetDistance = Vector3.Distance(upperArm.position, target.position);
            targetDistance = Mathf.Min(targetDistance, arm_Length - arm_Length * 0.001f);

            adyacent = ((upperArm_Length * upperArm_Length) - (forearm_Length * forearm_Length) + (targetDistance * targetDistance)) / (2 * targetDistance);

            angle = Mathf.Acos(adyacent / upperArm_Length) * Mathf.Rad2Deg;

            upperArm.RotateAround(upperArm.position, cross, -angle);

            forearm.LookAt(target, cross);
            forearm.Rotate(forearm_OffsetRotation);

            if (handMatchesTargetRotation) {
                hand.rotation = target.rotation;
                hand.Rotate(hand_OffsetRotation);
            }

            if (debug) {
                if (forearm != null && elbow != null) {
                    Debug.DrawLine(forearm.position, elbow.position, Color.blue);
                }

                if (upperArm != null && target != null) {
                    Debug.DrawLine(upperArm.position, target.position, Color.red);
                }
            }

        }

    }

    void LeftOnDrawGizmos(){
		if (leftDebug) {
			if(leftUpperArm != null && leftElbow != null && leftHand != null && leftTarget != null && leftElbow != null){
				Gizmos.color = Color.gray;
				Gizmos.DrawLine (leftUpperArm.position, leftForearm.position);
				Gizmos.DrawLine (leftForearm.position, leftHand.position);
				Gizmos.color = Color.red;
				Gizmos.DrawLine (leftUpperArm.position, leftTarget.position);
				Gizmos.color = Color.blue;
				Gizmos.DrawLine (leftForearm.position, leftElbow.position);
			}
		}
	}

    void OnDrawGizmos() {
        if (debug) {
            if (upperArm != null && elbow != null && hand != null && target != null && elbow != null) {
                Gizmos.color = Color.gray;
                Gizmos.DrawLine(upperArm.position, forearm.position);
                Gizmos.DrawLine(forearm.position, hand.position);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(upperArm.position, target.position);
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(forearm.position, elbow.position);
            }
        }
    }

}
