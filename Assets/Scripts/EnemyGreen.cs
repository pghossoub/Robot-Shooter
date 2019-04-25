using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGreen : RangedEnemy {

	public float shotWidth;

	protected override IEnumerator Shoot ()
	{
		while (true) {

			Vector3 direction = tr.rotation.eulerAngles;
			Instantiate (shot, shotSpawn.position, Quaternion.Euler(0f, 0f, direction.z + 0f));
			Instantiate (shot, shotSpawn.position, Quaternion.Euler(0f, 0f, direction.z + shotWidth));
			Instantiate (shot, shotSpawn.position, Quaternion.Euler(0f, 0f, direction.z - shotWidth));

			shotSound.Play ();
			yield return new WaitForSeconds (fireRate);
		}
	}
}
