using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

	[Serializable]
	public class NumberOf
	{
		public int minimum;
		public int maximum;

		public NumberOf (int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}

	public NumberOf rangeColumns = new NumberOf (10, 18);
	public NumberOf rangeRows = new NumberOf (10, 18);

	public NumberOf wallCount = new NumberOf (10, 20);
	public NumberOf enemyCount = new NumberOf (2, 3);

	public GameObject[] floorTiles;
	public GameObject[] outerWallTiles;
	public GameObject[] wallTiles;
	public GameObject[] enemyTiles;
	public GameObject exitTile;
	public GameObject player;
	public float characterStartDelay = 2f;

	[HideInInspector] public int nbEnemy;

	private Transform boardHolder;
	private List <Vector3> gridPositions = new List<Vector3> ();
	private int rows;
	private int columns;


	void InitialiseList()
	{
		gridPositions.Clear ();

		for (int x = 1; x < columns - 1; x++) {
			for (int y = 1; y < rows - 1; y++) {
				gridPositions.Add (new Vector3 (x, y, 0f));
			}
		}
	}

	void BoardSetup()
	{
		boardHolder = new GameObject ("Board").transform;
		for (int x = -1; x < columns + 1; x++){
			for (int y = -1; y < rows + 1; y++) {
				GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];

				if(x == -1 || x == columns || y == - 1|| y == rows)
					toInstantiate = outerWallTiles[Random.Range (0, outerWallTiles.Length)];

				//GameObject instance = Instantiate(toInstantiate, new Vector3 (x,y,0f), Quaternion.identity) as GameObject;
				GameObject instance = InstantiateInGrid(toInstantiate, new Vector3 (x,y,0f), Quaternion.identity) as GameObject;
				instance.transform.SetParent(boardHolder);
			}
		}
	}

	Vector3 RandomPosition()
	{
		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];
		gridPositions.RemoveAt (randomIndex);
		return randomPosition;
	}

	int LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
	{
		int objectCount = Random.Range (minimum, maximum + 1);

		for(int i = 0; i < objectCount; i++){
			Vector3 randomPosition = RandomPosition ();
			GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
			//Instantiate (tileChoice, randomPosition, Quaternion.identity);
			InstantiateInGrid (tileChoice, randomPosition, Quaternion.identity);
		}
		return objectCount;
	}

	UnityEngine.Object InstantiateInGrid(UnityEngine.Object tile, Vector3 position, Quaternion rotation)
	{
	    Vector3 gridFactor= new Vector3 (0.31f, 0.31f, 1f); //Because tiles are 32x32
		return Instantiate (tile, Vector3.Scale(position, gridFactor), rotation);
	}
		
	IEnumerator SetupCharacters(int level)
	{
		yield return new WaitForSeconds (characterStartDelay);
		int addEnemy = level / 2;
		if (addEnemy > 4)
			addEnemy = 4;
		nbEnemy = LayoutObjectAtRandom (enemyTiles, enemyCount.minimum + addEnemy, enemyCount.maximum + addEnemy);
		player.SetActive(true);
	}

	public void SetupScene(int level)
	{
		rows = Random.Range (rangeRows.minimum, rangeRows.maximum + 1);
		columns = Random.Range (rangeRows.minimum, rangeRows.maximum + 1);

		InstantiateInGrid (exitTile, new Vector3 (columns - 1, rows - 1, 0F), Quaternion.identity);
		BoardSetup ();
		InitialiseList ();
		LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);
		StartCoroutine(SetupCharacters(level));

		//int enemyCount = (int)Mathf.Log (level, 2f);
		//LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
		//Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0F), Quaternion.identity);
		//InstantiateInGrid (exit, new Vector3 (columns - 1, rows - 1, 0F), Quaternion.identity);
	}
}
