using UnityEngine;
using System.Collections;

public class Engine : MonoBehaviour {

	public AudioSource thrustSound;
	public Rigidbody body;
	public float acceleration = 5000f;
	public ParticleSystem exhaustFX;

	bool enginesOn = false;
	bool silent = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (enginesOn) {
			if (!exhaustFX.isPlaying && !silent){
				exhaustFX.Play();
			}
			if (!thrustSound.isPlaying && !silent) {
				thrustSound.Play();
			}
		} else {
			exhaustFX.Stop();
			thrustSound.Stop();
		}

		enginesOn = false; //assume not on
	}

	public void Accelerate (float thrust, ForceMode forceMode) {
		enginesOn = true;
		body.AddForce (body.transform.forward * ((thrust) * acceleration), forceMode);
	}

	public void Accelerate (float thrust)
	{
		Accelerate(thrust, ForceMode.Force);
	}

	public void Push (float force)
	{
		silent = true;
		Accelerate(force, ForceMode.Impulse);
	}

	public void Stop() {
		enginesOn = false;
		body.velocity = Vector3.Lerp (body.velocity, new Vector3(0f,0f,0f), Time.deltaTime);
	}
	
	public void Halt() {
		enginesOn = false;
		body.velocity = new Vector3(0f,0f,0f);
	}
	
	public void Hyperjump(float thrust){
		silent = true;
		Accelerate(thrust, ForceMode.Force);
	}
}
