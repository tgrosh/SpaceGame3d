using UnityEngine;
using System.Collections;

public class SpaceCapitalMouseCombat : MonoBehaviour
{
	public CapitalTurret turret;	
	public LayerMask layerMask;

	Vector3 firingDirection = Vector3.forward;
	RaycastHit hit; 
	bool rayCastHit;

	void Update () { 
		// create a ray from the center of the screen
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0f));
		rayCastHit = Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);

		if (rayCastHit){ 
			// if something aimed, find the direction from spawnPoint to the target 
			firingDirection = hit.point;
		} else {
			// if there's nothing, use the ray direction 
			firingDirection = ray.GetPoint(100000f); 
		}

		turret.Aim (firingDirection);

		if (Input.GetButton("Fire1"))
		{ 
			if (hit.collider == null || !hit.collider.gameObject.CompareTag("Player")) {
				turret.Fire ();
			}
		} 
	}

	public bool isTargetingObject() {
		return rayCastHit;
	}

	public float distanceToTarget() {
		if (rayCastHit) {
			return hit.distance;
		} else {
			return Mathf.Infinity;
		}
	}
}

