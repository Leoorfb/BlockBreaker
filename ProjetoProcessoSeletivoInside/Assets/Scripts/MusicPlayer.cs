using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource musicAudio_;

    public AudioClip[] musicPlaylist;

    private int playlistIndex;

    private static MusicPlayer _instance;
    public static MusicPlayer Instance
    {
        get
        {
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

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
