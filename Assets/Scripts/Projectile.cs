using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour {

	public float damage;
	public GameObject impact;

	protected Transform tr;

	protected virtual void Start()
	{
		tr = GetComponent<Transform> ();
	}

	protected virtual void OnTriggerEnter2D(Collider2D other)
	{
		Instantiate(impact, tr.position, tr.rotation);
		Destroy(gameObject);
	}
}
