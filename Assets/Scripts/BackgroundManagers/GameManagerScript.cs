using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance { get; private set; }

    [SerializeField] private int playerHealth;
    [SerializeField] private int playerMaxHealth;
    [SerializeField] private int enemyHealth;
    [SerializeField] private int enemyMaxHealth;
    [SerializeField] private int difficultyLevel;
    public int debtAmount = 500;
    public int turnsLeft = 5;
    public int rewardMultiplier;
    public int selectedDifficulty = 1; //default difficulty
    public int additionalPlayerPoints = 0;
    public int additionalEnemyPoints = 0;
    public int playerEndTotalPoints = 0;
    public int enemyEndTotalPoints = 0;

    public bool isPlayerTurn;

    public bool playerOutOfMoves = false;
    public bool enemyOutOfMoves = false;

    Coroutine roundCoroutine;

    public OptionsManagerScript OptionsManagerScript { get; private set; }
    public AudioManagerScript AudioManagerScript { get; private set; }
    public DeckManagerScript PlayerDeckManagerScript { get; private set; }
    public DeckManagerScript EnemyDeckManagerScript { get; private set; }
    public UIManagerScript UIManagerScript { get; private set; }
    public CookieManagerScript CookieManagerScript { get; private set; }


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
            InitializeManagers();
        }
    }

    public void InitializeManagers()
    {
        OptionsManagerScript = FindObjectOfType<OptionsManagerScript>();
        AudioManagerScript = FindObjectOfType<AudioManagerScript>();

        GameObject obj = GameObject.FindGameObjectWithTag("PlayerDeckManager");
        if (obj != null)
        {
            PlayerDeckManagerScript = obj.GetComponent<DeckManagerScript>();
        }

        obj = GameObject.FindGameObjectWithTag("EnemyDeckManager");
        if (obj != null)
        {
            EnemyDeckManagerScript = obj.GetComponent<DeckManagerScript>();
        }

        if (OptionsManagerScript == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Managers/OptionsManager");
            if (prefab == null)
            {
                Debug.LogError("OptionsManager prefab not found in Resources/Prefabs");
            }
            else
            {
                Instantiate(prefab, transform.position, Quaternion.identity, transform);
                OptionsManagerScript = GetComponentInChildren<OptionsManagerScript>();
            }
        }

        if (AudioManagerScript == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Managers/AudioManager");
            if (prefab == null)
            {
                Debug.LogError("AudioManager prefab not found in Resources/Prefabs");
            }
            else
            {
                Instantiate(prefab, transform.position, Quaternion.identity, transform);
                AudioManagerScript = GetComponentInChildren<AudioManagerScript>();
            }
        }
    }

    public int PlayerHealth
    {
        get { return playerHealth; }
        set { playerHealth = Mathf.Clamp(value, 0, value); } // Ensure health doesn't go below 0
    }
    public int PlayerMaxHealth
    {
        get { return playerMaxHealth; }
        set { playerMaxHealth = Mathf.Max(1, value); } // Ensure max health is at least 1
    }

    public int EnemyHealth
    {
        get { return enemyHealth; }
        set { enemyHealth = Mathf.Clamp(value, 0, value); } // Ensure health doesn't go below 0
    }
    public int EnemyMaxHealth
    {
        get { return enemyMaxHealth; }
        set { enemyMaxHealth = Mathf.Max(1, value); } // Ensure max health is at least 1
    }

    public int DifficultyLevel
    {
        get { return difficultyLevel; }
        set { difficultyLevel = Mathf.Clamp(value, 1, 5); } // Clamp difficulty between 1 and 10
    }

    public bool IsPlayerTurn
    {
        get { return isPlayerTurn; }
        set { isPlayerTurn = value; }
    }

    private void Update()
    {

    }

    public void InitializeGameplayManagers()
    {
        UIManagerScript = FindObjectOfType<UIManagerScript>();

        if (PlayerDeckManagerScript == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Managers/Player-DeckManager");
            if (prefab == null)
            {
                Debug.LogError("DeckManager prefab not found in Resources/Prefabs");
            }
            else
            {
                GameObject instance = Instantiate(prefab, transform.position, Quaternion.identity, transform);
                PlayerDeckManagerScript = instance.GetComponent<DeckManagerScript>();
            }
        }
        if (EnemyDeckManagerScript == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Managers/Enemy-DeckManager");
            if (prefab == null)
            {
                Debug.LogError("DeckManager prefab not found in Resources/Prefabs");
            }
            else
            {
                GameObject instance = Instantiate(prefab, transform.position, Quaternion.identity, transform);
                EnemyDeckManagerScript = instance.GetComponent<DeckManagerScript>();
            }
        }
        if (CookieManagerScript == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Managers/CookieManager");
            if (prefab == null)
            {
                Debug.LogError("CookieManager prefab not found in Resources/Prefabs");
            }
            else
            {
                Instantiate(prefab, transform.position, Quaternion.identity, transform);
                CookieManagerScript = GetComponentInChildren<CookieManagerScript>();
            }
        }
    }

    public void StartGame()
    {
        switch(selectedDifficulty)
        {
            case 1:
                PlayDifficultyLevelOne();
                break;
            case 2:
                PlayDifficultyLevelTwo();
                break;
            case 3:
                PlayDifficultyLevelThree();
                break;
            case 4:
                PlayDifficultyLevelFour();
                break;
            case 5:
                PlayDifficultyLevelFive();
                break;
        }
    }

    public void EndPlayerTurn()
    {
        isPlayerTurn = false;
        NextTurn();
    }

    public void StartPlayerTurn()
    {
        isPlayerTurn = true;
        NextTurn();
    }

    void NextTurn()
    {
        if (playerOutOfMoves && enemyOutOfMoves)
        {
            //round ends
            EvaluateRound();
            return;
        }

        HandManagerScript hand;
        FieldManagerScript field;

        //sets reference to hand and field for each entity's turn
        if (isPlayerTurn)
        {
            hand = GameObject.Find("PlayerHandManager").GetComponent<HandManagerScript>();
            field = GameObject.Find("PlayerFieldManager").GetComponent<FieldManagerScript>();
        }
        else
        {
            hand = GameObject.Find("EnemyHandManager").GetComponent<HandManagerScript>();
            field = GameObject.Find("EnemyFieldManager").GetComponent<FieldManagerScript>();
        }


        
        StartCoroutine(HandleTurn(hand, field));
    }

    IEnumerator HandleTurn(HandManagerScript hand, FieldManagerScript field)
    {
        yield return new WaitForSeconds(1f); // small delay so it doesn’t instantly loop

        CheckForLegalPlay(hand, field);
    }

    void CheckForLegalPlay(HandManagerScript hand, FieldManagerScript field)
    {
        if(hand.onHandCards.Count == 0)
        {
            // No cards in hand, end turn immediately
            if (isPlayerTurn)
            {
                playerOutOfMoves = true;
                EndPlayerTurn();
            }
            else
            {
                enemyOutOfMoves = true;
                Debug.Log("Enemy has no cards in hand, ending turn.");
                StartPlayerTurn();
            }
            return;
        }

        //checks for any legal poker cards

        List<Card> legalCards = new List<Card>();

        if (!isPlayerTurn)
        {
            for (int i = 0; i < hand.onHandCards.Count; i++)
            {
                Card card = hand.onHandCards[i].GetComponent<CardDisplay>().cardData;
                if (field.totalCardValue < 21)
                {
                    legalCards.Add(card);
                }
            }

            if (legalCards.Count > 0)
            {
                // Enemy's turn - decide and play a card
                EnemyTurn(hand, field, legalCards);
            }
            else
            {
                // Enemy has no legal plays, ending turn.
                enemyOutOfMoves = true;
                StartPlayerTurn();
            }
        }
        else
        {
            for (int i=0;i<hand.onHandCards.Count;i++)
            {
                Card card = hand.onHandCards[i].GetComponent<CardDisplay>().cardData;
                if (field.totalCardValue < 21 || card.cardType.Contains(Card.CardType.Power))
                {
                    legalCards.Add(card);
                }
            }

            if (legalCards.Count > 0)
            {
                // Player has legal plays, allow them to continue.
                return;
            }
            else
            {
                // Player has no legal plays, ending turn.
                playerOutOfMoves = true;
                Debug.Log("Player has no legal plays, ending turn.");
                EndPlayerTurn();
            }
        }
    }

    void EnemyTurn(HandManagerScript hand, FieldManagerScript field, List<Card> allCards)
    {
        // Always find the best card
        Card bestCard = allCards.OrderByDescending(c => c.cardPoints).First();

        Card chosenCard;

        // Roll chance to play optimally (higher difficulty = higher chance)
        float optimalChance = difficultyLevel / 8f; // 0.2 at diff=1, 1.0 at diff=7
        if (Random.value <= optimalChance)
        {
            // Pick the best card
            chosenCard = bestCard;
        }
        else
        {
            // Pick a random non-best card (or fallback to best if no other option)
            List<Card> weakerOptions = allCards.Where(c => c != bestCard).ToList();
            chosenCard = weakerOptions.Count > 0 ? weakerOptions[Random.Range(0, weakerOptions.Count)] : bestCard;
        }

        // Now you can actually play chosenCard here
        hand.RemoveCardFromHand(chosenCard);
        field.AddCardToField(chosenCard);

        StartPlayerTurn();
    }

    // Both players are out of moves, reset for next round
    void EvaluateRound()
    {
        playerOutOfMoves = false;
        enemyOutOfMoves = false;
        Debug.Log("Both players out of moves, evaluating round.");

        int playerTotal = GameObject.Find("PlayerFieldManager").GetComponent<FieldManagerScript>().totalCardValue + additionalPlayerPoints;
        int enemyTotal = GameObject.Find("EnemyFieldManager").GetComponent<FieldManagerScript>().totalCardValue + additionalEnemyPoints;

        //calculate health penalties
        int penalty = 0;
        if(enemyTotal > 21 && playerTotal > 21)
        {
            if(enemyTotal > playerTotal)
            {
                penalty = enemyTotal - 21;
                enemyHealth -= penalty;
            }
            else if (playerTotal > enemyTotal)
            {
                penalty = playerTotal - 21;
                PlayerHealth -= penalty;
            }
        }
        else if (playerTotal > enemyTotal)
        {
            if(playerTotal > 21)
            {
                penalty = playerTotal - 21;
                playerHealth -= penalty;
            }
            else
            {
                int diff = (playerTotal - enemyTotal);
                enemyHealth -= diff;
            }
        }
        else if (enemyTotal > playerTotal)
        {
            if(enemyTotal > 21)
            {
                penalty = enemyTotal - 21;
                enemyHealth -= penalty;
            }
            else
            {
                int diff = (enemyTotal - playerTotal);
                PlayerHealth -= diff;
            }
        }

        //clamp health to not go below 0
        if (enemyHealth < 0)
        {
            enemyHealth = 0;
        }
        if (playerHealth < 0)
        {
            playerHealth = 0;
        }

        //reset additional points
        additionalPlayerPoints = 0;
        additionalEnemyPoints = 0;

        AudioManagerScript.PlaySfx(AudioManagerScript.shuffleDeck);

        //return all drawn cards back to deck
        foreach (HandManagerScript h in FindObjectsOfType<HandManagerScript>())
        {
            h.ReturnAllCardsToDeck();
        }
        foreach (FieldManagerScript f in FindObjectsOfType<FieldManagerScript>())
        {
            f.ReturnAllCardsToDeck();
        }

        // Check for end game
        if (PlayerHealth <= 0)
        {
            AudioManagerScript.musicSource.Stop();
            AudioManagerScript.PlaySfx(AudioManagerScript.loseSound);
            SceneManager.LoadScene("GameOverScene");
            return;
        }
        else if (EnemyHealth <= 0)
        {
            if(selectedDifficulty < 5)
            {
                AudioManagerScript.musicSource.Stop();
                AudioManagerScript.PlaySfx(AudioManagerScript.winSound);
                UIManagerScript.winCoroutine = StartCoroutine(UIManagerScript.DisplayWinScreen());
                TutorialManagerScript tutorial = FindObjectOfType<TutorialManagerScript>();
                turnsLeft -= 1;
                if (tutorial != null && tutorial.isTutorialActive)
                {
                    tutorial.isTutorialActive = false;
                    tutorial.tutorialUIText.SetActive(false);
                }
                // display shop for deck update etc
                // make player choose between difficulty (1-3-5)
                return;
            }
            else
            {
                // Final boss defeated, trigger special ending
                UIManagerScript.finalBossWinCoroutine = StartCoroutine(UIManagerScript.FinalBossWinTransition());
                return;
            }
        }

        

        roundCoroutine = StartCoroutine(ContinueRound());
    }

    IEnumerator ContinueRound()
    {
        yield return new WaitForSeconds(1f); // small delay before next round

        PlayerDeckManagerScript.currentHandSize = 0;
        EnemyDeckManagerScript.currentHandSize = 0;
        PlayerDeckManagerScript.handInitializationCoroutine = StartCoroutine(PlayerDeckManagerScript.InitializeHand());
        EnemyDeckManagerScript.handInitializationCoroutine = StartCoroutine(EnemyDeckManagerScript.InitializeHand());

        if(playerOutOfMoves)
        {
            playerOutOfMoves = false;
        }

        StartPlayerTurn();
        roundCoroutine = null;
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------------


    void PlayDifficultyLevelOne()
    {
        playerHealth = playerMaxHealth = 30;
        enemyHealth = enemyMaxHealth = 15;
        difficultyLevel = 1;
        rewardMultiplier = 1;
    }

    void PlayDifficultyLevelTwo()
    {
        playerHealth = playerMaxHealth = 30;
        enemyHealth = enemyMaxHealth = 20;
        difficultyLevel = 2;
        rewardMultiplier = 1;

        PlayerDeckManagerScript.handInitializationCoroutine = StartCoroutine(PlayerDeckManagerScript.InitializeHand());
        EnemyDeckManagerScript.handInitializationCoroutine = StartCoroutine(EnemyDeckManagerScript.InitializeHand());

        StartPlayerTurn();
    }

    void PlayDifficultyLevelThree()
    {
        playerHealth = playerMaxHealth = 30;
        enemyHealth = enemyMaxHealth = 25;
        difficultyLevel = 3;
        rewardMultiplier = 2;

        PlayerDeckManagerScript.handInitializationCoroutine = StartCoroutine(PlayerDeckManagerScript.InitializeHand());
        EnemyDeckManagerScript.handInitializationCoroutine = StartCoroutine(EnemyDeckManagerScript.InitializeHand());

        StartPlayerTurn();
    }

    void PlayDifficultyLevelFour()
    {
        playerHealth = playerMaxHealth = 30;
        enemyHealth = enemyMaxHealth = 30;
        difficultyLevel = 3;

        PlayerDeckManagerScript.handInitializationCoroutine = StartCoroutine(PlayerDeckManagerScript.InitializeHand());
        EnemyDeckManagerScript.handInitializationCoroutine = StartCoroutine(EnemyDeckManagerScript.InitializeHand());

        StartPlayerTurn();
    }

    void PlayDifficultyLevelFive()
    {
        playerHealth = playerMaxHealth = 30;
        enemyHealth = enemyMaxHealth = 35;
        difficultyLevel = 5;
        rewardMultiplier = 4;

        PlayerDeckManagerScript.handInitializationCoroutine = StartCoroutine(PlayerDeckManagerScript.InitializeHand());
        EnemyDeckManagerScript.handInitializationCoroutine = StartCoroutine(EnemyDeckManagerScript.InitializeHand());

        StartPlayerTurn();
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------------

    public void OpenShop()
    {
        CookieManagerScript.ClaimRewards(rewardMultiplier);

        if(UIManagerScript.winCoroutine != null)
        {
            StopCoroutine(UIManagerScript.winCoroutine);
            UIManagerScript.winCoroutine = null;
        }

        SceneManager.LoadScene("ShopScene");
    }
}
