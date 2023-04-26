using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource, sfxSource;
    public AudioClip[] music;
    public AudioClip click, death, purchase, claim;

    private int musicIndex = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
    public static AudioManager Instance { get; private set; }
    private AudioManager() { }

    // Start is called before the first frame update
    void Start()
    {
        music = ShufflePlaylist();
    }

    // Update is called once per frame
    void Update()
    {
        if (!musicSource.isPlaying)
        {
            PlayNextSong();
        }
    }

    private void PlayNextSong()
    {
        musicSource.clip = music[musicIndex];
        musicSource.Play();
        musicIndex++;
        if (musicIndex >= music.Length)
        {
            musicIndex = 0;
            music = ShufflePlaylist();
        }
    }

    private AudioClip[] ShufflePlaylist()
    {
        System.Random random = new();
        for (int i = music.Length - 1; i >= 1; i--)
        {
            int j = random.Next(i + 1);
            (music[i], music[j]) = (music[j], music[i]);
        }
        return music;
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
