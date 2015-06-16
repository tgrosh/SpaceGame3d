using UnityEngine;
using System.Collections;

public class Hyperdrive : MonoBehaviour {

	public Camera hyperdriveCamera;
	public GameObject hyperspaceEffect;
	public Transform[] hyperspacePorts;	
	public AudioSource hyperdriveSpinupSound;
	public AudioSource hyperdriveEngageSound;

	GameObject trail;
	TrailRenderer trailRend;
	SpacePhysicsMovement mover;
	Camera originalCamera;
	PerlinShake hyperdriveShaker;
	UnityStandardAssets.ImageEffects.MotionBlur hyperdriveBlur;
	bool inHyperspace;
	bool enteringHyperSpace;
	bool leavingHyperspace;
	Transform hyperSpacePort;
	float hyperSpaceAlignRate = 1f;
	float alignTime = 0f;
	bool atPort;
	Canvas guiCanvas;
	float targetIntensity = 1f;
	float portLightFadeInSpeed = 20f;
	float portLightFadeOutSpeed = 1f;
	float portLightSpeed;

	float hyperspaceEntranceTime;
	bool hyperspaceEntranceSet = false;

	float exitPushImpulse = 1.2f;

	// Use this for initialization
	void Start () {
		hyperdriveShaker = hyperdriveCamera.gameObject.GetComponent<PerlinShake>();
		hyperdriveBlur = hyperdriveCamera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.MotionBlur>();
		trail = transform.FindChild("Trail").gameObject;
		trailRend = trail.GetComponent<TrailRenderer>();
		mover = GetComponent<SpacePhysicsMovement>();
		guiCanvas = GameObject.Find ("GUI").GetComponent<Canvas>();
		portLightSpeed = portLightFadeInSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		if (!originalCamera) {
			originalCamera = Camera.main;
		}

		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			if (!inHyperspace) {
				hyperSpacePort = getHyperspacePort();
				if (hyperSpacePort != null) {
					enteringHyperSpace = true;
				}
			} else {
//				hyperspaceEffect.SetActive(false);
//				hyperdriveCamera.enabled = false;
//				originalCamera.enabled = true;
//				orbitCam.setInputActive(true);
				enteringHyperSpace = false;
				inHyperspace = false;
			}
		}

		if (enteringHyperSpace) {
			EnterHyperSpace(hyperSpacePort);
		}

		if (leavingHyperspace) {
			Color c = trailRend.material.GetColor("_TintColor");
			c.a = Mathf.Clamp((mover.getCurrentSpeed() - 400f) / 800f, 0f, .5f);
			trailRend.material.SetColor("_TintColor", c);
		}
	}

	Transform getHyperspacePort() {
		foreach(Transform port in hyperspacePorts) {
			RaycastHit hit;

			if (Physics.Raycast(transform.position, (port.transform.position - transform.position), out hit)) {
				if (hit.collider.CompareTag("Hyperspace Port")) {
					return hit.collider.transform;
				}
			}
		}

		return null;
	}

	public void Stop() {
		mover.Stop();
		trail.SetActive(false);
	}

	public void LeaveHyperspace(Transform entrance) {
		hyperspaceEntranceTime += Time.deltaTime;

		leavingHyperspace = true;

		if (!hyperspaceEntranceSet){
			transform.position = entrance.position;
			transform.rotation = entrance.rotation;
			trail.SetActive(true);
			hyperspaceEntranceSet = true;
		}

		mover.Push(exitPushImpulse);

		exitPushImpulse -= Time.deltaTime;
		if (exitPushImpulse < 0) {
			exitPushImpulse = 0;
		}
	}

	void EnterHyperSpace(Transform port) {
		if (alignTime > 1f) {
			//spin up hyperDrive
			hyperdriveSpinupSound.enabled = true;
		}
		if (alignTime > 5f && !atPort) {
			originalCamera.gameObject.SetActive(false);
			originalCamera.enabled = false;	
			guiCanvas.enabled = false;
			hyperdriveCamera.gameObject.SetActive(true);
			hyperdriveCamera.enabled = true;
			hyperdriveCamera.transform.parent = null;
			transform.rotation = Quaternion.LookRotation(port.position - transform.position);
			mover.Hyperjump(500f);
			trail.SetActive(true);

			if (Vector3.Distance(transform.position, port.transform.position) < 1500f){
				mover.Halt();
				atPort = true;
			}
		} 

		if (!atPort) {
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(port.position - transform.position), hyperSpaceAlignRate * Time.deltaTime);			
			alignTime += Time.deltaTime;
		} else {
			trail.SetActive(false);
		}
	}
}
