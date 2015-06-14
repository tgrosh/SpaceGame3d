using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ProgressBar;

public class ThrottleUIScript : MonoBehaviour {

	public SpacePhysicsMovement spaceMovement;
	public float updateInterval = .3f;
	
	float currentUpdateTime = 0f;

	// Update is called once per frame
	void Update () {
		currentUpdateTime += Time.deltaTime;
		
		if (currentUpdateTime > updateInterval) {
			GetComponent<ProgressRadialBehaviour>().Value = spaceMovement.getCurrentThrust();
			currentUpdateTime = 0f;
		}
	}
}
