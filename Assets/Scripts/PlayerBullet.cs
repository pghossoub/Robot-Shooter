using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Projectile {

	protected override void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Enemy")){
			Instantiate(impact, tr.position, tr.rotation);
			other.GetComponent<Enemy>().LosePv(damage);
			other.GetComponent<Enemy>().bounceOnImpact(tr, Vector3.up);
		}

		base.OnTriggerEnter2D(other);
	}
}
