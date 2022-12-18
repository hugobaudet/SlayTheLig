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
        instance = this;
        InitializeAllClips();
    }

    public void PauseOrUnpauseAllClips(bool that)
    {
        if (that)
        {
            allSounds[0].source.Play();
        }
        else
        {
            allSounds[0].source.Pause();
        }
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
            s.source.playOnAwake = false;
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
        s.source.pitch = UnityEngine.Random.Range(s.pitch - 0.1f, s.pitch + 0.1f);
        s.source.Play();
    }
}