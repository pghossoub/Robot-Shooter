using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public float pv;
	public float fireRate; //seconds
	public GameObject shot;
	public Transform shotSpawn;
	public GameObject legs;
	public GameObject deathExplosion;

	//private because component attached to enemy
	private Animator animatorLegs;
	private AudioSource shotSound;
	private Transform tr;
	private Rigidbody2D rb;

	void Start () 
	{
		shotSound = GetComponent<AudioSource> ();
		tr = GetComponent<Transform> ();
		rb = GetComponent<Rigidbody2D> ();
	}

	public void LosePv (float damage, Transform bulletTransform)
	{
		pv = pv - damage;
		//Vector3 movement = new Vector3 (bulletTransform.rotation.eulerAngles.z, 
		//								bulletTransform.rotation.eulerAngles.z, 
		//								0);
		//Vector3 movement = bulletTransform.rotation * Vector3.forward;
		//Debug.Log ("movement:" + movement);
		//rb.AddForce(movement * 1);
		if (pv <= 0) {
			Instantiate (deathExplosion, tr.position, tr.rotation);
			Destroy (gameObject);
		}
	}
}
