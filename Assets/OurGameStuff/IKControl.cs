using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class IKControl : MonoBehaviour {

    protected Animator animator;

    public bool ikActive = false;
    public Transform rightHandObj = null;
    public Transform LeftHandObj = null;
    public GameObject righthand;
    public GameObject righthandaim;
    public Transform righthandposition;
    public Transform righthandpositionAim;
    public Transform lefhandpositonaim;
    public GameObject reloadpos;
    private Playeranimations aim;
    public Transform lookObj = null;
    private bool reload;

    void Start() {
        animator = GetComponent<Animator>();
        aim = this.gameObject.GetComponent<Playeranimations>();

    }
    //a callback for calculating IK
    void OnAnimatorIK() {
        if (animator) {

            //if the IK is active, set the position and rotation directly to the goal. 
            if (ikActive) {

                // Set the look target position, if one has been assigned
                //if (lookObj != null) {
                //    animator.SetLookAtWeight(1);
                //    animator.SetLookAtPosition(lookObj.position);
               // }

                // Set the right hand target position and rotation, if one has been assigned
                if (rightHandObj != null) {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                    if (!aim.Aim) {
                        rightHandObj.position = righthand.transform.position;
                        rightHandObj.rotation = righthand.transform.rotation;
                        animator.SetIKPosition(AvatarIKGoal.RightHand, righthandposition.transform.position);
                        animator.SetIKRotation(AvatarIKGoal.RightHand, righthandposition.transform.rotation);
                        animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandObj.transform.position);
                        animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandObj.transform.rotation);
                    } else if (aim.Aim) {
                         rightHandObj.position = righthandaim.transform.position;
                         rightHandObj.rotation = righthandaim.transform.rotation;
                        //animator.SetIKPosition(AvatarIKGoal.RightHand,  righthandaim.transform.position);
                        //animator.SetIKRotation(AvatarIKGoal.RightHand, righthandaim.transform.rotation);
                        animator.SetIKPosition(AvatarIKGoal.RightHand, righthandpositionAim.transform.position);
                        animator.SetIKRotation(AvatarIKGoal.RightHand, righthandpositionAim.transform.rotation);
                        animator.SetIKPosition(AvatarIKGoal.LeftHand, lefhandpositonaim.transform.position);
                        animator.SetIKRotation(AvatarIKGoal.LeftHand, lefhandpositonaim.transform.rotation);
                            animator.SetLookAtWeight(1);
                            animator.SetLookAtPosition(lookObj.position);
                    }
                    if (!aim.reloading) {
                        rightHandObj.position = reloadpos.transform.position;
                        rightHandObj.rotation = reloadpos.transform.rotation;
                        animator.SetIKPosition(AvatarIKGoal.RightHand, righthandposition.transform.position);
                        animator.SetIKRotation(AvatarIKGoal.RightHand, righthandposition.transform.rotation);
                        animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandObj.transform.position);
                        animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandObj.transform.rotation);
                    }


                }

            }
            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            else {
                //animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                //animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetLookAtWeight(0);
            }
        }
    }
}
