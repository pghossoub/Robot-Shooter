using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy {

	public float behaviorDuration;
	public float fireRate;
	public GameObject shot;
	public Transform shotSpawn;

	private int behavior = 0;
	private float behaviorTimer = 0.0f;
	private Vector3 randomMove;

	protected AudioSource shotSound;

	protected override void Start ()
	{
		base.Start ();

		shotSound = GetComponent<AudioSource> ();

		StartCoroutine(Attack());
	}

	protected override void Update () {
		
		base.Update ();

		behaviorTimer += Time.deltaTime;
		if(!isBouncing)
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
		return new Vector3 (Random.Range (0f, 1f), Random.Range (0f, 1f), 0);
	}

	protected IEnumerator Attack()
	{

		float deltaWaitTime = Random.Range (0f, 0.75f);
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