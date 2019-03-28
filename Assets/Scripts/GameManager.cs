using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

	//public GameObject boardManagerObject;
	public GameObject openExit;
	public float levelStartDelay = 2f;
	public static GameManager instance = null;

	[HideInInspector] public bool doingSetup = true;

	private BoardManager boardScript;
	private GameObject exit;
	private int level = 0;
	private GameObject levelImage;
	private Text levelText;

	void Awake () 
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
		//boardScript.SetupScene ();
	}

	//This is called each time a scene is loaded.
	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		//Add one to our level number.
		level++;
		//Call InitGame to initialize our level.
		InitGame();
	}

	void OnEnable()
	{
		//Tell our ‘OnLevelFinishedLoading’ function to start listening for a scene change event as soon as this script is enabled.
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnDisable()
	{
		//Tell our ‘OnLevelFinishedLoading’ function to stop listening for a scene change event as soon as this script is disabled. 
		//Remember to always have an unsubscription for every delegate you subscribe to!
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	void InitGame()
	{
		boardScript = GameObject.Find("BoardManager").GetComponent<BoardManager> ();
		doingSetup = true;
		levelImage = GameObject.Find("LevelImage");
		levelImage.SetActive (true);
		levelText = levelImage.GetComponentInChildren<Text> ();
		levelText.text = "Level " + level;
		Invoke ("HideLevelImage", levelStartDelay);
		boardScript.SetupScene();
	}

	private void HideLevelImage()
	{
		levelImage.SetActive (false);
		doingSetup = false;
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
