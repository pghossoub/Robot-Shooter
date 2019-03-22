using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Projectile {

	protected override void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Player")){
			//Instantiate(impact, tr.position, tr.rotation);
			other.GetComponent<PlayerController>().LosePv(damage, impact);
			//other.GetComponent<Player>().bounceOnImpact(tr);
		}

		base.OnTriggerEnter2D(other);
	}
}
