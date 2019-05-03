using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlue : RangedEnemy {

	public float dashSpeed;
	public float dashWait;
	public float dashDuration;

	private int[] sideDirections = new int[] {90, -90};
	private bool isReadyToDash = false;

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		StartCoroutine (HorizontalDash ());
	}

	IEnumerator HorizontalDash()
	{
		if (!isReadyToDash) {
			isReadyToDash = true;

			float deltaWaitTime = Random.Range (0f, 0.75f);
			yield return new WaitForSeconds (dashWait + deltaWaitTime);

			isDashing = true;

			Vector3 frontDirection = tr.rotation.eulerAngles + Vector3.up;
			int randomSide = Random.Range (0, 2);
			int angle = sideDirections[randomSide];
			Quaternion newRotation = Quaternion.Euler(0f, 0f, frontDirection.z + angle);

			rb.AddForce (newRotation * frontDirection *  dashSpeed);

			yield return new WaitForSeconds(dashDuration);

			isDashing = false;
			isReadyToDash = false;
		}
	}
}
