using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManagerScript : MonoBehaviour
{
    [Header("---------------------- Audio Source ----------------------")]
    public AudioSource musicSource;
    public AudioSource effectSource;


    [Header("----------------------- Audio Clip -----------------------")]
    public AudioClip hoverCard;
    public AudioClip shuffleDeck;
    public AudioClip drawCard;
    public AudioClip playCard;
    public AudioClip buttonPress;
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip trashCard;
    public AudioClip buyCard;



    [Header("----------------------- BGM Clips -----------------------")]
    public AudioClip startMenuBGM;
    public AudioClip levelSelectBGM;
    public AudioClip mainGameplayBGM;

    public static AudioManagerScript instance;

    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    private void Start()
    {
        musicSource.clip = startMenuBGM;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySfx(AudioClip clip)
    {
        if (effectSource != null && clip != null)
        {
            effectSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Effect source or clip is null");
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
        else
        {
            Debug.LogWarning("Music source is null");
        }
    }
}
