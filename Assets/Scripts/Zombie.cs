using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy {
	
	public float damage;
	public float dashRate;
	public float zombieDashTime;
	public GameObject impact;

	private bool zombieDashing = false;
	private AudioSource zombieSound;

	protected override void Start ()
	{
		base.Start ();

		zombieSound = GetComponent<AudioSource> ();

		StartCoroutine (ZombieDash ());
	}

	protected override void Update () {

		if (!zombieDashing)
			base.Update();
		
		if(!isBouncing)
			StartCoroutine(Move());
	}

	IEnumerator Move ()
	{
		animatorLegs.SetTrigger ("Walk");
		rb.AddForce (tr.rotation * Vector3.up * speed);
		yield return null;
	}

	IEnumerator ZombieDash()
	{
		float deltaWaitTime = Random.Range (0f, 0.75f);
		yield return new WaitForSeconds (waitTime + deltaWaitTime);

		while (true) {
			zombieSound.Play();
			speed = speed * 3;
			zombieDashing = true;
			yield return new WaitForSeconds (zombieDashTime);
			speed = speed / 3;
			zombieDashing = false;
			yield return new WaitForSeconds (dashRate);
		}
	}
}
