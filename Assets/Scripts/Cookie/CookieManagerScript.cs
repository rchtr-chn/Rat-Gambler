using System.Collections.Generic;
using UnityEngine;

public class CookieManagerScript : MonoBehaviour
{
    public GameObject ButtonText;
    private GameManagerScript _gameManager;
    private AudioManagerScript _audioManager;
    public int PlayerCookies = 10;
    public int WageredCookies = 0;
    public int WagerMinimum = 10;
    public GameObject CookieGroup;
    public GameObject CookieParent;
    public GameObject CookiePrefab;
    public GameObject ConfirmWagerButton;
    public List<GameObject> Cookies = new List<GameObject>();
    public bool BetPlaced = false;


    private void Start()
    {
        _gameManager = FindObjectOfType<GameManagerScript>();
        _audioManager = FindObjectOfType<AudioManagerScript>();
    }
    private void Update()
    {
        switch(_gameManager.SelectedDifficulty)
        {
            case 1:
                WagerMinimum = 10;
                break;
            case 2:
                WagerMinimum = 10;
                break;
            case 3:
                WagerMinimum = 50;
                break;
            case 4:
                WagerMinimum = 100;
                break;
            case 5:
                WagerMinimum = 300;
                break;
        }
    }

    public void IntializeCookieWagerMechanic()
    {
        CookieGroup = GameObject.Find("Cookie-Group");
        CookieParent = GameObject.Find("CookieParent");
        ConfirmWagerButton = GameObject.Find("ConfirmWagerButton");
        ButtonText = GameObject.Find("ConfirmWagerButtonText");

        ButtonText.GetComponent<UnityEngine.UI.Text>().text = "All-In";
        ConfirmWagerButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(AllIn);
        ConfirmWagerButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => _audioManager.PlaySfx(_audioManager.ButtonPress));
        ConfirmWagerButton.SetActive(true);

        PlaceBets();
    }

    public void AllIn()
    {
        WageredCookies = PlayerCookies;
        PlayerCookies = 0;
        ConfirmWager();
        ConfirmWagerButton.SetActive(false);
    }

    public void AddWageredCookies(GameObject obj)
    {
        WageredCookies += 10;
        PlayerCookies -= 10;
        Cookies.Remove(obj);

        ButtonText.GetComponent<UnityEngine.UI.Text>().text = "Confirm Wager";
        ConfirmWagerButton.GetComponent<UnityEngine.UI.Button>().onClick.RemoveListener(AllIn);
        ConfirmWagerButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ConfirmWager);

        if(WageredCookies >= WagerMinimum)
        {
            ConfirmWagerButton.SetActive(true);
        }
        else
        {
            ConfirmWagerButton.SetActive(false);
        }
    }

    public void ConfirmWager()
    {
        CookieGroup.SetActive(false);
        ConfirmWagerButton.SetActive(false);
        int index = 0;
        while (index < Cookies.Count)
        {
            Destroy(Cookies[index]);
            index++;
        }
        Cookies.Clear();
        BetPlaced = true;

        _gameManager.StartGame();
    }

    public void ClaimRewards(int mult)
    {
        int reward = (mult * WageredCookies) + WageredCookies;
        PlayerCookies += reward;
        WageredCookies = 0;
        BetPlaced = false;
    }

    public void PlaceBets()
    {
        int index = 10;
        while (index <= PlayerCookies)
        {
            float xRand = Random.Range(-100f, 100f);
            float yRand = Random.Range(-100f, 100f);
            GameObject obj = Instantiate(CookiePrefab, CookieParent.transform.position + new Vector3(xRand, yRand, 0), Quaternion.identity, CookieParent.transform);
            Cookies.Add(obj);
            index += 10;
        }
    }
}
