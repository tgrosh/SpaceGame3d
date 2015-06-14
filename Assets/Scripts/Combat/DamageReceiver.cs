using UnityEngine;
using System.Collections;

public class DamageReceiver : MonoBehaviour {

	public float maxHealth;
	public float healthRegenerationRate = 0f; //percent per second
	public GameObject damageDecal;

	DeathAnimation deathAnimation;

	protected float currentHealth;

	// Use this for initialization
	protected void Start () {
		currentHealth = maxHealth;
		deathAnimation = gameObject.GetComponent<DeathAnimation>();
	}

	protected void Update () {
		if (currentHealth == 0 && deathAnimation != null){
			deathAnimation.Die();
		}

		Heal(maxHealth * (healthRegenerationRate/100f) * Time.deltaTime);
	}

	public void Damage(float damageAmount) {
		currentHealth -= damageAmount;
		if (currentHealth < 0){
			currentHealth = 0;
		}
	}

	public void Heal(float healAmount){
		currentHealth += healAmount;
		if (currentHealth > maxHealth) {
			currentHealth = maxHealth;
		}
	}

	public float CurrentHealth {
		get {
			return currentHealth;
		}
	}
}
