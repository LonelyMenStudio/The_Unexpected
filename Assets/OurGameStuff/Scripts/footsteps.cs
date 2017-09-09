using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class footsteps : NetworkBehaviour
{

	public AudioClip[] rock;
	public AudioClip[] floor;
	public AudioClip[] grass;
	private bool isStep = true;
	public float audioStepLengthWalk = 0.45f;
	public float audioStepLengthRun = 0.25f;
	public AudioClip footstepCue;
	private float statusCue;
	private bool shouldPlay = true;

	IEnumerator OnControllerColliderHit (ControllerColliderHit hit)
	{
		CharacterController controller = GetComponent<CharacterController> ();
		shouldPlay = true;
		if (controller.isGrounded && controller.velocity.magnitude < 7 && controller.velocity.magnitude > 5 && hit.gameObject.tag == "Floor" && isStep == true || controller.isGrounded && controller.velocity.magnitude < 7 && controller.velocity.magnitude > 5 && hit.gameObject.tag == "Untagged" && isStep == true) {
			WalkOnFloor ();
			//yield return new WaitForSeconds (audioStepLengthWalk);
			//isStep = true;
		} else if (controller.isGrounded && controller.velocity.magnitude > 8 && hit.gameObject.tag == "Floor" && isStep == true || controller.isGrounded && controller.velocity.magnitude > 8 && hit.gameObject.tag == "Untagged" && isStep == true) {
			RunOnFloor ();

			//yield return new WaitForSeconds (audioStepLengthRun);
			//isStep = true;
		} else if (controller.isGrounded && controller.velocity.magnitude < 7 && controller.velocity.magnitude > 5 && hit.gameObject.tag == "Rock" && isStep == true) {
			WalkOnRock ();

			//yield return new WaitForSeconds (audioStepLengthWalk);
			//isStep = true;
		} else if (controller.isGrounded && controller.velocity.magnitude > 8 && hit.gameObject.tag == "Rock" && isStep == true) {
			RunOnRock ();

			//yield return new WaitForSeconds (audioStepLengthRun);
			//isStep = true;
		} else if (controller.isGrounded && controller.velocity.magnitude < 7 && controller.velocity.magnitude > 5 && hit.gameObject.tag == "Grass" && isStep == true) {
			WalkOnGrass ();

			//yield return new WaitForSeconds (audioStepLengthWalk);
			//isStep = true;
		} else if (controller.isGrounded && controller.velocity.magnitude > 8 && hit.gameObject.tag == "Grass" && isStep == true) {
			RunOnGrass ();
			//yield return new WaitForSeconds (audioStepLengthRun);
			//isStep = true;
		} else {
			shouldPlay = false;
		}
		if (shouldPlay) {
			GetComponent<AudioSource> ().clip = footstepCue;

			if (!isServer) {
				CmdPlayAudio();
			} else {
				RpcPlayAudio();
			}
			//GetComponent<AudioSource> ().Play ();
			yield return new WaitForSeconds (statusCue);
			isStep = true;
			}
	}

	[Command]
	void CmdPlayAudio() {
		RpcPlayAudio();
	}

	[ClientRpc]
	void RpcPlayAudio() {
		GetComponent<AudioSource> ().Play ();
	}

	void WalkOnFloor ()
	{
		isStep = false;
		footstepCue = floor [Random.Range (0, floor.Length)];
		GetComponent<AudioSource> ().volume = .7f;
		statusCue = audioStepLengthWalk;
	}

	void RunOnFloor ()
	{
		isStep = false;
		footstepCue = floor [Random.Range (0, floor.Length)];
		GetComponent<AudioSource> ().volume = 1.0f;
		statusCue = audioStepLengthRun;
	}

	void WalkOnRock ()
	{
		isStep = false;
		footstepCue = rock [Random.Range (0, rock.Length)];
		GetComponent<AudioSource> ().volume = .7f;
		statusCue = audioStepLengthWalk;
	}

	void RunOnRock ()
	{
		isStep = false;
		footstepCue = rock [Random.Range (0, rock.Length)];
		GetComponent<AudioSource> ().volume = 1.0f;
		statusCue = audioStepLengthRun;
	}

	void WalkOnGrass ()
	{
		isStep = false;
		footstepCue = grass [Random.Range (0, grass.Length)];
		GetComponent<AudioSource> ().volume = .7f;
		statusCue = audioStepLengthWalk;
	}

	void RunOnGrass ()
	{
		isStep = false;
		footstepCue = grass [Random.Range (0, grass.Length)];
		GetComponent<AudioSource> ().volume = 1.0f;
		statusCue = audioStepLengthRun;
	}
}