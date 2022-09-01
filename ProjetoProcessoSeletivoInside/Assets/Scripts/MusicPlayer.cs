using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{   
    [Header("Playlist Settings")]
    public AudioClip[] musicPlaylist;
    private AudioSource musicAudio_;

    private int playlistIndex;

    // Singleton
    public static MusicPlayer Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        musicAudio_ = GetComponent<AudioSource>();
        playlistIndex = 0;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!musicAudio_.isPlaying)
        {
            musicAudio_.PlayOneShot(musicPlaylist[playlistIndex]);
            playlistIndex++;
            if (playlistIndex == musicPlaylist.Length) 
                playlistIndex = 0;
        }
    }
}
