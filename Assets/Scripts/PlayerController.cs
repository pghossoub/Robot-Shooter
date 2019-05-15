using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public float pv;
	public float speed;
	public float blinkTime;
	public float blinkDuration;
	public float dashSpeed;
	public float dashTime;
	public float dashRecharge;
	public GameObject mainCamera;
	public GameObject legs;
	public GameObject dashChargeDisplay;
	public float restartLevelDelay = 1f;

	private Animator animatorLegs;
	private float nbBlink;
	private Rigidbody2D rb;
	private Transform tr;
	private SpriteRenderer[] sr;
	private GameObject heart;
	private AudioSource dashSound;
	private Text dashChargeDisplayText;
	private Color dashChargedColor;
	private bool isBlinking = false;
	private bool isDashingInCharge = false;
	private bool isDashing = false;

	void Start () 
	{
		rb = GetComponent<Rigidbody2D>();
		tr = GetComponent<Transform>();
		animatorLegs = legs.GetComponent<Animator> ();
		sr = GetComponentsInChildren<SpriteRenderer> ();
		dashSound = GetComponent<AudioSource> ();
		dashChargeDisplayText = dashChargeDisplay.GetComponentInChildren<Text> ();
		dashChargedColor = dashChargeDisplayText.color;

		nbBlink = blinkDuration / blinkTime;
		isBlinking = false;
		pv = GameManager.instance.playerPv;
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

			if (Input.GetButton ("Fire2") && !isDashingInCharge)
				StartCoroutine (Dash (movement));
		}

		//Rotation to follow mouse
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Quaternion PlayerRotation = Quaternion.LookRotation (tr.position - mousePosition, Vector3.forward);
		tr.rotation = PlayerRotation;

		//negate 3D rotation //not needed for orthographic camera
		transform.eulerAngles = new Vector3 (0, 0, transform.eulerAngles.z);
		rb.angularVelocity = 0;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Open Exit") {
			enabled = false;
			NewLevel();
		}
	}

	private void OnDisable()
	{
		GameManager.instance.playerPv = pv;
	}

	IEnumerator Dash(Vector3 movement)
	{
		
		speed = speed * dashSpeed;
		isDashing = true;
		isDashingInCharge = true;
		dashSound.Play ();
		dashChargeDisplayText.color = Color.grey;
		foreach (SpriteRenderer spriteRenderer in sr) {
			spriteRenderer.color = new Color(1f,1f,1f,0.5f);
		}

		yield return new WaitForSeconds(dashTime);

		isDashing = false;
		speed = speed / dashSpeed;
		foreach (SpriteRenderer spriteRenderer in sr) {
			spriteRenderer.color = new Color(1f,1f,1f,1f);
		}

		yield return new WaitForSeconds (dashRecharge);

		dashChargeDisplayText.color = dashChargedColor;
		isDashingInCharge = false;
	}

	IEnumerator Blink()
	{
		isBlinking = true;
		int locNbBlink = (int)nbBlink;

		while (locNbBlink > 0f) {
			locNbBlink -= 1;

			foreach (SpriteRenderer spriteRenderer in sr) {
				spriteRenderer.enabled = !spriteRenderer.enabled;
			}
			yield return new WaitForSeconds(blinkTime);
		}

		foreach (SpriteRenderer spriteRenderer in sr) {
			spriteRenderer.enabled = true;
		}
		isBlinking = false;
	}

	public void LosePv (float damage, GameObject impact)
	{
		if (isBlinking == false && isDashing == false) {
			
			Instantiate(impact, tr.position, tr.rotation);
			pv = pv - damage;
			heart = GameObject.FindWithTag("Heart");
			heart.SetActive (false);

			if (pv <= 0) {
				GameManager.instance.GameOver();

			} else {
				StartCoroutine (Blink ());
			}
		}
	}

	private void NewLevel()
	{
		GameManager.instance.timer += 5f;
		SceneManager.LoadScene ("main");
	}
}
