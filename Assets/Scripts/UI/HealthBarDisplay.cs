using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ProgressBar;

public class HealthBarDisplay : MonoBehaviour {

	public DamageReceiver barTarget;	
	public float updateInterval = .3f;	
	public float updateSpeed = 3f;

	Image barImage;
	float yellowThreshold = .5f;
	float redThreshold = .25f;

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
			fillAmount = barTarget.CurrentHealth / barTarget.maxHealth;
			currentUpdateTime = 0f;
		}

		barImage.fillAmount = Mathf.Lerp(barImage.fillAmount, fillAmount, updateSpeed * Time.deltaTime);

		if (barImage.fillAmount < redThreshold){
			barImage.color = new Color(255f/255f, 68f/255f, 13f/255f);
		} else if (barImage.fillAmount < yellowThreshold) {
			barImage.color = new Color(255f/255f, 248f/255f, 13f/255f);
		} else {
			barImage.color = new Color(13f/255f, 1f, 55f/255f);
		}
	}
}
