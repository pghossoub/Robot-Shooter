using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Projectile {

	protected override void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Enemy")){
			other.GetComponent<Enemy>().LosePv(damage);
			other.GetComponent<Enemy>().bounceOnImpact(tr);
		}

		base.OnTriggerEnter2D(other);
	}
}
