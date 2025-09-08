using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour
{
    public Sprite[] levelBackgrounds; //assign in inspector
    public Image backgroundImage; //assign in inspector

    public Text playerHealth;
    public Text playerMaxHealth;
    public Text enemyHealth;
    public Text enemyMaxHealth;
    public Text multiplierText;

    public Text playerPoint;
    public Text enemyPoint;

    public Text wagered;
    public Text totalWinnings;

    public GameObject matchResultGroup;
    public GameObject initialMatchResult;

    public Coroutine winCoroutine;

    public Coroutine finalBossWinCoroutine;
    public GameObject finalWinGroup;
    public Image finalWinBackground;
    public Text finalWinTextOne;
    public Text finalWinTextTwo;
    public GameObject finalWinTextThree;
    public GameObject finalWinButton;

    public GameObject shopButton;

    FieldManagerScript playerField;
    FieldManagerScript enemyField;

    CookieManagerScript cookieManager;

    GameManagerScript gameManager;



    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        cookieManager = GameObject.FindGameObjectWithTag("CookieManager").GetComponent<CookieManagerScript>();
        playerHealth = GameObject.Find("PlayerHealthText").GetComponent<Text>();
        playerMaxHealth = GameObject.Find("PlayerMaxHealthText").GetComponent<Text>();
        enemyHealth = GameObject.Find("EnemyHealthText").GetComponent<Text>();
        enemyMaxHealth = GameObject.Find("EnemyMaxHealthText").GetComponent<Text>();
        matchResultGroup = GameObject.Find("MatchResultGroup");
        initialMatchResult = GameObject.Find("InitialMatchResultGroup");
        multiplierText = GameObject.Find("Multiplier-Text").GetComponent<Text>();
        wagered = GameObject.Find("Bets-Text-Nominal").GetComponent<Text>();
        totalWinnings = GameObject.Find("TotalWinnings-Text-Nominal").GetComponent<Text>();
        shopButton = GameObject.Find("SHOP-Button");

        //sets up shop button to claim rewards and go to shop scene
        shopButton.GetComponent<Button>().onClick.AddListener(() => cookieManager.ClaimRewards(GameManagerScript.instance.rewardMultiplier));
        shopButton.GetComponent<Button>().onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene("ShopScene"));

        shopButton.SetActive(false);
        matchResultGroup.SetActive(false);
        initialMatchResult.SetActive(false);

        //winCoroutine = StartCoroutine(DisplayWinScreen());
    }

    void Update()
    {
        playerHealth.text = GameManagerScript.instance.PlayerHealth.ToString();
        playerMaxHealth.text = GameManagerScript.instance.PlayerMaxHealth.ToString();
        enemyHealth.text = GameManagerScript.instance.EnemyHealth.ToString();
        enemyMaxHealth.text = GameManagerScript.instance.EnemyMaxHealth.ToString();

        if (gameManager.selectedDifficulty <= 2)
        {
            backgroundImage.sprite = levelBackgrounds[0];
        }
        else if (gameManager.selectedDifficulty > 2 && gameManager.selectedDifficulty <= 4)
        {
            backgroundImage.sprite = levelBackgrounds[1];
        }
        else if (gameManager.selectedDifficulty == 5)
        {
            backgroundImage.sprite = levelBackgrounds[2];
        }

        UpdateEnemyPoints();
        UpdatePlayerPoints();
    }

    void UpdateEnemyPoints()
    {
        enemyField = GameObject.Find("EnemyFieldManager").GetComponent<FieldManagerScript>();
        int points = enemyField.totalCardValue + GameManagerScript.instance.additionalEnemyPoints;
        enemyPoint.text = points.ToString();
    }

    void UpdatePlayerPoints()
    {
        playerField = GameObject.Find("PlayerFieldManager").GetComponent<FieldManagerScript>();
        int points = playerField.totalCardValue + GameManagerScript.instance.additionalPlayerPoints;
        playerPoint.text = points.ToString();
    }

    public IEnumerator DisplayWinScreen()
    {
        int rewardMultiplier = GameManagerScript.instance.rewardMultiplier + 1;
        multiplierText.text = "x " + rewardMultiplier.ToString() + "00%";

        int wageredAmount = GameObject.FindGameObjectWithTag("CookieManager").GetComponent<CookieManagerScript>().wageredCookies;
        int totalReward = wageredAmount * rewardMultiplier;

        wagered.text = wageredAmount.ToString();
        totalWinnings.text = totalReward.ToString();

        initialMatchResult.SetActive(true);
        yield return new WaitForSeconds(3f);
        matchResultGroup.SetActive(true);

        float timer = 0f;
        while(timer < 1f)
        {
            timer += Time.deltaTime;
            matchResultGroup.transform.localPosition = Vector3.Lerp(matchResultGroup.transform.localPosition, new Vector3(0, 0, 0), timer);
            yield return null;
        }
        //initialMatchResult.SetActive(false);
        shopButton.SetActive(true);

        winCoroutine = null;
    }

    public IEnumerator FinalBossWinTransition()
    {
        finalWinGroup.SetActive(true);
        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime;
            finalWinBackground.color = new Color(0f, 0f, 0f, timer);
            finalWinTextOne.color = new Color(1f, 1f, 1f, timer);
            finalWinTextTwo.color = new Color(1f, 1f, 1f, timer);
            yield return null;
        }
        finalWinTextThree.SetActive(true);
        finalWinButton.SetActive(true);
        finalBossWinCoroutine = null;
    }
}
