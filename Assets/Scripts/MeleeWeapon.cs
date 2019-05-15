using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Melee Weapons are for enemies only
public class MeleeWeapon : MonoBehaviour {

	public float damage;
	public GameObject impact;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag ("Player")) 
			other.GetComponent<PlayerController> ().LosePv (damage, impact);
		
		if (other.CompareTag ("Damaged Wall")) {
			other.GetComponent<DamagedWall> ().DamageWall (1);
			GetComponentInParent<Enemy> ().BounceOnImpact (transform, Vector3.down);
		}
	}
}

