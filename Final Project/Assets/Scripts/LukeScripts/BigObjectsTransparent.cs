using UnityEngine;
using System.Collections;

public class BigObjectsTransparent : MonoBehaviour {

	private SpriteRenderer spriteRenderer;
	private Color color;

	void Start () 
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.color = new Color (spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);

	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.tag == "Player" && other.isTrigger == false)
			StartCoroutine ("FadeIn");
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Player" && other.isTrigger == false)
			StartCoroutine ("FadeOut");
	}

	
	IEnumerator FadeIn()
	{
		for (float f = 1f; f >= 0.3f; f -= 0.1f) 
		{
			spriteRenderer.color = new Color (spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, f);
			yield return null;
		}
	}

	IEnumerator FadeOut()
	{
		for (float f = 0.3f; f <= 1.1f; f += 0.1f) 
		{
			spriteRenderer.color = new Color (spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, f);
			yield return null;
		}
	}
}