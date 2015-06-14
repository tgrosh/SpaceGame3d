using UnityEngine;
using System.Collections;

public class HyperspacePort : MonoBehaviour {

	LensFlare hyperspaceLensFlare;
	AudioSource portAudio;

	float targetIntensity = 1f;
	float portLightFadeInSpeed = 20f;
	float portLightFadeOutSpeed = 1f;
	float portLightSpeed;

	bool showLensFlare;

	void Start () {
		hyperspaceLensFlare = GetComponent<LensFlare>();
		portAudio = GetComponent<AudioSource>();
		portLightSpeed = portLightFadeInSpeed;
	}

	void LateUpdate () {
		if (showLensFlare) {
			hyperspaceLensFlare.brightness = Mathf.Lerp(hyperspaceLensFlare.brightness, targetIntensity, Time.deltaTime * portLightSpeed);				
			if (hyperspaceLensFlare.brightness > .9f) {
				targetIntensity = 0f;
				portLightSpeed = portLightFadeOutSpeed;
			}
			if (targetIntensity == 0f && hyperspaceLensFlare.brightness < .01) {
				gameObject.SetActive(false);
			}
		}
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.CompareTag("Player")){
			showLensFlare = true;
			portAudio.enabled = true;
		}
	}
}
