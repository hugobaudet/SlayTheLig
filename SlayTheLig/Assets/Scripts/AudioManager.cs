using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Sounds
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool onlyOneCanBePlayed;
    public bool loop;

    [HideInInspector]
    public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sounds[] allSounds;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one AudioManager in the scene");
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
        InitializeAllClips();
    }


    /// <summary>
    /// Create an audio source for each clip and set it with the right parameter
    /// </summary>
    void InitializeAllClips()
    {
        foreach (Sounds s in allSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
            if (s.loop)
            {
                s.source.Play();
            }
            s.source.playOnAwake = s.loop;
        }
    }

    public void PauseOrUnpauseAllClips(bool that)
    {
        for (int i = 0; i < allSounds.Length; i++)
        {
            if (that)
            {
                allSounds[i].source.Play();
            }
            else
            {
                allSounds[i].source.Pause();
            }
        }
    }

    public void PlayClip(string name)
    {
        if (name == "")
        {
            Debug.LogWarning("Enter a clip name !");
            return;
        }
        Sounds s = Array.Find(allSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("The clip " + name + " doesn't exist !");
            return;
        }
        if (s.source.isPlaying && s.onlyOneCanBePlayed) return;
        s.source.Play();
    }
}