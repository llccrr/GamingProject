using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour 
{

    public Level[] levels;
    public Opponent opponent;
	public event System.Action<int> newLevelTop;

	ProceduralMapGenerator currentMap;
	Killable player;
	Transform playerTransform;
    Level currentLevel;
    int currentLevelNumber;

    int OpponentStillAlive;
    int opponentLeftSpawning;
    float nextSpawnTime;

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
            NextLevel();
        }
    }

	void OnPlayerKilled()
	{
		print("You died ...");
		playerDead = true;

	}

    void NextLevel()
    {
        currentLevelNumber++;

        if(currentLevelNumber - 1 < levels.Length)
        {
            currentLevel = levels[currentLevelNumber - 1];

            opponentLeftSpawning = currentLevel.opponentNbr;
            OpponentStillAlive = opponentLeftSpawning;

			if (newLevelTop != null) 
			{
				newLevelTop (currentLevelNumber);
			}
        }
     
    }

    [System.Serializable]
    public class Level
    {
        public int opponentNbr;
        public float spawnRate;
    }

}
