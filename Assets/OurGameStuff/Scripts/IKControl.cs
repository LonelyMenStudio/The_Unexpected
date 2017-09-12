using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]

public class IKControl : NetworkBehaviour {

    public Animator animator;

    public bool ikActive = false;
    private GameObject Variables;
    private GameObject Manager;
    private PlayerManager pManager;
    private VariablesScript ManagerGet;
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
    private List<GameObject> Playerz;
    private PlayerAssignGet player;
    public int playerno;
    Vector3 Notaimpos;
    Vector3 aimpos;
    Vector3 repos;
    Quaternion Notaimrot;
    Quaternion aimrot;
    Quaternion reloadrot;
    public int Hands;

    void Start() {
        Variables = GameObject.FindWithTag("Start");
        ManagerGet = Variables.GetComponent<VariablesScript>();
        Manager = ManagerGet.variables;
        pManager = Manager.GetComponent<PlayerManager>();
        Playerz = pManager.Players;
        animator = GetComponent<Animator>();
        aim = this.gameObject.GetComponent<Playeranimations>();
        player = this.gameObject.GetComponent<PlayerAssignGet>();
        playerno = player.currentPlayerNo;
        weapon = GetComponent<weaponManager>();
    }
    void update() {

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
                    Notaimpos = righthand.transform.position;
                    Notaimrot = righthand.transform.rotation;
                    aimpos = righthandaim.transform.position;
                    aimrot = righthandaim.transform.rotation;
                    repos = reloadpos.transform.position;
                    reloadrot = reloadpos.transform.rotation;
                    if (!aim.Aim) {
                            HandStuff( weapon.weaponOut, Notaimpos, Notaimrot);
                        } else if (aim.Aim) {
                            HandStuff( weapon.weaponOut, aimpos, aimrot);                        }
                        if (!aim.reloading) {
                            HandStuff( weapon.weaponOut, repos,reloadrot) ;
                        
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
       /* if (isLocalPlayer) {
            foreach (GameObject EPlayer in Playerz) {

                PlayerAssignGet PlayerNum = EPlayer.GetComponent<PlayerAssignGet>();
                if (playerno != PlayerNum.currentPlayerNo) {
                   IKControl temp = EPlayer.GetComponent<IKControl>();
                    if (temp.Hands == 1) {
                        temp.animator.SetIKPosition(AvatarIKGoal.RightHand, temp.righthandposition.transform.position);
                        temp.animator.SetIKRotation(AvatarIKGoal.RightHand, temp.righthandposition.transform.rotation);
                        temp.animator.SetIKPosition(AvatarIKGoal.LeftHand, temp.LeftHandObj.transform.position);
                        temp.animator.SetIKRotation(AvatarIKGoal.LeftHand, temp.LeftHandObj.transform.rotation);
                    }
                }

            }
        }*/
    }
    void HandStuff( int wep, Vector3 psi, Quaternion rot) {
        if (!isServer) {
            CmdSwitchWeapon( wep, psi, rot);
        } else {
            RpcSwitchWeapon( wep, psi, rot);
        }
    }

    [Command]
    void CmdSwitchWeapon( int outwep, Vector3 pos, Quaternion rotation) {
        RpcSwitchWeapon( outwep, pos, rotation);
    }
    [ClientRpc]
    void RpcSwitchWeapon( int outwep, Vector3 pos, Quaternion rotation) {
        if (outwep == 1) {
            rightHandObj.position = pos;
            rightHandObj.rotation = rotation;
            Hands = 1;
        } else if (outwep == 2) {
            rightShotty.position = pos;
            rightShotty.rotation = rotation;
            Hands = 2;

        } else if (outwep == 3) {
                rightSniper.position = pos;
                rightSniper.rotation = rotation;
            Hands = 3;
            
        }
        
    }
}
