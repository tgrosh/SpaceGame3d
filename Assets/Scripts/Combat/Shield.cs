using UnityEngine;
using System.Collections;

public class Shield : DamageReceiver {

	// Use this for initialization
	void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	void Update () {
		if (currentHealth == 0) {
			Destroy (gameObject);
		}

		base.Update();
	}
}
