using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerScript : MonoBehaviour
{
    private AudioManagerScript _audioManager;
    public GameObject FinalBossTransition;
    private CookieManagerScript _cookieManager;
    private GameManagerScript _gameManager;
    public Button[] Buttons;

    public GameObject ErrorMessage;

    public void PlayGame()
    {
        _audioManager.PlaySfx(_audioManager.ButtonPress);
        SceneManager.LoadScene("GameplayScene");
    }
    public void OpenShopScene()
    {
        _audioManager.PlaySfx(_audioManager.ButtonPress);
        SceneManager.LoadScene("ShopScene");
    }
    public void OpenMainMenu()
    {
        _audioManager.PlaySfx(_audioManager.ButtonPress);
        SceneManager.LoadScene("StartMenuScene");
    }
    public void GameOver()
    {
        _audioManager.MusicSource.Stop();
        _audioManager.PlaySfx(_audioManager.LoseSound);
        _cookieManager.PlayerCookies = 50;
        _gameManager.SelectedDifficulty = 1;
        _gameManager.TurnsLeft = 5;
        _gameManager.DebtAmount = 500;
        _gameManager.PlayerDeckManagerScript.PlayingDeck.Clear();
        _gameManager.PlayerDeckManagerScript.PlayingDeck.AddRange(_gameManager.PlayerDeckManagerScript.ResourceDeck);
        SceneManager.LoadScene("GameOverScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LevelSelectScene()
    {
        if (_gameManager.TurnsLeft > 0)
        {
            _audioManager.PlaySfx(_audioManager.ButtonPress);
            SceneManager.LoadScene("LevelSelectScene");
        }
        else
        {
            if(_cookieManager.PlayerCookies < _gameManager.DebtAmount)
            {
                GameOver();
            }
            else
            {
                _gameManager.SelectedDifficulty = 5;
                PlayGame();
            }

        }
    }

    private void Start()
    {
        _audioManager = FindObjectOfType<AudioManagerScript>();
        _gameManager = FindObjectOfType<GameManagerScript>();
        _cookieManager = FindObjectOfType<CookieManagerScript>();

        if (_gameManager.SelectedDifficulty == 5 && SceneManager.GetActiveScene().name == "GameplayScene")
        {
            if(FinalBossTransition == null)
            {
                FinalBossTransition = GameObject.Find("FinalBossTransition");
            }
            FinalBossTransition.SetActive(true);
        }

        if (SceneManager.GetActiveScene().name == "LevelSelectScene")
        {
            Buttons[0].onClick.AddListener(() => ChangeValue(2));
            Buttons[0].onClick.AddListener(() => PlayGame());

            if(_cookieManager.PlayerCookies < 50)
            {
                Buttons[1].onClick.AddListener(() => DisplayErrorMessage());
            }
            else
            {
                Buttons[1].onClick.AddListener(() => ChangeValue(3));
                Buttons[1].onClick.AddListener(() => PlayGame());
            }

            if(_cookieManager.PlayerCookies < 100)
            {
                Buttons[2].onClick.AddListener(() => DisplayErrorMessage());
            }
            else
            {
                Buttons[2].onClick.AddListener(() => ChangeValue(4));
                Buttons[2].onClick.AddListener(() => PlayGame());
            }

            //if (cookieManager.playerCookies < 100)
            //{
            //    buttons[2].interactable = false;
            //    if(cookieManager.playerCookies < 50)
            //    {
            //        buttons[1].interactable = false;
            //    }
            //}
        }
    }

    void ChangeValue(int value)
    {
        _gameManager.SelectedDifficulty = value;
        Debug.Log("Selected difficulty: " + _gameManager.SelectedDifficulty + "; Input: " + value);
    }

    void DisplayErrorMessage()
    {
        ErrorMessage.SetActive(true);
        ErrorMessage.GetComponentInChildren<Text>().text = "Not enough Cookies!";
    }
}
