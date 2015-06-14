using UnityEngine;
using System.Collections;

public class SpaceCapitalCombat : MonoBehaviour {
	public LazerCannon[] mainCannonsRight;
	public LazerCannon[] mainCannonsLeft;
	public GameObject targetAreaPrefab;
	public SpaceCamera spaceCamera;
	public Transform rightTargetPoint, leftTargetPoint;

	GameObject targetArea;
	float minDelay = .1f;
	float maxDelay = .3f;
	float cannonDelay;
	float prevCannonTime = 0f;
	int currentCannon = 0;
	ArrayList cannonsFired = new ArrayList();
	bool firingCannons = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		prevCannonTime += Time.deltaTime;

		if (spaceCamera.isLookingRight()){
			//ShowTargetingArea (rightTargetPoint);

			if (Input.GetButton("Fire1")){
				firingCannons = true;
			}

			if (firingCannons) {
				FireCannons (mainCannonsRight);
			}
		} else if (spaceCamera.isLookingLeft()){
			//ShowTargetingArea (leftTargetPoint);

			if (Input.GetButton("Fire1")){
				firingCannons = true;
			}
			
			if (firingCannons) {
				FireCannons (mainCannonsLeft);
			}
		} else {
			if (Input.GetButton("Fire1")){
				firingCannons = true;
			}

			if (firingCannons) {
				FireCannons (mainCannonsRight);
				FireCannons (mainCannonsLeft);
			}
			//Destroy(targetArea);
		}


	}

	void FireCannons (LazerCannon[] cannons)
	{
		cannonDelay = Random.Range (minDelay, maxDelay);
		if (prevCannonTime > cannonDelay) {
			do {
				currentCannon = Random.Range (0, cannons.Length);
			}
			while (cannonsFired.Contains (currentCannon));
			cannonsFired.Add (currentCannon);
			cannons[currentCannon].Fire ();
			prevCannonTime = 0f;
		}
		if (cannonsFired.Count == cannons.Length) {
			firingCannons = false;
			cannonsFired.Clear ();
			prevCannonTime = 0f;
		}
	}

	void ShowTargetingArea (Transform targetPoint)
	{
		if (!targetArea) {
			targetArea = Instantiate<GameObject> (targetAreaPrefab);
		}
		targetArea.transform.position = targetPoint.position;
		targetArea.transform.rotation = targetPoint.rotation;
	}
}
