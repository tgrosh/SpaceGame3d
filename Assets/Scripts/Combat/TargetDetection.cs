using UnityEngine;
using System.Collections;

public class TargetDetection : MonoBehaviour {

	public Material noTargetMaterial;
	public Material targetingMaterial;
	MeshRenderer meshRenderer;

	// Use this for initialization
	void Start () {
		meshRenderer = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.GetComponent<LazerFire>() == null){			
			meshRenderer.material = targetingMaterial;
		}
	}

	void OnTriggerExit (Collider other) {
		meshRenderer.material = noTargetMaterial;
	}

	void OnTriggerStay (Collider other) {
		if (other.gameObject.GetComponent<LazerFire>() == null){			
			meshRenderer.material = targetingMaterial;
		}
	}
}
