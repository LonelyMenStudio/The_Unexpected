using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

    public static Manager control;
    public Button Pistol;
    public Button Medic;
    public Button Armour;
    public Toggle Toggle12;
    public Toggle Toggle1;
    public Toggle Toggle2;
    public Toggle Toggle3;
    public Toggle Toggle4;
    //public Toggle Toggle5;
    public int weaponSpawn = 0;  //number used to pick the weapon for the spot
    public int weaponSpawn1 = 0;
    public int weaponSpawn2 = 0;
    public int weaponSpawn3 = 0;
    public int weaponSpawn4 = 0;
    public int weaponSpawn5 = 0;
    private bool pistol = false;
    private bool medic = false;
    private bool armour = false;
    /*
    // Use this for initialization
    void Awake() {
        
        if (control == null) {
            DontDestroyOnLoad(gameObject);
            control = this;
        } else if (control != this) {
            Destroy(gameObject);
        }
    }
    */
    // Update is called once per frame

    void Update() {
        weaponSpawn = ItemSpawner(Toggle12, weaponSpawn);
        weaponSpawn1 = ItemSpawner(Toggle1, weaponSpawn1);
        weaponSpawn2 = ItemSpawner(Toggle2, weaponSpawn2);
        weaponSpawn3 = ItemSpawner(Toggle3, weaponSpawn3);
        weaponSpawn4 = ItemSpawner(Toggle4, weaponSpawn4);
        //weaponSpawn5 = ItemSpawner(Toggle5, weaponSpawn5);

    }
    public int ItemSpawner(Toggle Toggle, int weaponSpawnz) {

        if (Pistol.interactable == false) {

            pistol = true;
            medic = false;
            armour = false;
        }
        if (Medic.interactable == false) {
            pistol = false;
            medic = true;
            armour = false;
        }
        if (Armour.interactable == false) {
            pistol = false;
            medic = false;
            armour = true;
        }

        if ((pistol == true) && (Toggle.isOn == true) && (weaponSpawnz == 0)) {
            //Toggle.isOn = false;
            //Toggle.image.color = Color.red;
            weaponSpawnz = 1;
            


        } else if ((pistol == true) && (Toggle.isOn == true) && (weaponSpawnz != 0)) {
            //Toggle.isOn = false;
            //Toggle.image.color = Color.white;
            weaponSpawnz = 0;
        }
        if ((medic == true) && (Toggle.isOn == true) && (weaponSpawnz == 0)) {
            //Toggle.isOn = false;
            Toggle.image.color = Color.yellow;
            weaponSpawnz = 2;

        } else if ((medic == true) && (Toggle.isOn == true) && (weaponSpawnz != 0)) {
            //Toggle.isOn = false;
            Toggle.image.color = Color.white;
            weaponSpawnz = 0;
        }
        if ((armour == true) && (Toggle.isOn == true) && (weaponSpawnz == 0)) {
            Toggle.isOn = false;
            Toggle.image.color = Color.blue;
            weaponSpawnz = 3;

        } else if ((armour == true) && (Toggle.isOn == true) && (weaponSpawnz != 0)) {
            Toggle.isOn = false;
            Toggle.image.color = Color.white;
            weaponSpawnz = 0;
        }
        return weaponSpawnz;
    }
}

//Registar where the items will be spawning once the game starts.
/* public int ItemSpawner(Toggle Toggle, int weaponSpawnz) {
     if (Toggle.isOn == true && Pistol.interactable == false) {
         weaponSpawnz = 1;

     }
     if (Toggle.isOn == true && Medic.interactable == false) {
         weaponSpawnz = 2;

     }
     if (Toggle.isOn == true && Armour.interactable == false) {
         weaponSpawnz = 3;

     }else if (Toggle.isOn == false) {
         weaponSpawnz = 0;
     }

     return weaponSpawnz;
     }

void OnGUI() {



    GUI.Label(new Rect(10, 100, 150, 50), "weaponSpawn1: " + weaponSpawn1);
    GUI.Label(new Rect(10, 130, 180, 50), "weaponSpawn1: " + weaponSpawn2);
    GUI.Label(new Rect(10, 170, 210, 50), "weaponSpawn1: " + weaponSpawn3);
    GUI.Label(new Rect(10, 200, 240, 50), "weaponSpawn1: " + weaponSpawn4);
    GUI.Label(new Rect(10, 230, 270, 50), "weaponSpawn1: " + weaponSpawn5);
    GUI.Label(new Rect(10, 270, 300, 50), "weaponSpawn1: " + weaponSpawn);

    GUI.Button(new Rect(800, 200, 100, 100), "Pistol");


    //Button1 = GUI.Button(new Rect(900, 900, 100, 50), "Pubic");
    Toggle10 = GUI.Toggle(new Rect(200, 100, 300, 30), Toggle10, " ");
    //if (GUI.Button(new Rect(400, 200, 100, 100), "")) {

}

     Toggle11 = GUI.Toggle(new Rect(200, 200, 300, 30), Toggle11, " ");
      if (Toggle11 == true && Medic.interactable == true) {
          weaponSpawn2 = 3;
      } else if (Toggle10 == false) {
          weaponSpawn2 = 0;
      }


}*/
