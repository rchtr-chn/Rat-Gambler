using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManagerScript : MonoBehaviour
{
    private CookieManagerScript _cookieManagerScript;
    private GameManagerScript _gameManagerScript;
    public bool IsTutorialActive = false;
    public bool WagerConfirmed = false;
    public GameObject TutorialBox;
    public GameObject TutorialUIText;

    public GameObject HpArrow;
    public GameObject PointArrow;

    public GameObject CookieWagerParent;
    public GameObject PlayerField;

    private void Start()
    {
        _cookieManagerScript = FindObjectOfType<CookieManagerScript>();
        _gameManagerScript = FindObjectOfType<GameManagerScript>();
        if (_gameManagerScript.SelectedDifficulty != 1)
        {
            TutorialBox.SetActive(false);
            TutorialUIText.SetActive(false);
            return;
        }
        else
        {
            TutorialBox.SetActive(true);
            TutorialUIText.SetActive(true);
        }

        if (SceneManager.GetActiveScene().name == "GameplayScene" && _gameManagerScript.SelectedDifficulty == 1)
        {
            Text tutorialText = TutorialUIText.GetComponent<Text>();
            StartCoroutine(TutorialSequence(tutorialText));

        }
        else if(SceneManager.GetActiveScene().name == "ShopScene" && _gameManagerScript.SelectedDifficulty == 1)
        {
            StartCoroutine(ShopTutorial());
        }
        else if(SceneManager.GetActiveScene().name == "LevelSelectScene" && _gameManagerScript.SelectedDifficulty == 1)
        {
            StartCoroutine(LevelSelectTutorial());
        }
    }

    IEnumerator TutorialSequence(Text text)
    {
        TutorialBox.SetActive(true);
        text.text = "Drag cookies to the plate to place your bet. Each cookie is worth 10 crumbs";
        while (_cookieManagerScript.WageredCookies == 0)
        {
            yield return null;
        }
        text.text = "The more you wager, the more risk of winning or losing it all";
        yield return new WaitForSeconds(3f);
        while (!WagerConfirmed)
        {
            text.text = "Click the confirm button to lock in your wager";
            yield return null;
        }
        PointArrow.SetActive(true);
        text.text = "On the right side displays you and your opponent's total points";
        yield return new WaitForSeconds(5f);
        PointArrow.SetActive(false);
        HpArrow.SetActive(true);
        text.text = "On the left side displays both you and your opponent's HP";
        yield return new WaitForSeconds(5f);
        HpArrow.SetActive(false);
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

        gameManager.PlayerDeckManagerScript.HandInitializationCoroutine = StartCoroutine(gameManager.PlayerDeckManagerScript.InitializeHand());
        gameManager.EnemyDeckManagerScript.HandInitializationCoroutine = StartCoroutine(gameManager.EnemyDeckManagerScript.InitializeHand());

        gameManager.StartPlayerTurn();

        while (PlayerField.transform.childCount < 2)
        {
            text.text = "Drag a card from your hand to the play area to play it";
            yield return null;
        }
        text.text = "";
        TutorialUIText.SetActive(false);
        TutorialBox.SetActive(false);

    }

    IEnumerator ShopTutorial()
    {
        TutorialBox.SetActive(true);
        TutorialUIText.SetActive(true);

        Text text = TutorialUIText.GetComponent<Text>();
        text.text = "Welcome to the shop! Here you can buy new cards to add to your deck.";
        yield return new WaitForSeconds(5f);
        text.text = "To buy a card into your deck, you need to drag it into the green box.";
        yield return new WaitForSeconds(5f);
        text.text = "However, each deck can only have 7 cards max,";
        yield return new WaitForSeconds(3f);
        text.text = " so get rid of unwanted cards from your deck by dragging it to the trashbin";
        yield return new WaitForSeconds(5f);
        text.text = "";

        TutorialUIText.SetActive(false);
        TutorialBox.SetActive(false);
    }

    IEnumerator LevelSelectTutorial()
    {
        TutorialBox.SetActive(true);
        TutorialUIText.SetActive(true);

        Text text = TutorialUIText.GetComponent<Text>();
        text.text = "Welcome to the underworld, choom! Here you can choose to gamble with the mafia crook, manager, or supervisor from the left building to the right";
        yield return new WaitForSeconds(10f);
        text.text = "The higher the risk, the higher the reward. But be careful, if you lose all your cookies, it's game over!";
        yield return new WaitForSeconds(7f);
        text.text = "";

        TutorialUIText.SetActive(false);
        TutorialBox.SetActive(false);
    }
    public void ConfirmWager()
    {
        WagerConfirmed = true;
    }
}
