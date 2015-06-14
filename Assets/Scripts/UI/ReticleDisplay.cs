using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReticleDisplay : MonoBehaviour {

	public SpaceCapitalMouseCombat mouseCombat;

	Image reticleImage;
	Text reticleText;

	// Use this for initialization
	void Start () {
		reticleImage = gameObject.GetComponent<Image>();
		reticleText = gameObject.transform.GetChild(0).GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		if (mouseCombat.isTargetingObject()) {
			reticleImage.color = Color.red;
			reticleText.text = mouseCombat.distanceToTarget().ToString("f0") + " m";
		} else {
			reticleImage.color = Color.white;
			reticleText.text = "";
		}
	}
}
