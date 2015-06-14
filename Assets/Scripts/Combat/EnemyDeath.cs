using UnityEngine;
using System.Collections;

public class EnemyDeath : DeathAnimation {

	public Detonator enemyDeathExplosion;
	public AudioClip[] deathSounds;
	public ThirdPersonOrbitCam cam;

	public override void Die ()
	{
		Instantiate(enemyDeathExplosion, transform.position, Quaternion.identity);
		cam.GetComponent<PerlinShake>().PlayShake();
		foreach (AudioClip audio in deathSounds) {
			AudioSource.PlayClipAtPoint(audio, transform.position);
		}
		Destroy (gameObject);
	}
}
