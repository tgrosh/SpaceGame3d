using UnityEngine;
using System.Collections;

public class CapitalTurret : MonoBehaviour {
	
	public LazerCannon[] turretCannons;	
	public float cannonDelay = .2f;

	float prevCannonTime = 0f;
	int currentCannon = 0;
	float lookDampSpeed = 3f;

	// Use this for initialization
	void Start () {
		prevCannonTime = cannonDelay;
	}
	
	// Update is called once per frame
	void Update () {
		prevCannonTime += Time.deltaTime;	}

	public void Aim (Vector3 target) {
		SmoothLook(transform, target);

		//transform.LookAt(target);
		foreach (LazerCannon turretCannon in turretCannons) {
			//turretCannon.transform.LookAt(target);
			SmoothLook(turretCannon.transform, target);
		}
	}

	public void Fire ()
	{
		if (prevCannonTime > cannonDelay) {
			turretCannons[currentCannon].Fire ();
			prevCannonTime = 0f;
			currentCannon++;
			if (currentCannon > turretCannons.Length - 1){
				currentCannon = 0;
			}
		}
	}

	void SmoothLook(Transform transform, Vector3 target) {
		//transform.LookAt(target);
		Vector3 pos = target - transform.position;
		Quaternion newRot = Quaternion.LookRotation(pos);
		transform.rotation = Quaternion.Lerp(transform.rotation, newRot, lookDampSpeed * Time.deltaTime);
	}
}
