using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;
using System.Collections;

public class GameUI : MonoBehaviour 
{
	public Image maskScreen;
	public GameObject endGameUI;
	public Text floorLevel;
	public Text floorObjectives;
	public RectTransform floorBanner;
    public Text scoreUI;
    public RectTransform lifeBar;

	Spawner spawn;
    Player player;
	// Use this for initialization
	void Start () 
	{
        player = FindObjectOfType<Player>();
		player.OnKilled += OnPlayerKilled;
	}
	
	void OnPlayerKilled ()
	{
		StartCoroutine (Fading (Color.clear, Color.black, 2));
		endGameUI.SetActive (true);
	}

	void Awake()
	{
		spawn = FindObjectOfType<Spawner>();
		spawn.newLevelTop += newLevelTop;
	}

    void Update()
    {
        scoreUI.text = ScoreManager.score.ToString("D6");
        if(player != null)
        {
            float lifePercent = player.lifePoints / player.beginningLifePoints;
            lifeBar.localScale = new Vector3(lifePercent, 1, 1);
        }
       
    }
	void newLevelTop(int floorNumber)
	{
		string[] numbersStrings = { "First", "Second", "Third", "Fourth", "Fith", "Sixth", "Seventh", "Eighth", "Nineth", "Tenth" };
		floorLevel.text = " - " + numbersStrings [floorNumber - 1] + "Floor  -";
		floorObjectives.text = "Activate the objectives";

		StartCoroutine (AnimateFloorBanner ());
	}

	IEnumerator AnimateFloorBanner()
	{

		float delay = 1.5f;
		float moveSpeed = 1.5f;
		float percentage = 0;
		int direction = 1;

		float delayTime = Time.time + delay + 1 / moveSpeed;
		while (percentage >= 0) 
		{
			percentage += Time.deltaTime * moveSpeed * direction;

			if (percentage >= 1) {
				percentage = 1;
				if (Time.time > delayTime) {
					direction = -direction;
				}
			}

			floorBanner.anchoredPosition = Vector2.up * Mathf.Lerp (-430, -200, percentage);
			yield return null;
		}
	}

	IEnumerator Fading(Color initialColor, Color finalColor, float timer)
	{
		float fadingSpeed = 1 / timer;
		float fadingPercent = 0;

		while (fadingPercent < 1) 
		{
			fadingPercent += fadingSpeed * Time.deltaTime;
			maskScreen.color = Color.Lerp (initialColor, finalColor, fadingPercent);
			yield return null;
		}
	}

	public void StartGame()
	{
        SceneManager.LoadScene("SupaScene");
	}
    public void ReturnMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
