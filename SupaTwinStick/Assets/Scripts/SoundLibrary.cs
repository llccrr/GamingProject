using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SoundLibrary : MonoBehaviour
{

    public SoundGroup[] soundGroups;
    Dictionary<string, AudioClip[]> groupDictionnary = new Dictionary<string, AudioClip[]>();

    void Awake()
    {
        foreach (SoundGroup group in soundGroups)
        {
            groupDictionnary.Add(group.groupID, group.group);
        }
    }
    public AudioClip GetClipFromName(string name)
    {
        if (groupDictionnary.ContainsKey(name))
        {
            AudioClip[] sounds = groupDictionnary[name];
            return sounds[Random.Range(0, sounds.Length)];
        }
        return null;
    }

    [System.Serializable]
    public class SoundGroup
    {
        public string groupID;
        public AudioClip[] group;

    }
}
