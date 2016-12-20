using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour 
{

    public Level[] levels;
    public Opponent opponent;
	public Objective objectiveObject;
	public event System.Action<int> newLevelTop;
	Transform playerSpawn;


	ProceduralMapGenerator currentMap;
	Killable player;
	Transform playerTransform;
    Level currentLevel;
    int currentLevelNumber;

    int OpponentStillAlive;
    int opponentLeftSpawning;
    float nextSpawnTime;

	int objectiveCounter;

	bool playerDead;

    void Start()
    {
		player = FindObjectOfType<Player>();
		playerTransform = player.transform;
		currentMap = FindObjectOfType<ProceduralMapGenerator> ();
		player.OnKilled += OnPlayerKilled;
        NextLevel();
    }

    void Update()
    {
		if (!playerDead) {
			if (opponentLeftSpawning > 0 && Time.time > nextSpawnTime) {
				opponentLeftSpawning--;
				nextSpawnTime = Time.time + currentLevel.spawnRate;

				StartCoroutine (OpponentSpawn ());
			}
		}
		if (objectiveCounter < 1) {
			NextLevel ();
		}
    }

	void PlaceObjectivesOnMap(){
		int sizeOfDeadEnds = (int) Mathf.Round(currentMap.GetNumberDeadEnds () / 2f);
		objectiveCounter = sizeOfDeadEnds ;
		for (int i = -1; i < sizeOfDeadEnds -1 ; i++) 
		{
				Transform objectiveSpawn = currentMap.GetPositionDeadEnd ();
				Objective spawnedObjective = (Objective)Instantiate (objectiveObject, objectiveSpawn.position + Vector3.up, Quaternion.identity);
				spawnedObjective.OnActivated += ObjectiveCountDown;
		}

	}

	void ReplacePlayerToDeadEnd(){
		playerSpawn = currentMap.GetPositionDeadEnd ();
		playerTransform.position = playerSpawn.position + Vector3.up * 2;
	}

	IEnumerator OpponentSpawn ()
	{
		float flashingSpeed = 4;
		float spawnRate = 1;
		float spawnTimer = 0;
		Transform spawnLocation = currentMap.GetRandomFreeCoordinates ();
		Material tileSkin = spawnLocation.GetComponent<Renderer> ().material;
		Color normalSkin = tileSkin.color;
		Color flashingSkin = Color.black;

		while (spawnTimer < spawnRate) 
		{
			tileSkin.color = Color.Lerp (normalSkin, flashingSkin, Mathf.PingPong (spawnTimer * flashingSpeed, 1));
			spawnTimer += Time.deltaTime;
			yield return null;
		}

		Opponent spawnedOpponent = (Opponent)Instantiate(opponent, spawnLocation.position + Vector3.up, Quaternion.identity);
		spawnedOpponent.OnKilled += OnOpponentKilled;
	}

    void OnOpponentKilled()
    {
        print("An opponent has been slain");
        OpponentStillAlive--;
        if(OpponentStillAlive == 0)
        {
            //NextLevel();
        }
    }

	void OnPlayerKilled()
	{
		print("You died ...");
		playerDead = true;

	}

    void NextLevel()
    {
		GameObject[] toReset = GameObject.FindGameObjectsWithTag("Resetable");
		print (toReset.Length);
		if (toReset.Length > 0) {
			foreach (GameObject ob in toReset)
			{
				Destroy (ob);
			}
		}

        currentLevelNumber++;

		if (currentLevelNumber - 1 < levels.Length) {
			currentLevel = levels [currentLevelNumber - 1];

			opponentLeftSpawning = currentLevel.opponentNbr;
			OpponentStillAlive = opponentLeftSpawning;

			if (newLevelTop != null) {
				newLevelTop (currentLevelNumber);
			}
			ReplacePlayerToDeadEnd ();
			PlaceObjectivesOnMap ();

		} else {
			player.Die();
		}
     
    }

	void ObjectiveCountDown(){
		objectiveCounter -= 1;
	}

	public int getObjectifCounter(){
		return objectiveCounter;
	}

    [System.Serializable]
    public class Level
    {
        public int opponentNbr;
        public float spawnRate;
    }

}
