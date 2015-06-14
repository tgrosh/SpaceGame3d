using UnityEngine;
using System.Collections;

public class LazerFire : MonoBehaviour {

	public float speed = 300f;
	public AudioClip explosionClip;
	public GameObject explosionPrefab;
	public int damageAmount = 10;
	public float range = 4000f;

	Vector3 spawnPoint;
	
	// Use this for initialization
	void Start () {
		spawnPoint = transform.position;
	}

	// Update is called once per frame
	void FixedUpdate () {
		transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);

		if (Vector3.Distance(spawnPoint, transform.position) > range) {
			Destroy (gameObject);
		}
	}

	void OnCollisionEnter (Collision collision) {
		DamageReceiver target = collision.collider.GetComponent<DamageReceiver>();
		if (target != null) {
			dealDamage (target);
			showDamage (transform.position, target.damageDecal, collision.collider.transform);
		}

		explode (transform.position);
	}

	void OnTriggerEnter (Collider collider) {
		DamageReceiver target = collider.gameObject.GetComponent<DamageReceiver>();
		if (target != null) {
			dealDamage (target);
			showDamage (transform.position, target.damageDecal, collider.transform);
		}
		
		Shield shield = collider.gameObject.GetComponent<Shield>();
		if (shield == null) {
			explode(transform.position);
		} else {
			Destroy (gameObject);
		}
	}

	private void explode(Vector3 position){
		AudioSource.PlayClipAtPoint(explosionClip, position);
		
		Instantiate(explosionPrefab, position, Quaternion.identity);
		
		/*Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, explosionRadius);
		foreach (Collider collider in colliders){
			TakeDamage damage = collider.GetComponent<TakeDamage>();
			if (damage){
				float distance = Vector3.Distance(collider.transform.position, gameObject.transform.position);
				damage.inflict(totalDamage - ((distance/explosionRadius) * totalDamage));
			}
		}*/
		Destroy (gameObject);
	}

	private void dealDamage(DamageReceiver target) {

		if (target) {
			target.Damage(damageAmount);
		}
	}

	private void showDamage(Vector3 position, GameObject damageDecal, Transform parent) {
		if (damageDecal != null) {
			GameObject decal = (GameObject)Instantiate(damageDecal, position, Quaternion.identity);
			decal.transform.parent = parent;
		}
	}
}
