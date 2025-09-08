using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManagerScript : MonoBehaviour
{
    CookieManagerScript cookieManagerScript;
    GameManagerScript gameManagerScript;
    public bool isTutorialActive = false;
    public bool wagerConfirmed = false;
    public GameObject tutorialBox;
    public GameObject tutorialUIText;

    public GameObject hpArrow;
    public GameObject pointArrow;

    public GameObject CookieWagerParent;
    public GameObject playerField;

    private void Start()
    {
        cookieManagerScript = FindObjectOfType<CookieManagerScript>();
        gameManagerScript = FindObjectOfType<GameManagerScript>();
        if (gameManagerScript.selectedDifficulty != 1)
        {
            tutorialBox.SetActive(false);
            tutorialUIText.SetActive(false);
            return;
        }
        else
        {
            tutorialBox.SetActive(true);
            tutorialUIText.SetActive(true);
        }

        if (SceneManager.GetActiveScene().name == "GameplayScene" && gameManagerScript.selectedDifficulty == 1)
        {
            Text tutorialText = tutorialUIText.GetComponent<Text>();
            StartCoroutine(TutorialSequence(tutorialText));

        }
        else if(SceneManager.GetActiveScene().name == "ShopScene" && gameManagerScript.selectedDifficulty == 1)
        {
            StartCoroutine(ShopTutorial());
        }
        else if(SceneManager.GetActiveScene().name == "LevelSelectScene" && gameManagerScript.selectedDifficulty == 1)
        {
            StartCoroutine(LevelSelectTutorial());
        }
    }

    IEnumerator TutorialSequence(Text text)
    {
        tutorialBox.SetActive(true);
        text.text = "Drag cookies to the plate to place your bet. Each cookie is worth 10 crumbs";
        while (cookieManagerScript.wageredCookies == 0)
        {
            yield return null;
        }
        text.text = "The more you wager, the more risk of winning or losing it all";
        yield return new WaitForSeconds(3f);
        while (!wagerConfirmed)
        {
            text.text = "Click the confirm button to lock in your wager";
            yield return null;
        }
        pointArrow.SetActive(true);
        text.text = "On the right side displays you and your opponent's total points";
        yield return new WaitForSeconds(5f);
        pointArrow.SetActive(false);
        hpArrow.SetActive(true);
        text.text = "On the left side displays both you and your opponent's HP";
        yield return new WaitForSeconds(5f);
        hpArrow.SetActive(false);
        text.text = "Each turn you have to play a card as long as its a legal move";
        yield return new WaitForSeconds(5f);
        text.text = "Each round of cards are evaluated by whichever has the highest points";
        yield return new WaitForSeconds(5f);
        text.text = "If you end the round above 21 points,";
        yield return new WaitForSeconds(3f);
        text.text = "you WILL be penalized for the excessive amount of points";
        yield return new WaitForSeconds(3f);
        text.text = "";

        GameManagerScript gameManager = FindObjectOfType<GameManagerScript>();

        gameManager.PlayerDeckManagerScript.handInitializationCoroutine = StartCoroutine(gameManager.PlayerDeckManagerScript.InitializeHand());
        gameManager.EnemyDeckManagerScript.handInitializationCoroutine = StartCoroutine(gameManager.EnemyDeckManagerScript.InitializeHand());

        gameManager.StartPlayerTurn();

        while (playerField.transform.childCount < 2)
        {
            text.text = "Drag a card from your hand to the play area to play it";
            yield return null;
        }
        text.text = "";
        tutorialUIText.SetActive(false);
        tutorialBox.SetActive(false);

    }

    IEnumerator ShopTutorial()
    {
        tutorialBox.SetActive(true);
        tutorialUIText.SetActive(true);

        Text text = tutorialUIText.GetComponent<Text>();
        text.text = "Welcome to the shop! Here you can buy new cards to add to your deck.";
        yield return new WaitForSeconds(5f);
        text.text = "To buy a card into your deck, you need to drag it into the green box.";
        yield return new WaitForSeconds(5f);
        text.text = "However, each deck can only have 7 cards max,";
        yield return new WaitForSeconds(3f);
        text.text = " so get rid of unwanted cards from your deck by dragging it to the trashbin";
        yield return new WaitForSeconds(5f);
        text.text = "";

        tutorialUIText.SetActive(false);
        tutorialBox.SetActive(false);
    }

    IEnumerator LevelSelectTutorial()
    {
        tutorialBox.SetActive(true);
        tutorialUIText.SetActive(true);

        Text text = tutorialUIText.GetComponent<Text>();
        text.text = "Welcome to the underworld, choom! Here you can choose to gamble with the mafia crook, manager, or supervisor from the left building to the right";
        yield return new WaitForSeconds(10f);
        text.text = "The higher the risk, the higher the reward. But be careful, if you lose all your cookies, it's game over!";
        yield return new WaitForSeconds(7f);
        text.text = "";

        tutorialUIText.SetActive(false);
        tutorialBox.SetActive(false);
    }
    public void ConfirmWager()
    {
        wagerConfirmed = true;
    }
}
