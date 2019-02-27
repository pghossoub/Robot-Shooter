using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject boardManagerObject;

	// Use this for initialization
	void Start () {
		BoardManager boardScript = boardManagerObject.GetComponent<BoardManager> ();
		boardScript.SetupScene ();
	}
}
