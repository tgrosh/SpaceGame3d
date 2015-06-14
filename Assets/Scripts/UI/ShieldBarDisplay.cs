using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ProgressBar;

public class ShieldBarDisplay : MonoBehaviour {

	public DamageReceiver barTarget;	
	public float updateInterval = .3f;	
	public float updateSpeed = 3f;

	Image barImage;

	float currentUpdateTime = 0f;
	float fillAmount = 1f;

	void Start(){
		barImage = this.GetComponent<Image>();
		barImage.fillAmount = fillAmount;
	}

	// Update is called once per frame
	void Update () {
		currentUpdateTime += Time.deltaTime;

		if (currentUpdateTime > updateInterval) {
//			fillAmount = barTarget.CurrentShield / barTarget.maxShield;
			currentUpdateTime = 0f;
		}

		barImage.fillAmount = Mathf.Lerp(barImage.fillAmount, fillAmount, updateSpeed * Time.deltaTime);
	}
}
