using UnityEngine;

public class Billboard : MonoBehaviour { 
	void Update() { 
		transform.LookAt(Camera.main.transform.position, -Camera.main.transform.up); 
		//transform.rotation = Camera.main.transform.rotation;
	} 
}