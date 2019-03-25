using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject boardManagerObject;
	public GameObject openExit;

	private BoardManager boardScript;
	private GameObject exit;

	void Start () {
		boardScript = boardManagerObject.GetComponent<BoardManager> ();
		boardScript.SetupScene ();
	}

	public void RemoveEnemy()
	{
		boardScript.nbEnemy--;
	}

	public bool CheckNoEnemyLeft()
	{
		Debug.Log("Enemy left= " + boardScript.nbEnemy);
		return boardScript.nbEnemy <= 0;
	}

	public void OpenExit()
	{
		Debug.Log("Exit Opened");
		exit = GameObject.FindWithTag("Exit");
		Instantiate (openExit, exit.transform.position, exit.transform.rotation);
	}
}
