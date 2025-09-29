using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour
{
    public Sprite[] LevelBackgrounds; //assign in inspector
    public Image BackgroundImage; //assign in inspector

    public Text PlayerHealth;
    public Text PlayerMaxHealth;
    public Text EnemyHealth;
    public Text EnemyMaxHealth;
    public Text MultiplierText;

    public Text PlayerPoint;
    public Text EnemyPoint;

    public Text Wagered;
    public Text TotalWinnings;

    public GameObject MatchResultGroup;
    public GameObject InitialMatchResult;

    public Coroutine WinCoroutine;

    public Coroutine FinalBossWinCoroutine;
    public GameObject FinalWinGroup;
    public Image FinalWinBackground;
    public Text FinalWinTextOne;
    public Text FinalWinTextTwo;
    public GameObject FinalWinTextThree;
    public GameObject FinalWinButton;

    public GameObject ShopButton;

    private FieldManagerScript _playerField;
    private FieldManagerScript _enemyField;

    private CookieManagerScript _cookieManager;

    private GameManagerScript _gameManager;



    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        _cookieManager = GameObject.FindGameObjectWithTag("CookieManager").GetComponent<CookieManagerScript>();
        PlayerHealth = GameObject.Find("PlayerHealthText").GetComponent<Text>();
        PlayerMaxHealth = GameObject.Find("PlayerMaxHealthText").GetComponent<Text>();
        EnemyHealth = GameObject.Find("EnemyHealthText").GetComponent<Text>();
        EnemyMaxHealth = GameObject.Find("EnemyMaxHealthText").GetComponent<Text>();
        MatchResultGroup = GameObject.Find("MatchResultGroup");
        InitialMatchResult = GameObject.Find("InitialMatchResultGroup");
        MultiplierText = GameObject.Find("Multiplier-Text").GetComponent<Text>();
        Wagered = GameObject.Find("Bets-Text-Nominal").GetComponent<Text>();
        TotalWinnings = GameObject.Find("TotalWinnings-Text-Nominal").GetComponent<Text>();
        ShopButton = GameObject.Find("SHOP-Button");

        //sets up shop button to claim rewards and go to shop scene
        ShopButton.GetComponent<Button>().onClick.AddListener(() => _cookieManager.ClaimRewards(GameManagerScript.Instance.RewardMultiplier));
        ShopButton.GetComponent<Button>().onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene("ShopScene"));

        ShopButton.SetActive(false);
        MatchResultGroup.SetActive(false);
        InitialMatchResult.SetActive(false);

        //winCoroutine = StartCoroutine(DisplayWinScreen());
    }

    void Update()
    {
        PlayerHealth.text = GameManagerScript.Instance.PlayerHealth.ToString();
        PlayerMaxHealth.text = GameManagerScript.Instance.PlayerMaxHealth.ToString();
        EnemyHealth.text = GameManagerScript.Instance.EnemyHealth.ToString();
        EnemyMaxHealth.text = GameManagerScript.Instance.EnemyMaxHealth.ToString();

        if (_gameManager.SelectedDifficulty <= 2)
        {
            BackgroundImage.sprite = LevelBackgrounds[0];
        }
        else if (_gameManager.SelectedDifficulty > 2 && _gameManager.SelectedDifficulty <= 4)
        {
            BackgroundImage.sprite = LevelBackgrounds[1];
        }
        else if (_gameManager.SelectedDifficulty == 5)
        {
            BackgroundImage.sprite = LevelBackgrounds[2];
        }

        UpdateEnemyPoints();
        UpdatePlayerPoints();
    }

    void UpdateEnemyPoints()
    {
        _enemyField = GameObject.Find("EnemyFieldManager").GetComponent<FieldManagerScript>();
        int points = _enemyField.TotalCardValue + GameManagerScript.Instance.AdditionalEnemyPoints;
        EnemyPoint.text = points.ToString();
    }

    void UpdatePlayerPoints()
    {
        _playerField = GameObject.Find("PlayerFieldManager").GetComponent<FieldManagerScript>();
        int points = _playerField.TotalCardValue + GameManagerScript.Instance.AdditionalPlayerPoints;
        PlayerPoint.text = points.ToString();
    }

    public IEnumerator DisplayWinScreen()
    {
        int rewardMultiplier = GameManagerScript.Instance.RewardMultiplier + 1;
        MultiplierText.text = "x " + rewardMultiplier.ToString() + "00%";

        int wageredAmount = GameObject.FindGameObjectWithTag("CookieManager").GetComponent<CookieManagerScript>().WageredCookies;
        int totalReward = wageredAmount * rewardMultiplier;

        Wagered.text = wageredAmount.ToString();
        TotalWinnings.text = totalReward.ToString();

        InitialMatchResult.SetActive(true);
        yield return new WaitForSeconds(3f);
        MatchResultGroup.SetActive(true);

        float timer = 0f;
        while(timer < 1f)
        {
            timer += Time.deltaTime;
            MatchResultGroup.transform.localPosition = Vector3.Lerp(MatchResultGroup.transform.localPosition, new Vector3(0, 0, 0), timer);
            yield return null;
        }
        //initialMatchResult.SetActive(false);
        ShopButton.SetActive(true);

        WinCoroutine = null;
    }

    public IEnumerator FinalBossWinTransition()
    {
        FinalWinGroup.SetActive(true);
        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime;
            FinalWinBackground.color = new Color(0f, 0f, 0f, timer);
            FinalWinTextOne.color = new Color(1f, 1f, 1f, timer);
            FinalWinTextTwo.color = new Color(1f, 1f, 1f, timer);
            yield return null;
        }
        FinalWinTextThree.SetActive(true);
        FinalWinButton.SetActive(true);
        FinalBossWinCoroutine = null;
    }
}
