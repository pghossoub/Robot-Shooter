using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	public Button resume;
	public Button restart;
	public Button quit;

	private GameManager gameManager;

	void Start () 
	{
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		resume.onClick.AddListener (gameManager.TogglePause);
		restart.onClick.AddListener (gameManager.TogglePause);
		restart.onClick.AddListener (gameManager.Restart);
		quit.onClick.AddListener (gameManager.Quit);
	}
}
