using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class MusicManager : MonoBehaviour {

    public AudioClip mainMusic;
    public AudioClip menuMusic;

    string sceneName;

    void Start()
    {
        OnLevelWasLoaded(0);
    }
    
    void OnLevelWasLoaded(int sceneIndex)
    {
        string newSceneName = SceneManager.GetActiveScene().name;
        if(newSceneName != sceneName)
        {
            sceneName = newSceneName;
            Invoke("PlayMusic", .2f);
        }
    }

    void PlayMusic()
    {
        AudioClip clip = null;
        if(sceneName == "Menu")
        {
            clip = menuMusic;            
        }else if(sceneName == "SupaScene")
        {
            clip = mainMusic;
        }
        if(clip != null)
        {
            AudioManager.instance.PlayMusic(clip, 2);
            Invoke("PlayMusic", clip.length);
        }
    }
}
