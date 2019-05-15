using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

	public float fireRate;
	public GameObject shot;
	public Transform shotSpawn;

	private float nextFire;
	private AudioSource shotSound;

	void Start () 
	{
		nextFire = 0f;
		shotSound = GetComponent<AudioSource> ();
	}
	
	void Update () 
	{
		if (Input.GetButton ("Fire1") && Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
			shotSound.Play ();
		}
	}
}
