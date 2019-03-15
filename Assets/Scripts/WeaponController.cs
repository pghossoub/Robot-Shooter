using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

	public float fireRate; //seconds
	public GameObject shot;
	public Transform shotSpawn;

	//private because component attached to weapon
	//private AudioSource shotSound;
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
