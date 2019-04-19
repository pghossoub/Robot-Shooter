using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy {

	public float behaviorDuration;

	private int behavior = 0;
	private float behaviorTimer = 0.0f;
	private Vector3 randomMove;

	protected AudioSource shotSound;

	protected override void Start ()
	{
		base.Start ();

		shotSound = GetComponent<AudioSource> ();
	}

	protected override void Update () {
		
		base.Update ();

		//time thing
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
}