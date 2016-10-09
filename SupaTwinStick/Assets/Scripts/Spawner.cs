using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public Level[] levels;
    public Opponent opponent;

    Level currentLevel;
    int currentLevelNumber;

    int OpponentStillAlive;
    int opponentLeftSpawning;
    float nextSpawnTime;

    void Start()
    {
        NextLevel();
    }

    void Update()
    {
        if(opponentLeftSpawning > 0 && Time.time > nextSpawnTime)
        {
            opponentLeftSpawning--;
            nextSpawnTime = Time.time + currentLevel.spawnRate;

            Opponent spawnedOpponent = (Opponent)Instantiate(opponent, Vector3.zero, Quaternion.identity);
            spawnedOpponent.OnKilled += OnOpponentKilled;
        }
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

    void NextLevel()
    {
        currentLevelNumber++;
        print("Level : " + currentLevelNumber);
        if(currentLevelNumber - 1 < levels.Length)
        {
            currentLevel = levels[currentLevelNumber - 1];

            opponentLeftSpawning = currentLevel.opponentNbr;
            OpponentStillAlive = opponentLeftSpawning;
        }
     
    }

    [System.Serializable]
    public class Level
    {
        public int opponentNbr;
        public float spawnRate;
    }

}
