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
	public Engine[] reverseEngines;

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
				engine.Accelerate(forwardThrust);
			}
		} else {
			foreach (Engine engine in forwardEngines) {
				engine.Off();
			}
		}
		if (reverseThrust > 0f) {
			foreach (Engine engine in reverseEngines) {
				engine.Accelerate(reverseThrust);
			}
		} else {
			foreach (Engine engine in reverseEngines) {
				engine.Off();
			}
		}

		body.AddTorque(transform.forward * -rollThrust * rollSpeed);
		body.AddTorque(transform.right * pitchThrust * pitchSpeed);
		body.AddTorque(transform.up * yaw * yawSpeed);
	}

	public float getCurrentSpeed() {
		return body.velocity.magnitude;
	}

	public float getCurrentThrust() {
		return forwardThrust * 100f;
	}

	
	public void Accelerate (float thrust, ForceMode forceMode) {
		foreach (Engine engine in forwardEngines) {
			engine.Accelerate(thrust, forceMode);
		}
	}
	public void Accelerate (float thrust)
	{
		manualForce = true;
		foreach (Engine engine in forwardEngines) {
			engine.Accelerate(thrust);
		}
	}

	public void Push (float force)
	{
		foreach (Engine engine in forwardEngines) {
			engine.Push(force);
		}
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
		manualForce = false;
		foreach (Engine engine in forwardEngines) {
			engine.Stop();
		}
	}

	public void Halt() {
		manualForce = false;
		foreach (Engine engine in forwardEngines) {
			engine.Halt();
		}
	}

	public void Hyperjump(float thrust){
		manualForce = true;
		foreach (Engine engine in forwardEngines) {
			engine.Hyperjump(thrust);
		}
	}
}
