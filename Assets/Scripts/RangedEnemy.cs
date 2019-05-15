using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RangedEnemy : Enemy {

	public float behaviorDuration;
	public float fireRate;
	public GameObject shot;
	public Transform shotSpawn;

	private int behavior = 0;
	private float behaviorTimer = 0.0f;
	private Vector3 randomMove;

	protected AudioSource shotSound;
	protected bool isDashing = false;

	protected override void Start ()
	{
		base.Start ();

		shotSound = GetComponent<AudioSource> ();

		StartCoroutine(Attack());
	}

	protected virtual void FixedUpdate () 
	{
		behaviorTimer += Time.deltaTime;
		if(!isBouncing && !isDashing)
			StartCoroutine(Move());
	}

	protected IEnumerator Move ()
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

	protected Vector3 RandomDirection()
	{
		float x = UnityEngine.Random.Range (0f, 1f);
		double y = Math.Sqrt (1 - (x*x));
		return new Vector3 (x, (float)y, 0);
	}

	protected IEnumerator Attack()
	{

		float deltaWaitTime = UnityEngine.Random.Range (0f, 0.75f);
		yield return new WaitForSeconds (waitTime + deltaWaitTime);

		StartCoroutine (Shoot ());
	}

	protected virtual IEnumerator Shoot()
	{
		while (true) {
			Instantiate (shot, shotSpawn.position, tr.rotation);
			shotSound.Play ();
			yield return new WaitForSeconds (fireRate);
		}
	}
}