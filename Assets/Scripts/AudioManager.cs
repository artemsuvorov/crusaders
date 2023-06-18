using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class Sound 
{
    public string Name;

    public AudioClip Clip;

    [Range(0.0f, 1.0f)]
    public float Volume;

    [Range(0.1f, 3.0f)]
    public float Pitch;

    [HideInInspector]
    public AudioSource AudioSource { get; set; }
}

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private Sound[] sounds;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        foreach (var sound in sounds)
        {
            sound.AudioSource = gameObject.AddComponent<AudioSource>();
            sound.AudioSource.clip = sound.Clip;
            sound.AudioSource.volume = sound.Volume;
            sound.AudioSource.pitch = sound.Pitch;
        }
    }

    private void Start()
    {
        Play("Main Theme");
    }

    public void Play(string name)
    {
        var sound = sounds.FirstOrDefault(s => s.Name == name);
        if (sound is null)
            Debug.LogError($"Sound '{name}' was not found in the Audio Manager.");
        sound?.AudioSource.Play();
    }
}
