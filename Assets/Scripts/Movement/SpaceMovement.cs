using UnityEngine;
using System.Collections;

public class SpaceMovement : MonoBehaviour {
	public AudioSource thrustSound;
	public CharacterController controller;
	public ParticleSystem[] rearExhaustPortsFX;
	public ParticleSystem[] frontExhaustPortsFX;
	public ParticleSystem[] leftRollExhaustPortsFX;
	public ParticleSystem[] rightRollExhaustPortsFX;
	public float topSpeed = 50.0f;
	public float acceleration = .1f;
	public float rotationSpeed = .2f;
	public float yawSpeed = .2f;

	float currentSpeed = 0.0f;
	float forwardThrust = 0f;
	float reverseThrust = 0f;
	float rollThrust, pitchThrust;
	float yaw = 0f;


	bool enginesOn = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		forwardThrust = Input.GetAxis("Right Trigger");
		reverseThrust = Input.GetAxis("Left Trigger");
		rollThrust = Input.GetAxis ("Horizontal");
		pitchThrust = Input.GetAxis ("Vertical");
		yaw = Input.GetButton("Left Bumper") ? -1f * yawSpeed: 0f;
		yaw += Input.GetButton("Right Bumper") ? 1f * yawSpeed: 0f;

		currentSpeed += ((forwardThrust - reverseThrust) * acceleration);
		if (currentSpeed > topSpeed) {
			currentSpeed = topSpeed;
		}
		controller.Move(transform.forward * currentSpeed * Time.deltaTime);

		transform.Rotate(pitchThrust * rotationSpeed, yaw, rollThrust * -rotationSpeed * 2, Space.Self);

		//Debug.Log ("forwardThrust: " + forwardThrust + " reverseThrust: " + reverseThrust + " rollThrust: " + rollThrust + " pitchThrust: " + pitchThrust);
		if (!enginesOn && (forwardThrust > -1f || reverseThrust > -1f)){
			//playsound
			thrustSound.Play();
			enginesOn = true;
		}
		if (enginesOn && (forwardThrust == -1f && reverseThrust == -1f)){
			//playsound
			thrustSound.Stop();
			enginesOn = false;
		}
		thrustSound.volume = (2f + forwardThrust + reverseThrust)/4f;

		if (forwardThrust > -1f) {
			foreach(ParticleSystem exhaustFX in rearExhaustPortsFX){
				exhaustFX.startSize = 1.5f + (((1f + forwardThrust)/2f) * 5f);
				exhaustFX.startSpeed = 20f + (((1f + forwardThrust)/2f) * 20f);
				if (!exhaustFX.isPlaying){
					exhaustFX.Play();
				}
			}
		} else {
			foreach(ParticleSystem exhaustFX in rearExhaustPortsFX){
				if (!exhaustFX.isStopped){
					exhaustFX.Stop();
				}
			}
		}

		if (reverseThrust > -1f) {
			foreach(ParticleSystem exhaustFX in frontExhaustPortsFX){
				exhaustFX.startSpeed = 7f + (((1f + reverseThrust)/2f) * 20f);
				if (!exhaustFX.isPlaying){
					exhaustFX.Play();
				}
			}
		} else {
			foreach(ParticleSystem exhaustFX in frontExhaustPortsFX){
				if (!exhaustFX.isStopped){
					exhaustFX.Stop();
				}
			}
		}

		if (rollThrust < 0f) {
			foreach(ParticleSystem exhaustFX in leftRollExhaustPortsFX){
				exhaustFX.startLifetime = .1f + (((1f + rollThrust * -1f)/2f) * .25f);
				if (!exhaustFX.isPlaying){
					exhaustFX.Play();
				}
			}
			foreach(ParticleSystem exhaustFX in rightRollExhaustPortsFX){
				if (!exhaustFX.isStopped){
					exhaustFX.Stop();
				}
			}
		} else if (rollThrust > 0f) {
			foreach(ParticleSystem exhaustFX in rightRollExhaustPortsFX){
				exhaustFX.startLifetime = .1f + (((1f + rollThrust)/2f) * .25f);
				if (!exhaustFX.isPlaying){
					exhaustFX.Play();
				}
			}
			foreach(ParticleSystem exhaustFX in leftRollExhaustPortsFX){
				if (!exhaustFX.isStopped){
					exhaustFX.Stop();
				}
			}
		} else {
			foreach(ParticleSystem exhaustFX in rightRollExhaustPortsFX){
				if (!exhaustFX.isStopped){
					exhaustFX.Stop();
				}
			}
			foreach(ParticleSystem exhaustFX in leftRollExhaustPortsFX){
				if (!exhaustFX.isStopped){
					exhaustFX.Stop();
				}
			}
		}
	}

}
