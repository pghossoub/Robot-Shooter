using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

	public GameObject countdown;
	public GameObject openExit;
	public GameObject playerDeathExplosion;
	public float levelStartDelay = 2f;
	public static GameManager instance = null;
	public float playerPv;
	public float playerDieDelay = 2f;
	public float timer = 30.0f;

	[HideInInspector] public bool doingSetup = true;

	private BoardManager boardScript;
	private GameObject exit;
	private int level = 0;
	private GameObject levelImage;
	private Text levelText;
	private GameObject[] listEnemy;
	private GameObject player;
	private GameObject mainCamera;
	private bool gameOver = false;
	private Text countdownText;
	private GameObject lifeDisplay;
	private GameObject heart;
	private bool readyToReset = false;


	void Awake () 
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}

	void Start()
	{
		StartCoroutine (CountDown());
	}

	void Update()
	{
		if (readyToReset) 
		{
			if (Input.GetKeyDown (KeyCode.R)) 
			{
				//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
				level = 0;
				playerPv = 3;
				timer = 30f;
				gameOver = false;
				readyToReset = false;
				SceneManager.LoadScene (0);
			}
		}
	}

	IEnumerator CountDown()
	{
		while(true){
			if (!gameOver && !doingSetup) {
				timer -= Time.deltaTime;
				displayTimer ();
				if (timer <= 0) {
					countdownText.text = "0:00";
					GameOver ();
					//break;
				}
			}
			yield return null;
		}
	}

	void displayTimer()
	{
		int minutes = Mathf.FloorToInt(timer / 60F);
		int seconds = Mathf.FloorToInt(timer - minutes * 60);
		string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
		countdownText.text = niceTime;
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
		doingSetup = true;

		lifeDisplay = GameObject.Find("PlayerLife");
		heart = GameObject.Find("Heart");
		for(int i = 0; i < playerPv - 1; i++){
			Instantiate(heart, lifeDisplay.transform);
		}

		countdownText = GameObject.Find("Countdown").GetComponentInChildren<Text> ();
		displayTimer ();

		levelImage = GameObject.Find("LevelImage");
		levelImage.SetActive (true);
		levelText = levelImage.GetComponentInChildren<Text> ();
		levelText.text = "Level " + level;
		Invoke ("HideLevelImage", levelStartDelay);

		boardScript = GameObject.Find("BoardManager").GetComponent<BoardManager> ();
		boardScript.SetupScene(level);
	}

	private void HideLevelImage()
	{
		levelImage.SetActive (false);
		doingSetup = false;
	}

	public void RemoveEnemy(float timeGain)
	{
		timer += timeGain;

		boardScript.nbEnemy--;
	}

	public bool CheckNoEnemyLeft()
	{
		return boardScript.nbEnemy <= 0;
	}

	public void OpenExit()
	{
		exit = GameObject.FindWithTag("Exit");
		Instantiate (openExit, exit.transform.position, exit.transform.rotation);
		exit.SetActive(false);
	}

	public void GameOver()
	{
		gameOver = true;
		player = GameObject.FindGameObjectWithTag("Player");
		Instantiate (playerDeathExplosion, player.transform.position, player.transform.rotation);
		Destroy (player);

		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		mainCamera.GetComponent<CameraController> ().enabled = false;

		Invoke ("deathScreen", playerDieDelay);
	}

	public void deathScreen()
	{
		levelText.text = "You died at level " + level + ".";
		levelImage.SetActive (true);

		listEnemy = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject enemy in listEnemy) {
			Destroy(enemy);
		}
		readyToReset = true;
	}
}
