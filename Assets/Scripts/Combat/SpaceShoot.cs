using UnityEngine;
using System.Collections;

//rename to SpaceFighterCombat
public class SpaceShoot : MonoBehaviour {
	public GameObject[] cannons;
	public float cannonDelay = .5f;

	int currentCannon = 0;
	float prevCannonTime = 0f;

	// Use this for initialization
	void Start () {		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetButton("Fire1") && prevCannonTime > cannonDelay){
			cannons[currentCannon].GetComponent<LazerCannon>().Fire();
			currentCannon++;

			if (currentCannon >= cannons.Length) {
				currentCannon = 0;
			}

			prevCannonTime = 0;
		}

		prevCannonTime += Time.deltaTime;
	}
}
