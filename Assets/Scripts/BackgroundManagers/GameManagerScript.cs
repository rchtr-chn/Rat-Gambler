using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance { get; private set; }

    [SerializeField] private int _playerHealth;
    [SerializeField] private int _playerMaxHealth;
    [SerializeField] private int _enemyHealth;
    [SerializeField] private int _enemyMaxHealth;
    [SerializeField] private int _difficultyLevel;
    public int DebtAmount = 500;
    public int TurnsLeft = 5;
    public int RewardMultiplier;
    public int SelectedDifficulty = 1; //default difficulty
    public int AdditionalPlayerPoints = 0;
    public int AdditionalEnemyPoints = 0;
    public int PlayerEndTotalPoints = 0;
    public int EnemyEndTotalPoints = 0;

    public bool PlayerOutOfMoves = false;
    public bool EnemyOutOfMoves = false;

    public bool IsPlayerTurn = true;

    private Coroutine _roundCoroutine;

    public OptionsManagerScript OptionsManagerScript { get; private set; }
    public AudioManagerScript AudioManagerScript { get; private set; }
    public DeckManagerScript PlayerDeckManagerScript { get; private set; }
    public DeckManagerScript EnemyDeckManagerScript { get; private set; }
    public UIManagerScript UIManagerScript { get; private set; }
    public CookieManagerScript CookieManagerScript { get; private set; }

    public int PlayerHealth
    {
        get { return _playerHealth; }
        set { _playerHealth = Mathf.Clamp(value, 0, value); } // Ensure health doesn't go below 0
    }
    public int PlayerMaxHealth
    {
        get { return _playerMaxHealth; }
        set { _playerMaxHealth = Mathf.Max(1, value); } // Ensure max health is at least 1
    }

    public int EnemyHealth
    {
        get { return _enemyHealth; }
        set { _enemyHealth = Mathf.Clamp(value, 0, value); } // Ensure health doesn't go below 0
    }
    public int EnemyMaxHealth
    {
        get { return _enemyMaxHealth; }
        set { _enemyMaxHealth = Mathf.Max(1, value); } // Ensure max health is at least 1
    }

    public int DifficultyLevel
    {
        get { return _difficultyLevel; }
        set { _difficultyLevel = Mathf.Clamp(value, 1, 5); } // Clamp difficulty between 1 and 10
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
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
        switch(SelectedDifficulty)
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
        IsPlayerTurn = false;
        NextTurn();
    }

    public void StartPlayerTurn()
    {
        IsPlayerTurn = true;
        NextTurn();
    }

    void NextTurn()
    {
        if (PlayerOutOfMoves && EnemyOutOfMoves)
        {
            //round ends
            EvaluateRound();
            return;
        }

        HandManagerScript hand;
        FieldManagerScript field;

        //sets reference to hand and field for each entity's turn
        if (IsPlayerTurn)
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
        if(hand.OnHandCards.Count == 0)
        {
            // No cards in hand, end turn immediately
            if (IsPlayerTurn)
            {
                PlayerOutOfMoves = true;
                EndPlayerTurn();
            }
            else
            {
                EnemyOutOfMoves = true;
                Debug.Log("Enemy has no cards in hand, ending turn.");
                StartPlayerTurn();
            }
            return;
        }

        //checks for any legal poker cards

        List<Card> legalCards = new List<Card>();

        if (!IsPlayerTurn)
        {
            for (int i = 0; i < hand.OnHandCards.Count; i++)
            {
                Card card = hand.OnHandCards[i].GetComponent<CardDisplay>().CardData;
                if (field.TotalCardValue < 21)
                {
                    legalCards.Add(card);
                }
            }

            if (legalCards.Count > 0)
            {
                // Enemy's turn - decide and play a card
                StartCoroutine(EnemyTurnSequence(hand, field, legalCards));
                //EnemyTurn(hand, field, legalCards);
            }
            else
            {
                // Enemy has no legal plays, ending turn.
                EnemyOutOfMoves = true;
                StartPlayerTurn();
            }
        }
        else
        {
            for (int i=0;i<hand.OnHandCards.Count;i++)
            {
                Card card = hand.OnHandCards[i].GetComponent<CardDisplay>().CardData;
                if (field.TotalCardValue < 21 || card.CardType.Contains(Card.CardTypes.Power))
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
                PlayerOutOfMoves = true;
                Debug.Log("Player has no legal plays, ending turn.");
                EndPlayerTurn();
            }
        }
    }

    IEnumerator EnemyTurnSequence(HandManagerScript hand, FieldManagerScript field, List<Card> allCards)
    {
        yield return new WaitForSeconds(0.5f); // small delay before enemy plays

        EnemyTurn(hand, field, allCards);

        yield return new WaitForSeconds(1.5f); // small delay after enemy plays

        StartPlayerTurn();
    }

    void EnemyTurn(HandManagerScript hand, FieldManagerScript field, List<Card> allCards)
    {
        // Always find the best card
        Card bestCard = allCards.OrderByDescending(c => c.CardPoints).First();

        Card chosenCard;

        // Roll chance to play optimally (higher difficulty = higher chance)
        float optimalChance = _difficultyLevel / 8f; // 0.2 at diff=1, 1.0 at diff=7
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
    }

    // Both players are out of moves, reset for next round
    void EvaluateRound()
    {
        PlayerOutOfMoves = false;
        EnemyOutOfMoves = false;
        Debug.Log("Both players out of moves, evaluating round.");

        int playerTotal = GameObject.Find("PlayerFieldManager").GetComponent<FieldManagerScript>().TotalCardValue + AdditionalPlayerPoints;
        int enemyTotal = GameObject.Find("EnemyFieldManager").GetComponent<FieldManagerScript>().TotalCardValue + AdditionalEnemyPoints;

        //calculate health penalties
        int penalty = 0;
        if(enemyTotal > 21 && playerTotal > 21)
        {
            if(enemyTotal > playerTotal)
            {
                penalty = enemyTotal - 21;
                _enemyHealth -= penalty;
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
                _playerHealth -= penalty;
            }
            else
            {
                int diff = (playerTotal - enemyTotal);
                _enemyHealth -= diff;
            }
        }
        else if (enemyTotal > playerTotal)
        {
            if(enemyTotal > 21)
            {
                penalty = enemyTotal - 21;
                _enemyHealth -= penalty;
            }
            else
            {
                int diff = (enemyTotal - playerTotal);
                PlayerHealth -= diff;
            }
        }

        //clamp health to not go below 0
        if (_enemyHealth < 0)
        {
            _enemyHealth = 0;
        }
        if (_playerHealth < 0)
        {
            _playerHealth = 0;
        }

        //reset additional points
        AdditionalPlayerPoints = 0;
        AdditionalEnemyPoints = 0;

        AudioManagerScript.PlaySfx(AudioManagerScript.ShuffleDeck);

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
            AudioManagerScript.MusicSource.Stop();
            AudioManagerScript.PlaySfx(AudioManagerScript.LoseSound);
            SceneManager.LoadScene("GameOverScene");
            return;
        }
        else if (EnemyHealth <= 0)
        {
            if(SelectedDifficulty < 5)
            {
                AudioManagerScript.MusicSource.Stop();
                AudioManagerScript.PlaySfx(AudioManagerScript.WinSound);
                UIManagerScript.WinCoroutine = StartCoroutine(UIManagerScript.DisplayWinScreen());
                TutorialManagerScript tutorial = FindObjectOfType<TutorialManagerScript>();
                TurnsLeft -= 1;
                if (tutorial != null && tutorial.IsTutorialActive)
                {
                    tutorial.IsTutorialActive = false;
                    tutorial.TutorialUIText.SetActive(false);
                }
                // display shop for deck update etc
                // make player choose between difficulty (1-3-5)
                return;
            }
            else
            {
                // Final boss defeated, trigger special ending
                UIManagerScript.FinalBossWinCoroutine = StartCoroutine(UIManagerScript.FinalBossWinTransition());
                return;
            }
        }

        

        _roundCoroutine = StartCoroutine(ContinueRound());
    }

    IEnumerator ContinueRound()
    {
        yield return new WaitForSeconds(1f); // small delay before next round

        PlayerDeckManagerScript.CurrentHandSize = 0;
        EnemyDeckManagerScript.CurrentHandSize = 0;
        PlayerDeckManagerScript.HandInitializationCoroutine = StartCoroutine(PlayerDeckManagerScript.InitializeHand());
        EnemyDeckManagerScript.HandInitializationCoroutine = StartCoroutine(EnemyDeckManagerScript.InitializeHand());

        if(PlayerOutOfMoves)
        {
            PlayerOutOfMoves = false;
        }

        StartPlayerTurn();
        _roundCoroutine = null;
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------------


    void PlayDifficultyLevelOne()
    {
        _playerHealth = _playerMaxHealth = 30;
        _enemyHealth = _enemyMaxHealth = 15;
        _difficultyLevel = 1;
        RewardMultiplier = 1;
    }

    void PlayDifficultyLevelTwo()
    {
        _playerHealth = _playerMaxHealth = 30;
        _enemyHealth = _enemyMaxHealth = 20;
        _difficultyLevel = 2;
        RewardMultiplier = 1;

        PlayerDeckManagerScript.HandInitializationCoroutine = StartCoroutine(PlayerDeckManagerScript.InitializeHand());
        EnemyDeckManagerScript.HandInitializationCoroutine = StartCoroutine(EnemyDeckManagerScript.InitializeHand());

        StartPlayerTurn();
    }

    void PlayDifficultyLevelThree()
    {
        _playerHealth = _playerMaxHealth = 30;
        _enemyHealth = _enemyMaxHealth = 25;
        _difficultyLevel = 3;
        RewardMultiplier = 2;

        PlayerDeckManagerScript.HandInitializationCoroutine = StartCoroutine(PlayerDeckManagerScript.InitializeHand());
        EnemyDeckManagerScript.HandInitializationCoroutine = StartCoroutine(EnemyDeckManagerScript.InitializeHand());

        StartPlayerTurn();
    }

    void PlayDifficultyLevelFour()
    {
        _playerHealth = _playerMaxHealth = 30;
        _enemyHealth = _enemyMaxHealth = 30;
        _difficultyLevel = 3;

        PlayerDeckManagerScript.HandInitializationCoroutine = StartCoroutine(PlayerDeckManagerScript.InitializeHand());
        EnemyDeckManagerScript.HandInitializationCoroutine = StartCoroutine(EnemyDeckManagerScript.InitializeHand());

        StartPlayerTurn();
    }

    void PlayDifficultyLevelFive()
    {
        _playerHealth = _playerMaxHealth = 30;
        _enemyHealth = _enemyMaxHealth = 35;
        _difficultyLevel = 5;
        RewardMultiplier = 4;

        PlayerDeckManagerScript.HandInitializationCoroutine = StartCoroutine(PlayerDeckManagerScript.InitializeHand());
        EnemyDeckManagerScript.HandInitializationCoroutine = StartCoroutine(EnemyDeckManagerScript.InitializeHand());

        StartPlayerTurn();
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------------

    public void OpenShop()
    {
        CookieManagerScript.ClaimRewards(RewardMultiplier);

        if(UIManagerScript.WinCoroutine != null)
        {
            StopCoroutine(UIManagerScript.WinCoroutine);
            UIManagerScript.WinCoroutine = null;
        }

        SceneManager.LoadScene("ShopScene");
    }
}
