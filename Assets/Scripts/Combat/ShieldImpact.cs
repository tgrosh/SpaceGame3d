using UnityEngine;
using System.Collections;

public class ShieldImpact : MonoBehaviour {

	public float startScale = 1f;
	public float maxScale = 10f;
	public float duration = 1f;
	float fadeRate = .5f;
	Renderer rend;

	float scaleChange;
	float fadeChange;
	float startFade;

	// Use this for initialization
	void Start () {
		transform.localScale = Vector3.one * startScale;
		rend = GetComponent<Renderer>();
		startFade = rend.material.GetColor("_TintColor").a;
	}
	
	// Update is called once per frame
	void Update () {
		scaleChange = ((maxScale - startScale)/duration) * Time.deltaTime;
		transform.localScale += Vector3.one * scaleChange;

		fadeChange = startFade/duration * Time.deltaTime;
		Color c = rend.material.GetColor("_TintColor");
		c.a -= fadeChange;
		rend.material.SetColor("_TintColor", c);

		if (transform.localScale.x >= maxScale) {
			Destroy (transform.parent.gameObject);
		}
	}
}
