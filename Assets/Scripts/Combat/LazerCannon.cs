using UnityEngine;
using System.Collections;

public class LazerCannon : MonoBehaviour {
	
	public float cooldown = .3f;
	public AudioSource[] projectileSounds = new AudioSource[0];
	public GameObject projectile;
	public ParticleSystem blastParticles;
		
	float cooldownRemaining = 0f;
	int currentAudio = -1;
	bool firing = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		cooldownRemaining -= Time.deltaTime;

		if (firing){
			if (currentAudio == -1 || !projectileSounds[currentAudio].isPlaying){
				currentAudio++;
				projectileSounds[currentAudio].Play();
			}

			if (currentAudio == projectileSounds.Length-1){
				GameObject lazer = (GameObject)Instantiate(projectile, transform.position + (transform.forward * 200f), transform.rotation);
				lazer.transform.position = transform.position + (transform.forward * lazer.gameObject.GetComponent<Collider>().bounds.size.z);
				lazer.transform.rotation = transform.rotation;

				cooldownRemaining = cooldown;
				firing = false;
				currentAudio = -1;

				blastParticles.Play();
			}
		}
	}

	public bool isFiring() {
		return firing;
	}

	public void Fire() {
		if (cooldownRemaining <= 0){					
			firing = true;
		}
	}

	public void Aim(Transform target) {
		transform.LookAt(target);
	}
}
