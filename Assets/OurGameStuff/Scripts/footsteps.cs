using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footsteps : MonoBehaviour {

	public AudioClip[] rock;
	public AudioClip[] floor;
	public AudioClip[] grass;
	private bool isStep = true;
	public float audioStepLengthWalk = 0.45f;
	public float audioStepLengthRun = 0.25f;

	IEnumerator OnControllerColliderHit (ControllerColliderHit hit) {
		CharacterController controller = GetComponent<CharacterController>();
		if (controller.isGrounded && controller.velocity.magnitude < 7 && controller.velocity.magnitude > 5 && hit.gameObject.tag == "Floor" && isStep == true || controller.isGrounded && controller.velocity.magnitude < 7 && controller.velocity.magnitude > 5 && hit.gameObject.tag == "Untagged" && isStep == true) {
			WalkOnFloor ();
			yield return new WaitForSeconds (audioStepLengthWalk);
			isStep = true;
		} else if (controller.isGrounded && controller.velocity.magnitude > 8 && hit.gameObject.tag == "Floor" && isStep == true || controller.isGrounded && controller.velocity.magnitude > 8 && hit.gameObject.tag == "Untagged" && isStep == true) {
			RunOnFloor ();
			yield return new WaitForSeconds (audioStepLengthRun);
			isStep = true;
		} else if (controller.isGrounded && controller.velocity.magnitude < 7 && controller.velocity.magnitude > 5 && hit.gameObject.tag == "Rock" && isStep == true) {
			WalkOnRock ();
			yield return new WaitForSeconds (audioStepLengthWalk);
			isStep = true;
		} else if (controller.isGrounded && controller.velocity.magnitude > 8 && hit.gameObject.tag == "Rock" && isStep == true) {
			RunOnRock ();
			yield return new WaitForSeconds (audioStepLengthRun);
			isStep = true;
		} else if (controller.isGrounded && controller.velocity.magnitude < 7 && controller.velocity.magnitude > 5 && hit.gameObject.tag == "Grass" && isStep == true) {
			WalkOnGrass ();
			yield return new WaitForSeconds (audioStepLengthWalk);
			isStep = true;
		} else if (controller.isGrounded && controller.velocity.magnitude > 8 && hit.gameObject.tag == "Grass" && isStep == true) {
			RunOnGrass ();
			yield return new WaitForSeconds (audioStepLengthRun);
			isStep = true;
		} 
	}

	void WalkOnFloor() {
		if (isStep) {
			isStep = false;
			GetComponent<AudioSource> ().clip = floor [Random.Range (0, floor.Length)];
			GetComponent<AudioSource> ().volume = .5f;
			GetComponent<AudioSource> ().Play ();
		}
	}	
	void RunOnFloor() {
		isStep = false;
		GetComponent<AudioSource> ().clip = floor[Random.Range(0, floor.Length)];
		GetComponent<AudioSource> ().volume = .7f;
		GetComponent<AudioSource> ().Play ();
	}
	void WalkOnRock() {
		isStep = false;
		GetComponent<AudioSource> ().clip = rock[Random.Range(0, rock.Length)];
		GetComponent<AudioSource> ().volume = .5f;
		GetComponent<AudioSource> ().Play ();
	}	
	void RunOnRock() {
		isStep = false;
		GetComponent<AudioSource> ().clip = rock[Random.Range(0, rock.Length)];
		GetComponent<AudioSource> ().volume = .7f;
		GetComponent<AudioSource> ().Play ();
	}
	void WalkOnGrass() {
		isStep = false;
		GetComponent<AudioSource> ().clip = grass[Random.Range(0, grass.Length)];
		GetComponent<AudioSource> ().volume = .5f;
		GetComponent<AudioSource> ().Play ();
	}	
	void RunOnGrass() {
		isStep = false;
		GetComponent<AudioSource> ().clip = grass[Random.Range(0, grass.Length)];
		GetComponent<AudioSource> ().volume = .7f;
		GetComponent<AudioSource> ().Play ();
	}
}