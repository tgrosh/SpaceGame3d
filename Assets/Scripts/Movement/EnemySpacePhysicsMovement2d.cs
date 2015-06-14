using UnityEngine;
using System.Collections;

public class EnemySpacePhysicsMovement2d : SpacePhysicsMovement2d {

	public Transform target = null;
	public float turnRate = .5f;

	bool intercepting = false;
	bool patrolling = false;
	public float interceptDistance = 1500f;
	float distanceToTarget;
	Vector3[] waypoints;
	int currentWaypoint = 0;
	float waypointThreshold = 500f;

	SpaceCapitalEnemyCombat combat;

	// Use this for initialization
	void Start () {
		combat = GetComponent<SpaceCapitalEnemyCombat>();
	}
	
	// Update is called once per frame
	void Update () {
		if (target && intercepting){
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), turnRate * Time.deltaTime);

			distanceToTarget = Vector3.Distance (transform.position, target.transform.position);
			if (distanceToTarget > interceptDistance) {
				Accelerate(1f);
			} else {
				Stop ();
			}
		}

		if (patrolling && waypoints.Length > 0) {
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(waypoints[currentWaypoint] - transform.position), turnRate * Time.deltaTime);

			distanceToTarget = Vector3.Distance (transform.position, waypoints[currentWaypoint]);
			if (distanceToTarget > waypointThreshold) {
				Accelerate(1f);
			} else {
				currentWaypoint ++;
				if (currentWaypoint > waypoints.Length - 1) {
					currentWaypoint = 0;
				}
			}
		}
	}

	public void intercept(Transform target, float range) {
		this.target = target;
		this.interceptDistance = range;
		this.intercepting = true;
		this.patrolling = false;
	}

	public void patrol(Vector3[] waypoints) {
		this.waypoints = waypoints;
		this.patrolling = true;
		this.intercepting = false;
	}
}
