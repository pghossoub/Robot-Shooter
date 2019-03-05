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
	}

	void FixedUpdate()
	{
		//Vector3 direction = tr.position - trPlayer.position;
		//Debug.Log ("direction: "+ direction);
		//tr.rotation = Quaternion.LookRotation (direction, Vector3.forward);
		//Debug.Log(tr.position - trPlayer.position);

		//Quaternion enemyRotation = Quaternion.LookRotation (tr.position - trPlayer.position, Vector3.forward);
		//tr.rotation = enemyRotation;

		//tr.eulerAngles = new Vector3 (0, 0, tr.eulerAngles.z);
		//rb.angularVelocity = 0;

		//Quaternion PlayerRotation = Quaternion.LookRotation (tr.position - mousePosition, Vector3.forward);
		//tr.rotation = PlayerRotation;

		Vector3 direction = (trPlayer.position - tr.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.forward);
		tr.rotation = Quaternion.Slerp(tr.rotation, lookRotation, Time.deltaTime * 100);
		tr.eulerAngles = new Vector3 (0, 0, tr.eulerAngles.z);
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
	