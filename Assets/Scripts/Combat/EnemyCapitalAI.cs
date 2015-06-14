using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyCapitalAI : MonoBehaviour
{
	public float firingRange = 4000f;

	List<Vector3> patrolWaypoints = new List<Vector3>();
	EnemySpacePhysicsMovement2d mover;
	SpaceCapitalEnemyCombat combat;
	Transform target;
	bool intercepting;
	float distanceToTarget;

	bool patrolling = false;

	// Use this for initialization
	void Start ()
	{
		mover = GetComponent<EnemySpacePhysicsMovement2d>();
		combat = GetComponent<SpaceCapitalEnemyCombat>();

		GameObject waypoints = GameObject.Find("Enemy Patrol Waypoints");
		for (int x=0; x < waypoints.transform.childCount; x++)
		{
			Transform child = waypoints.transform.GetChild(x);
			patrolWaypoints.Add (child.position);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (target && intercepting){
			distanceToTarget = Vector3.Distance (transform.position, target.position);
			if (combat != null && distanceToTarget <= firingRange) {
				combat.FireCannons(target);
			}
		} else if (!patrolling) {
			patrolling = true;
			mover.patrol(patrolWaypoints.ToArray());
		}
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject != gameObject && collider.gameObject.tag == "Player"){
			intercepting = true;
			patrolling = false;
			target = collider.transform;

			if (mover) {
				mover.intercept(target, firingRange);
			}
		}
	}
}

