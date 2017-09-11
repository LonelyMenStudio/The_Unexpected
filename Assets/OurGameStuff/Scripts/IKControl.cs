using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent(typeof(Animator))]

public class IKControl : NetworkBehaviour {

    protected Animator animator;

    public bool ikActive = false;
    public Transform rightHandObj = null;
    public Transform rightShotty = null;
    public Transform rightSniper = null;
    public Transform LeftHandObj = null;
    public GameObject righthand;
    public GameObject righthandaim;
    public Transform righthandposition;
    public Transform righthandpositionAim;
    public Transform lefhandpositonaim;
    public GameObject reloadpos;
    private Playeranimations aim;
    public Transform lookObj = null;
    private weaponManager weapon;
    private bool reload;

    void Start() {
        animator = GetComponent<Animator>();
        aim = this.gameObject.GetComponent<Playeranimations>();
        weapon = GetComponent<weaponManager>();
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
                            HandStuff(1, weapon.weaponOut);
                        } else if (aim.Aim) {
                            HandStuff(2, weapon.weaponOut);                        }
                        if (!aim.reloading) {
                            HandStuff(3, weapon.weaponOut);
                        
                    }





                    if (!aim.Aim) {
                        animator.SetIKPosition(AvatarIKGoal.RightHand, righthandposition.transform.position);
                        animator.SetIKRotation(AvatarIKGoal.RightHand, righthandposition.transform.rotation);
                        animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandObj.transform.position);
                        animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandObj.transform.rotation);
                    } else if (aim.Aim) {

                        //animator.SetIKPosition(AvatarIKGoal.RightHand,  righthandaim.transform.position);
                        //animator.SetIKRotation(AvatarIKGoal.RightHand, righthandaim.transform.rotation);
                        animator.SetIKPosition(AvatarIKGoal.RightHand, righthandpositionAim.transform.position);
                        animator.SetIKRotation(AvatarIKGoal.RightHand, righthandpositionAim.transform.rotation);
                        animator.SetIKPosition(AvatarIKGoal.LeftHand, lefhandpositonaim.transform.position);
                        animator.SetIKRotation(AvatarIKGoal.LeftHand, lefhandpositonaim.transform.rotation);

                    }
                    if (!aim.reloading) {

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
    void HandStuff(int num, int wep) {
        if (!isServer) {
            CmdSwitchWeapon(num, wep);
        } else {
            RpcSwitchWeapon(num, wep);
        }
    }

    [Command]
    void CmdSwitchWeapon(int handlocation, int outwep) {
        RpcSwitchWeapon(handlocation, outwep);
    }
    [ClientRpc]
    void RpcSwitchWeapon(int handlocation, int outwep) {
        if (outwep == 1) {
            if (handlocation == 1) {
                rightHandObj.position = righthand.transform.position;
                rightHandObj.rotation = righthand.transform.rotation;
            } else if (handlocation == 2) {
                rightHandObj.position = righthandaim.transform.position;
                rightHandObj.rotation = righthandaim.transform.rotation;
                animator.SetLookAtWeight(1);
                animator.SetLookAtPosition(lookObj.position);
            } else if (handlocation == 3) {
                rightHandObj.position = reloadpos.transform.position;
                rightHandObj.rotation = reloadpos.transform.rotation;
            }
        }
        else if (outwep == 2) {
            if (handlocation == 1) {
                rightShotty.position = righthand.transform.position;
                rightShotty.rotation = righthand.transform.rotation;
            } else if (handlocation == 2) {
                rightShotty.position = righthandaim.transform.position;
                rightShotty.rotation = righthandaim.transform.rotation;
                animator.SetLookAtWeight(1);
                animator.SetLookAtPosition(lookObj.position);
            } else if (handlocation == 3) {
                rightShotty.position = reloadpos.transform.position;
                rightShotty.rotation = reloadpos.transform.rotation;
            }
        } else if (outwep == 3) {
                if (handlocation == 1) {
                rightSniper.position = righthand.transform.position;
                rightSniper.rotation = righthand.transform.rotation;
            } else if (handlocation == 2) {
                rightSniper.position = righthandaim.transform.position;
                rightSniper.rotation = righthandaim.transform.rotation;
                animator.SetLookAtWeight(1);
                animator.SetLookAtPosition(lookObj.position);
            } else if (handlocation == 3) {
                rightSniper.position = reloadpos.transform.position;
                rightSniper.rotation = reloadpos.transform.rotation;

            }
        }
        
    }
}
