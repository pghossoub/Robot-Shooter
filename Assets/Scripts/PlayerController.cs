using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	public float pv;
	public float speed;

	public float blinkTime;
	public float blinkDuration;
	private float nbBlink;

	public GameObject mainCamera;
	public GameObject legs;
	public GameObject deathExplosion;
	public float restartLevelDelay = 1f;

	private Animator animatorLegs;
	private Rigidbody2D rb;
	private Transform tr;
	private SpriteRenderer[] sr;

	private bool isBlinking;

	void Start () 
	{
		rb = GetComponent<Rigidbody2D>();
		tr = GetComponent<Transform>();
		animatorLegs = legs.GetComponent<Animator> ();
		sr = GetComponentsInChildren<SpriteRenderer> ();

		nbBlink = blinkDuration / blinkTime;
		isBlinking = false;
	}

	void FixedUpdate()
	{
		if (Input.GetKey ("right") || Input.GetKey ("left") || Input.GetKey ("up") || Input.GetKey ("down"))
		{
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");

			Vector3 movement = new Vector3 (moveHorizontal, moveVertical, 0.0f);

			rb.AddForce(movement * speed);
			animatorLegs.SetTrigger ("Walk");
		}

		//Rotation to follow mouse
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Quaternion PlayerRotation = Quaternion.LookRotation (tr.position - mousePosition, Vector3.forward);
		tr.rotation = PlayerRotation;

		//negate 3D rotation //no need in orthographic ?
		transform.eulerAngles = new Vector3 (0, 0, transform.eulerAngles.z);
		rb.angularVelocity = 0;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Open Exit") {
			Debug.Log("Next Level!");
			enabled = false;
			Restart();
		}
	}

	IEnumerator blink()
	{
		isBlinking = true;
		int locNbBlink = (int)nbBlink;

		while (locNbBlink > 0f) {
			locNbBlink -= 1;

			//toggle renderer
			foreach (SpriteRenderer spriteRenderer in sr) {
				spriteRenderer.enabled = !spriteRenderer.enabled;
			}
			//wait for a bit
			yield return new WaitForSeconds(blinkTime);
		}

		//make sure renderer is enabled when we exit
		foreach (SpriteRenderer spriteRenderer in sr) {
			spriteRenderer.enabled = true;
		}
		isBlinking = false;
	}

	public void LosePv (float damage, GameObject impact)
	{
		if (isBlinking == false) {
			
			Instantiate(impact, tr.position, tr.rotation);
			pv = pv - damage;

			if (pv <= 0) {
				Instantiate (deathExplosion, tr.position, tr.rotation);
				Destroy (gameObject);
			} else {
				StartCoroutine (blink ());
			}
		}
	}

	private void Restart()
	{
		SceneManager.LoadScene (0);
	}
}
