using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {

	public float pv;
	public float timeGain;
	public float scorePoints;
	public float speed;
	public float waitTime;
	public float bounceTime;

	public GameObject legs;
	public GameObject deathExplosion;

	protected Animator animatorLegs;
	protected Transform tr;
	protected Rigidbody2D rb;
	protected bool isBouncing = false;

	private GameObject player;
	private Transform trPlayer;
	private GameManager gameManager;

	protected virtual void Start () 
	{
		tr = GetComponent<Transform> ();
		rb = GetComponent<Rigidbody2D> ();
		animatorLegs = legs.GetComponent<Animator> ();

		player = GameObject.FindWithTag("Player");
		trPlayer = player.GetComponent<Transform> ();

		gameManager = GameObject.FindWithTag("Game Manager").GetComponent<GameManager>();
	}

	protected virtual void Update()
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

	public void BounceOnImpact(Transform hit, Vector3 direction)
	{
		if(!isBouncing)
			StartCoroutine (Bounce(hit, direction));	
	}

	IEnumerator Bounce(Transform hit, Vector3 direction)
	{
		isBouncing = true;
		Vector3 movement = hit.rotation * direction;
		rb.AddForce (movement * 100);
		yield return new WaitForSeconds (bounceTime);
		isBouncing = false;
	}
}