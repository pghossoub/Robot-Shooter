using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


	public float speed;
	public GameObject mainCamera;
	public GameObject legs;
	private Animator animatorLegs;
	private Rigidbody2D rb;
	private Transform tr;



	// Use this for initialization
	void Start () 
	{
		rb = GetComponent<Rigidbody2D>();
		tr = GetComponent<Transform>();
		animatorLegs = legs.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//rb.velocity = Vector3.forward * speed;

	}

	void FixedUpdate()
	{
		if (Input.GetKey ("right") || Input.GetKey ("left") || Input.GetKey ("up") || Input.GetKey ("down"))
		{
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");

			Vector3 movement = new Vector3 (moveHorizontal, moveVertical, 0.0f);

			//Move with rigid.AddForce
			rb.AddForce(movement * speed);
			animatorLegs.SetTrigger ("Walk");
		}

		//Rotation to follow mouse
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//Vector3 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
		//Debug.Log("mousePosition" + mousePosition);
		Quaternion PlayerRotation = Quaternion.LookRotation (tr.position - mousePosition, Vector3.forward);
		tr.rotation = PlayerRotation;

		//negate 3D rotation //no need in orthographic ?
		transform.eulerAngles = new Vector3 (0, 0, transform.eulerAngles.z);
		rb.angularVelocity = 0;
	}

	IEnumerator SmoothMovement(Vector3 movement)
	{
		float startime = Time.time;
		Vector3 start_pos = tr.position; //Starting position.
		Vector3 end_pos = tr.position + movement; //Ending position.

		while (start_pos != end_pos && ((Time.time - startime) * speed) < 1f) { 
			//float move = Mathf.Lerp (0, 1, (Time.time - startime) * speed);
			rb.AddForce(movement * speed);
			//transform.position += direction * move;

			yield return null;
		}
	}
}
