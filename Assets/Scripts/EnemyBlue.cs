using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlue : Enemy {
	
	public float fireRate; //seconds
	public float behaviorDuration;
	public GameObject shot;
	public Transform shotSpawn;

	private AudioSource shotSound;
	private int behavior = 0;
	private float behaviorTimer = 0.0f;
	private Vector3 randomMove;

	protected override void Start ()
	{
		base.Start ();

		shotSound = GetComponent<AudioSource> ();

		StartCoroutine(Attack());
	}

	protected override void Update () 
	{
		base.Update ();

		//time thing
		behaviorTimer += Time.deltaTime;
		if(!isBouncing)
			StartCoroutine(Move());
	}

	IEnumerator Move ()
	{
		Vector3 movement;

		if (behavior == 0)
			movement = Vector3.up;
		else
			movement = randomMove;

		animatorLegs.SetTrigger ("Walk");
		rb.AddForce (tr.rotation * movement * speed);

		if (behaviorTimer > behaviorDuration) {
			behaviorTimer = 0.0f;
			if (behavior == 0) {
				behavior = 1;
				randomMove = RandomDirection ();
			}
			else
				behavior = 0;
		}

		yield return null;

	}

	IEnumerator Attack ()
	{
		float deltaWaitTime = Random.Range (0f, 0.75f);
		yield return new WaitForSeconds (waitTime + deltaWaitTime);

		while (true) {
			Instantiate (shot, shotSpawn.position, tr.rotation);
			shotSound.Play ();
			yield return new WaitForSeconds (fireRate);
		}
	}

	Vector3 RandomDirection()
	{
		return new Vector3 (Random.Range (0f, 1f), Random.Range (0f, 1f), 0);
	}
}
