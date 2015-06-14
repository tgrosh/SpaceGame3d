using UnityEngine;
using System.Collections;

public class SpaceCapitalEnemyCombat : MonoBehaviour
{
	public LazerCannon[] bowCannons;
	public float firingArc = 60f;

	float cannonDelay = .5f;
	float prevCannonTime = 0f;
	int currentCannon = 0;
	bool firingCannons = false;
	Transform target;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		prevCannonTime += Time.deltaTime;

		if (firingCannons) {
			float angle = Vector3.Angle(target.position - transform.position, transform.forward);

			if (angle < firingArc/2f && prevCannonTime > cannonDelay) {
				bowCannons[currentCannon].Aim (target);
				bowCannons[currentCannon].Fire ();
				prevCannonTime = 0f;
				currentCannon++;
			}
			if (currentCannon > bowCannons.Length - 1) {
				firingCannons = false;
				prevCannonTime = 0f;
				currentCannon = 0;
			}
		}
	}

	public void FireCannons(Transform target) {
		this.target = target;
		firingCannons = true;
	}
}

