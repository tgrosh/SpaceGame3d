using UnityEngine;
using System.Collections;

public class SpacePhysicsMovement : MonoBehaviour {
	public AudioSource thrustSound;
	public Rigidbody body;
	public ParticleSystem[] rearExhaustPortsFX;
	public ParticleSystem[] frontExhaustPortsFX;
	public float acceleration = 10f;
	public float rollSpeed = .2f;
	public float pitchSpeed = .2f;
	public float yawSpeed = .2f;
	public Engine[] forwardEngines;

	float forwardThrust = 0f;
	float reverseThrust = 0f;
	float rollThrust, pitchThrust;
	float yaw = 0f;

	bool manualForce = false;
	bool enginesOn = false;
	bool silent = false;
	bool keyboard = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (manualForce) return;

		silent = false;
		if (!keyboard) {
			forwardThrust = (1f + Input.GetAxis("Right Trigger"))/2f; //0..1
			reverseThrust = (1f + Input.GetAxis("Left Trigger"))/2f; //0..1
			rollThrust = Input.GetAxis ("Horizontal");
			pitchThrust = Input.GetAxis ("Vertical");
			yaw = Input.GetButton("Left Bumper") ? -1f: 0f;
			yaw += Input.GetButton("Right Bumper") ? 1f: 0f;
		} else {
			forwardThrust = Input.GetAxis("Vertical") > 0f ? Input.GetAxis("Vertical") : 0f;
			reverseThrust = Input.GetAxis("Vertical") < 0f ? -Input.GetAxis("Vertical") : 0f;
			rollThrust = Input.GetAxis ("Strafe");
			pitchThrust = Input.GetAxis ("UpDown");
			yaw = Input.GetAxis("Horizontal");
		}

		if (forwardThrust > 0f) {
			foreach (Engine engine in forwardEngines) {
				forwardEngines[0].Accelerate(forwardThrust);
			}
		}
		//body.AddForce(transform.forward * ((forwardThrust - reverseThrust) * acceleration));
		body.AddTorque(transform.forward * -rollThrust * rollSpeed);
		body.AddTorque(transform.right * pitchThrust * pitchSpeed);
		body.AddTorque(transform.up * yaw * yawSpeed);

		if (reverseThrust > -1f) {
			foreach(ParticleSystem exhaustFX in frontExhaustPortsFX){
				exhaustFX.startSpeed = 7f + (1f + reverseThrust * 20f);
				if (!exhaustFX.isPlaying && !silent){
					exhaustFX.Play();
				}
			}
		} else {
			foreach(ParticleSystem exhaustFX in frontExhaustPortsFX){
				if (!exhaustFX.isStopped && !silent){
					exhaustFX.Stop();
				}
			}
		}
	}

	public float getCurrentSpeed() {
		return body.velocity.magnitude;
	}

	public float getCurrentThrust() {
		return forwardThrust * 100f;
	}

	
	public void Accelerate (float thrust, ForceMode forceMode) {
		body.AddForce (transform.forward * ((thrust) * acceleration), forceMode);

		if (!enginesOn && !silent){
			thrustSound.Play();
			enginesOn = true;
		}
		
		if (forwardThrust > 0) {
			foreach(ParticleSystem exhaustFX in rearExhaustPortsFX){
				exhaustFX.startSpeed = 25f + (((1f + forwardThrust)/2f) * 125f);
				if (!exhaustFX.isPlaying && !silent){
					exhaustFX.Play();
				}
			}
		} else {
			foreach(ParticleSystem exhaustFX in rearExhaustPortsFX){
				if (!exhaustFX.isStopped && !silent){
					exhaustFX.Stop();
				}
			}
		}
	}
	public void Accelerate (float thrust)
	{
		manualForce = true;
		Accelerate(thrust, ForceMode.Force);
	}

	public void Push (float force)
	{
		silent = true;
		Accelerate(force, ForceMode.Impulse);
	}
	
	public void Roll (float rollThrust)
	{
		body.AddTorque (transform.forward * rollThrust);
	}
	
	public void Yaw (float yaw)
	{
		body.AddTorque (transform.up * yaw * yawSpeed);
	}
	
	public void Stop() {
		body.velocity = Vector3.Lerp (body.velocity, new Vector3(0f,0f,0f), Time.deltaTime);
		enginesOn = false;
		manualForce = false;
		thrustSound.Stop();
	}

	public void Halt() {
		body.velocity = new Vector3(0f,0f,0f);
		enginesOn = false;		
		manualForce = false;
		thrustSound.Stop();
	}

	public void Hyperjump(float thrust){
		silent = true;
		manualForce = true;
		Accelerate(thrust, ForceMode.Force);
	}
}
