using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public float pv;
	public float timeGain;
	public float scorePoints;
	public float speed;
	public float fireRate; //seconds
	public float waitTime;
	public float behaviorDuration;
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

	private int behavior = 0;
	private float behaviorTimer = 0.0f;
	private Vector3 randomMove;

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
		//StartCoroutine(MoveTest());

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

		//time thing
		behaviorTimer += Time.deltaTime;
		StartCoroutine(Move());

	}

	IEnumerator Move ()
	{
		Vector3 movement;


		if (behavior == 0)
			movement = Vector3.up;
		else
			movement = randomMove;
		
		animatorLegs.SetTrigger ("Walk");
		rb.AddForce (tr.rotation * movement * speed);

		if (behaviorTimer > behaviorDuration) {
			behaviorTimer = 0.0f;
			if (behavior == 0) {
				behavior = 1;
				randomMove = RandomDirection ();
			}
			else
				behavior = 0;
		}

		yield return null;

	}

	Vector3 RandomDirection()
	{
		return new Vector3 (Random.Range (0f, 1f), Random.Range (0f, 1f), 0);
	}

	IEnumerator Attack ()
	{
		float deltaWaitTime = Random.Range (0f, 0.75f);
		yield return new WaitForSeconds (waitTime + deltaWaitTime);

		while(true)
		{
			Instantiate (shot, shotSpawn.position, tr.rotation);
			//Debug.Log ("rotation = " + tr.rotation);
			shotSound.Play ();
			yield return new WaitForSeconds (fireRate);
		}
	}

	public void LosePv (float damage)
	{
		pv = pv - damage;

		if (pv <= 0) {
			Instantiate (deathExplosion, tr.position, tr.rotation);
			gameManager.RemoveEnemy (timeGain, scorePoints);

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
	