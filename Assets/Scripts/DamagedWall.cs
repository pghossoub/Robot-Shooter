using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedWall : MonoBehaviour {

	public Sprite[] dmgSprites;
	public int hp = 3;
	public GameObject destroyImpact;

	private AudioSource damageSound;
	private SpriteRenderer spriteRenderer;

	void Start () 
	{
		spriteRenderer = GetComponent<SpriteRenderer> ();
		damageSound = GetComponent<AudioSource> ();
	}

	public void DamageWall(int loss)
	{
		damageSound.Play ();
		hp -= loss;

		if (hp == 2) {
			spriteRenderer.sprite = dmgSprites [0];
		} else if (hp == 1) {
			spriteRenderer.sprite = dmgSprites [1];
		}
		else if (hp == 0) {
			Instantiate (destroyImpact, transform.position, transform.rotation);
			Destroy (gameObject);
		}
	}
}
