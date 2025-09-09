using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerScript : MonoBehaviour
{
    AudioManagerScript audioManager;
    public GameObject finalBossTransition;
    CookieManagerScript cookieManager;
    GameManagerScript gameManager;
    public Button[] buttons;

    public GameObject errorMessage;

    public void PlayGame()
    {
        audioManager.PlaySfx(audioManager.buttonPress);
        SceneManager.LoadScene("GameplayScene");
    }
    public void OpenShopScene()
    {
        audioManager.PlaySfx(audioManager.buttonPress);
        SceneManager.LoadScene("ShopScene");
    }
    public void OpenMainMenu()
    {
        audioManager.PlaySfx(audioManager.buttonPress);
        SceneManager.LoadScene("StartMenuScene");
    }
    public void GameOver()
    {
        audioManager.musicSource.Stop();
        audioManager.PlaySfx(audioManager.loseSound);
        cookieManager.playerCookies = 50;
        gameManager.selectedDifficulty = 1;
        gameManager.turnsLeft = 5;
        gameManager.debtAmount = 500;
        gameManager.PlayerDeckManagerScript.playingDeck.Clear();
        gameManager.PlayerDeckManagerScript.playingDeck.AddRange(gameManager.PlayerDeckManagerScript.resourceDeck);
        SceneManager.LoadScene("GameOverScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LevelSelectScene()
    {
        if (gameManager.turnsLeft > 0)
        {
            audioManager.PlaySfx(audioManager.buttonPress);
            SceneManager.LoadScene("LevelSelectScene");
        }
        else
        {
            if(cookieManager.playerCookies < gameManager.debtAmount)
            {
                GameOver();
            }
            else
            {
                gameManager.selectedDifficulty = 5;
                PlayGame();
            }

        }
    }

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManagerScript>();
        gameManager = FindObjectOfType<GameManagerScript>();
        cookieManager = FindObjectOfType<CookieManagerScript>();

        if (gameManager.selectedDifficulty == 5 && SceneManager.GetActiveScene().name == "GameplayScene")
        {
            if(finalBossTransition == null)
            {
                finalBossTransition = GameObject.Find("FinalBossTransition");
            }
            finalBossTransition.SetActive(true);
        }

        if (SceneManager.GetActiveScene().name == "LevelSelectScene")
        {
            buttons[0].onClick.AddListener(() => ChangeValue(2));
            buttons[0].onClick.AddListener(() => PlayGame());

            if(cookieManager.playerCookies < 50)
            {
                buttons[1].onClick.AddListener(() => DisplayErrorMessage());
            }
            else
            {
                buttons[1].onClick.AddListener(() => ChangeValue(3));
                buttons[1].onClick.AddListener(() => PlayGame());
            }

            if(cookieManager.playerCookies < 100)
            {
                buttons[2].onClick.AddListener(() => DisplayErrorMessage());
            }
            else
            {
                buttons[2].onClick.AddListener(() => ChangeValue(4));
                buttons[2].onClick.AddListener(() => PlayGame());
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
        gameManager.selectedDifficulty = value;
        Debug.Log("Selected difficulty: " + gameManager.selectedDifficulty + "; Input: " + value);
    }

    void DisplayErrorMessage()
    {
        errorMessage.SetActive(true);
        errorMessage.GetComponentInChildren<Text>().text = "Not enough Cookies!";
    }
}
