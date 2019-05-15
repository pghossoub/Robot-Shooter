using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Projectile {

	protected override void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Player")){
			other.GetComponent<PlayerController>().LosePv(damage, impact);
		}

		base.OnTriggerEnter2D(other);
	}
}
