using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    [Header("---------------------- Audio Source ----------------------")]
    public AudioSource MusicSource;
    public AudioSource EffectSource;


    [Header("----------------------- Audio Clip -----------------------")]
    public AudioClip HoverCard;
    public AudioClip ShuffleDeck;
    public AudioClip DrawCard;
    public AudioClip PlayCard;
    public AudioClip ButtonPress;
    public AudioClip WinSound;
    public AudioClip LoseSound;
    public AudioClip TrashCard;
    public AudioClip BuyCard;



    [Header("----------------------- BGM Clips -----------------------")]
    public AudioClip StartMenuBGM;
    public AudioClip LevelSelectBGM;
    public AudioClip MainGameplayBGM;

    public static AudioManagerScript Instance;

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
        MusicSource.clip = StartMenuBGM;
        MusicSource.loop = true;
        MusicSource.Play();
    }

    public void PlaySfx(AudioClip clip)
    {
        if (EffectSource != null && clip != null)
        {
            EffectSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Effect source or clip is null");
        }
    }

    public void StopMusic()
    {
        if (MusicSource != null)
        {
            MusicSource.Stop();
        }
        else
        {
            Debug.LogWarning("Music source is null");
        }
    }
}
