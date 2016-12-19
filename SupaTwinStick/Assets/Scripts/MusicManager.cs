using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

    public AudioClip mainMusic;
    public AudioClip menuMusic;

    void Start()
    {
        AudioManager.instance.PlayMusic(mainMusic, 10);
    }
    void update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.instance.PlayMusic(menuMusic, 3);
        }
    }
}
