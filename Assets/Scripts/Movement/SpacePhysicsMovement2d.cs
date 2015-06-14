using UnityEngine;
using System.Collections;

public class SpacePhysicsMovement2d : MonoBehaviour {
	public bool acceptUserInput = true;
	public AudioSource thrustSound;
	public Rigidbody body;
	public ParticleSystem[] rearExhaustPortsFX;
	public ParticleSystem[] frontExhaustPortsFX;
	public ParticleSystem[] leftRollExhaustPortsFX;
	public ParticleSystem[] rightRollExhaustPortsFX;
	public float acceleration = 10f;
	public float rollSpeed = .2f;
	public float yawSpeed = .2f;

	float forwardThrust = 0f;
	float reverseThrust = 0f;
	float rollThrust;
	float yaw = 0f;

	bool enginesOn = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		forwardThrust = 0f;
		yaw = 0f;
		rollThrust = 0f;

		if (acceptUserInput) {
			forwardThrust = Input.GetAxis("Right Trigger");
			reverseThrust = Input.GetAxis("Left Trigger");
			yaw = Input.GetAxis ("Horizontal");
			//rollThrust = Input.GetButton("Left Bumper") ? -1000f * rollSpeed: 0f;
			//rollThrust += Input.GetButton("Right Bumper") ? 1000f * rollSpeed: 0f;
		}
		
		Accelerate (forwardThrust - reverseThrust);
		Roll (-rollThrust);
		Yaw (yaw * 1000f);
	}

	public void Accelerate (float thrust, ForceMode forceMode) {
		body.AddForce (transform.forward * ((thrust) * acceleration), forceMode);
		
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
				exhaustFX.startSpeed = 25f + (((1f + forwardThrust)/2f) * 125f);
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
	}
	public void Accelerate (float thrust)
	{
		Accelerate(thrust, ForceMode.Force);
	}

	public void Roll (float rollThrust)
	{
		body.AddTorque (transform.forward * rollThrust);

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

	public void Yaw (float yaw)
	{
		body.AddTorque (transform.up * yaw * yawSpeed);
	}

	public void Stop() {
		body.velocity = Vector3.Lerp (body.velocity, new Vector3(0f,0f,0f), Time.deltaTime);
	}

	public float getCurrentSpeed() {
		return body.velocity.magnitude;
	}

	public float getCurrentThrust() {
		return ((1f + forwardThrust)/2f) * 100f;
	}
}
