using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadListenerScript : MonoBehaviour
{
    public GameManagerScript GameManager;
    public AudioManagerScript AudioManager;

    private SceneLoadListenerScript _instance;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "GameplayScene")
        {
            GameManager.InitializeGameplayManagers();
            GameManager.CookieManagerScript.IntializeCookieWagerMechanic();

            AudioManager.StopMusic();
            AudioManager.MusicSource.clip = AudioManager.MainGameplayBGM;
            AudioManager.MusicSource.loop = true;
            AudioManager.MusicSource.Play();
        }
    }
}
