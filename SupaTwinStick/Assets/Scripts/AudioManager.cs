using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    float masterVolumePercent = 1;
    float sfxVolumePercent = 1;
    float musicVolumePercent = 1;

    AudioSource[] musicSources;
    int activeMusicSourceIndex;

    //Singleton
    public static AudioManager instance;

    Transform audioListener;
    Transform playerT;
    void Awake()
    {
        instance = this;
        musicSources = new AudioSource[2];
        for(int i = 0; i < 2; i++)
        {
            GameObject newMusicSource = new GameObject("Music source " + (i + 1));
            musicSources[i] = newMusicSource.AddComponent<AudioSource>();
            newMusicSource.transform.parent = transform;
        }
        audioListener = FindObjectOfType<AudioListener>().transform;
        playerT = FindObjectOfType<Player>().transform;
    }
    public void Update()
    {
        if(playerT != null)
        {
            audioListener.position = playerT.position;
        }
    }

    public void PlayMusic(AudioClip clip, float fadeDuration = 1)
    {
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;
        musicSources[activeMusicSourceIndex].clip = clip;
        musicSources[activeMusicSourceIndex].Play();

        StartCoroutine(AnimateMusicCrossFade(fadeDuration));


    }
    public void PlaySound(AudioClip clip, Vector3 pos)
    {
        //playclip adapted for short sound like sfx
        if(clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos, sfxVolumePercent * masterVolumePercent);

        }
    }

    IEnumerator AnimateMusicCrossFade(float duration)
    {
        float percent = 0;
        while(percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0, musicVolumePercent * masterVolumePercent, percent);
            musicSources[1- activeMusicSourceIndex].volume = Mathf.Lerp(musicVolumePercent * masterVolumePercent, 0, percent);
            yield return null;
        }
    }
}
