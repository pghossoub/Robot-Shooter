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
	private GameObject player;
	private Transform trPlayer;

	void Start () 
	{
		shotSound = GetComponent<AudioSource> ();
		tr = GetComponent<Transform> ();
		rb = GetComponent<Rigidbody2D> ();

		player = GameObject.FindWithTag("Player");
		trPlayer = player.GetComponent<Transform> ();


		//Attack!
		StartCoroutine (Attack());
	}

	void Update()
	{
		//Aim Player

		Vector3 playerPosition = Camera.main.WorldToScreenPoint (trPlayer.position);
		Vector3 enemyPosition = Camera.main.WorldToScreenPoint (tr.position);
		Vector3 direction = (playerPosition - enemyPosition);
		Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.back);
		lookRotation.x = 0;
		lookRotation.y = 0;
		tr.rotation = Quaternion.Slerp(tr.rotation, lookRotation, 1f);
		//tr.eulerAngles = new Vector3 (0, 0, tr.eulerAngles.z);

	}

	IEnumerator Attack ()
	{
		while(true)
		{
			Instantiate (shot, shotSpawn.position, tr.rotation);
			yield return new WaitForSeconds (fireRate);
		}
	}

	public void LosePv (float damage)
	{
		pv = pv - damage;
		if (pv <= 0) {
			Instantiate (deathExplosion, tr.position, tr.rotation);
			Destroy (gameObject);
		}
	}

	public void bounceOnImpact(Transform bulletTransform)
	{
		Vector3 movement = bulletTransform.rotation * Vector3.up;
		rb.AddForce(movement * 100);
	}
}
	