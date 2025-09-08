using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadListenerScript : MonoBehaviour
{
    public GameManagerScript gameManager;
    public AudioManagerScript audioManager;

    SceneLoadListenerScript instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
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
            gameManager.InitializeGameplayManagers();
            gameManager.CookieManagerScript.IntializeCookieWagerMechanic();

            audioManager.StopMusic();
            audioManager.musicSource.clip = audioManager.mainGameplayBGM;
            audioManager.musicSource.loop = true;
            audioManager.musicSource.Play();
        }
    }
}
