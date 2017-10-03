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
    public GameObject Backwards;
    Vector3 Notaimpos;
    Vector3 aimpos;
    Vector3 repos;
    Vector3 backwards;
    Quaternion Notaimrot;
    Quaternion aimrot;
    Quaternion reloadrot;
    Quaternion backwardsrot;
    public int Hands;
    private InverseKinematics handsIK;


    public AvatarIKGoal rightHand = AvatarIKGoal.RightHand;
    public AvatarIKGoal leftHand = AvatarIKGoal.LeftHand;


    // Here
    private Vector3 Movetopos;
    private Vector3 MoveFrompos;
    private Vector3 Movetoposy;
    private Vector3 MoveFromposy;
    private Vector3 Movetoposz;
    private Vector3 MoveFromposz;
    //private float speedMove = 0.2f;
    private float lerpTime = 0.6f;
    public float currentlerp = 0;
    private bool movingTo = true;
    // private bool reset = false;

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
        handsIK = this.GetComponent<InverseKinematics>();

    }
    void update() {

    }
    //a callback for calculating IK
    void OnAnimatorIK() {

        if (animator) {
            //if the IK is active, set the position and rotation directly to the goal. 
            //handsIK.CmdSetRunning(ikActive);
            if (ikActive) {
                // Set the look target position, if one has been assigned
                //if (lookObj != null) {
                //    animator.SetLookAtWeight(1);
                //    animator.SetLookAtPosition(lookObj.position);
                // }
                // Set the right hand target position and rotation, if one has been assigned


                if (rightHandObj != null || rightShotty != null || rightSniper != null) {
                    animator.SetIKPositionWeight(rightHand, 1);
                    animator.SetIKRotationWeight(rightHand, 1);
                    animator.SetIKPositionWeight(leftHand, 1);
                    animator.SetIKRotationWeight(leftHand, 1);
                    Notaimpos = righthand.transform.position;
                    Notaimrot = righthand.transform.rotation;
                    aimpos = righthandaim.transform.position;
                    aimrot = righthandaim.transform.rotation;
                    repos = reloadpos.transform.position;
                    reloadrot = reloadpos.transform.rotation;
                    backwards = Backwards.transform.position;
                    backwardsrot = Backwards.transform.rotation;
                    // MoveFrompos = righthand.transform.position;
                    // Movetopos = righthandaim.transform.position;
                    if (aim.Change == true) {
                        movingTo = true;
                        currentlerp = 0;
                        aim.Change = false;
                    }
                    if (movingTo == true || !aim.reloading) {
                        currentlerp += Time.deltaTime;
                        if (currentlerp >= lerpTime) {
                            currentlerp = lerpTime;
                            movingTo = false;
                            // currentlerp = 0;
                        }
                    }
                    if (aim.outofaimrun && !movingTo) {
                        aim.outofaimrun = false;
                    }
                    float Perc = currentlerp / lerpTime;
                    if (aim.backactive && aim.reloading && !aim.Aim) {
                        HandStuff(weapon.weaponOut, backwards, backwardsrot, Perc);
                    }
                     else if (!aim.Aim && aim.reloading && !aim.backactive) {
                        // movingTo = true;

                        HandStuff(weapon.weaponOut, Notaimpos, Notaimrot, Perc);

                    } else if (aim.Aim && aim.reloading) {
                        HandStuff(weapon.weaponOut, aimpos, aimrot, Perc);
                    } else if (!aim.reloading) {
                        HandStuff(weapon.weaponOut, repos, reloadrot, Perc);

                    }



                    /*   if (weapon.weaponOut == 1 || weapon.weaponOut == 2 || weapon.weaponOut == 3) {

                           if (!aim.Aim) {
                               animator.SetIKPosition(rightHand, righthandposition.transform.position);
                               animator.SetIKRotation(rightHand, righthandposition.transform.rotation);
                               animator.SetIKPosition(leftHand, LeftHandObj.transform.position);
                               animator.SetIKRotation(leftHand, LeftHandObj.transform.rotation);
                           } else if (aim.Aim) {

                               //animator.SetIKPosition(AvatarIKGoal.RightHand,  righthandaim.transform.position);
                               //animator.SetIKRotation(AvatarIKGoal.RightHand, righthandaim.transform.rotation);
                               animator.SetIKPosition(rightHand, righthandpositionAim.transform.position);
                               animator.SetIKRotation(rightHand, righthandpositionAim.transform.rotation);
                               animator.SetIKPosition(leftHand, lefhandpositonaim.transform.position);
                               animator.SetIKRotation(leftHand, lefhandpositonaim.transform.rotation);

                           }
                           if (!aim.reloading) {

                               animator.SetIKPosition(rightHand, righthandposition.transform.position);
                               animator.SetIKRotation(rightHand, righthandposition.transform.rotation);
                               animator.SetIKPosition(leftHand, LeftHandObj.transform.position);
                               animator.SetIKRotation(leftHand, LeftHandObj.transform.rotation);
                           }
                       }*/

                }

            }


            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            else {
                //animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                //animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                // animator.SetIKPositionWeight(leftHand, 0);
                //   animator.SetIKRotationWeight(leftHand, 0);
                //   animator.SetLookAtWeight(0);
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


    void HandStuff(int wep, Vector3 psi, Quaternion rot, float perc) {
        SwitchWeapon(wep, psi, rot, perc);
        /*
        if (!isServer) {
            CmdSwitchWeapon(wep, psi, rot, perc);
        } else {
            RpcSwitchWeapon(wep, psi, rot, perc);
        }
        */
    }

    [Command]
    void CmdSwitchWeapon(int outwep, Vector3 pos, Quaternion rotation, float percent) {
        //RpcSwitchWeapon(outwep, pos, rotation, percent);
    }

    //[ClientRpc]
    //void RpcSwitchWeapon(int outwep, Vector3 pos, Quaternion rotation, float percent) {
    void SwitchWeapon(int outwep, Vector3 pos, Quaternion rotation, float percent) {
        if (outwep == 1) {
            rightHandObj.position = Vector3.Lerp(rightHandObj.position, pos, percent);
            rightHandObj.rotation = Quaternion.Lerp(rightHandObj.rotation, rotation, percent);
            Hands = 1;
        } else if (outwep == 2) {
            rightShotty.position = Vector3.Lerp(rightShotty.position, pos, percent); 
            rightShotty.rotation = Quaternion.Lerp(rightShotty.rotation, rotation, percent);
            Hands = 2;

        } else if (outwep == 3) {
            rightSniper.position = Vector3.Lerp(rightSniper.position, pos, percent); 
            rightSniper.rotation = Quaternion.Lerp(rightSniper.rotation, rotation, percent);
            Hands = 3;

        }

    }
}
