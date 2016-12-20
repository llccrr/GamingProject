using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {

    public static int score { get; private set; }
    float lastOpponentDeathTime;
    int streakCount;
    float streakExpireTime = 1;

    void Start()
    {
        Opponent.OnDeathStatic += OnOpponentDeath;
        FindObjectOfType<Player>().OnKilled += OnPlayerDeath;
    }

    void OnOpponentDeath()
    {
        if (Time.time < lastOpponentDeathTime + streakExpireTime)
        {
            streakCount++;

        }
        else {
            streakCount = 0;
        }
        lastOpponentDeathTime = Time.time;

        score += 5 + (int)Mathf.Pow(2, streakCount);
    }

    void OnPlayerDeath()
    {
        Opponent.OnDeathStatic -= OnOpponentDeath;
    }

}
