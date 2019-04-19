using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlue : RangedEnemy {
	
	public float fireRate; //seconds
	public GameObject shot;
	public Transform shotSpawn;

	protected override void Start ()
	{
		base.Start ();

		StartCoroutine(Attack());
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
}
