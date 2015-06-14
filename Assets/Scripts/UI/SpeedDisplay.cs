using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeedDisplay : MonoBehaviour {

	public SpacePhysicsMovement movement;
	public float updateInterval = .3f;

	float currentUpdateTime = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		currentUpdateTime += Time.deltaTime;

		if (currentUpdateTime > updateInterval) {
			GetComponent<Text>().text = ((int)movement.getCurrentSpeed()).ToString();
			currentUpdateTime = 0f;
		}
	}
}
