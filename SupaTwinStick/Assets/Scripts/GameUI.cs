using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;
using System.Collections;

public class GameUI : MonoBehaviour 
{
	public Image maskScreen;
	public GameObject endGameUI;

	// Use this for initialization
	void Start () 
	{
		FindObjectOfType<Player> ().OnKilled += OnPlayerKilled;
	}
	
	void OnPlayerKilled ()
	{
		StartCoroutine (Fading (Color.clear, Color.black, 2));
		endGameUI.SetActive (true);
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

}
