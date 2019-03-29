using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public float pv;
	public float speed;
	public float fireRate; //seconds
	public float waitTime;
	public GameObject shot;
	public Transform shotSpawn;
	public GameObject legs;
	public GameObject deathExplosion;

	private Animator animatorLegs;
	private AudioSource shotSound;
	private Transform tr;
	private Rigidbody2D rb;
	private GameObject player;
	private Transform trPlayer;
	private GameManager gameManager;

	void Start () 
	{
		shotSound = GetComponent<AudioSource> ();
		tr = GetComponent<Transform> ();
		rb = GetComponent<Rigidbody2D> ();
		animatorLegs = legs.GetComponent<Animator> ();

		player = GameObject.FindWithTag("Player");
		trPlayer = player.GetComponent<Transform> ();

		gameManager = GameObject.FindWithTag("Game Manager").GetComponent<GameManager>();

		//Move
		StartCoroutine(Move());

		//Attack!
		StartCoroutine (Attack());
	}

	void Update()
	{
		//Aim Player

		if(trPlayer != null)
		{
			Vector3 playerPosition = Camera.main.WorldToScreenPoint (trPlayer.position);
			Vector3 enemyPosition = Camera.main.WorldToScreenPoint (tr.position);
			Vector3 direction = (playerPosition - enemyPosition);
			Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.back);
			lookRotation.x = 0;
			lookRotation.y = 0;
			tr.rotation = Quaternion.Slerp(tr.rotation, lookRotation, 1f);
			//tr.eulerAngles = new Vector3 (0, 0, tr.eulerAngles.z);
		}

	}

	IEnumerator Move ()
	{
		while (true) 
		{
			animatorLegs.SetTrigger ("Walk");
			rb.AddForce (tr.rotation * Vector3.up * speed);
			yield return null;
		}
	}

	IEnumerator Attack ()
	{
		yield return new WaitForSeconds (waitTime);

		while(true)
		{
			Instantiate (shot, shotSpawn.position, tr.rotation);
			shotSound.Play ();
			yield return new WaitForSeconds (fireRate);
		}
	}

	public void LosePv (float damage)
	{
		pv = pv - damage;

		if (pv <= 0) {
			Instantiate (deathExplosion, tr.position, tr.rotation);
			gameManager.RemoveEnemy ();

			if (gameManager.CheckNoEnemyLeft ()) {
				gameManager.OpenExit ();
			}

			Destroy (gameObject);
		}
	}

	public void bounceOnImpact(Transform bulletTransform)
	{
		Vector3 movement = bulletTransform.rotation * Vector3.up;
		rb.AddForce(movement * 100);
	}
}
	