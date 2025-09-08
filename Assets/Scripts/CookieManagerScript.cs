using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieManagerScript : MonoBehaviour
{
    public GameObject buttonText;
    GameManagerScript gameManager;
    AudioManagerScript audioManager;
    public int playerCookies = 10;
    public int wageredCookies = 0;
    public int wagerMinimum = 10;
    public GameObject cookieGroup;
    public GameObject cookieParent;
    public GameObject cookiePrefab;
    public GameObject confirmWagerButton;
    public List<GameObject> cookies = new List<GameObject>();
    public bool betPlaced = false;


    private void Start()
    {
        gameManager = FindObjectOfType<GameManagerScript>();
        audioManager = FindObjectOfType<AudioManagerScript>();
    }
    private void Update()
    {
        switch(gameManager.selectedDifficulty)
        {
            case 1:
                wagerMinimum = 10;
                break;
            case 2:
                wagerMinimum = 10;
                break;
            case 3:
                wagerMinimum = 50;
                break;
            case 4:
                wagerMinimum = 100;
                break;
            case 5:
                wagerMinimum = 300;
                break;
        }
    }

    public void IntializeCookieWagerMechanic()
    {
        cookieGroup = GameObject.Find("Cookie-Group");
        cookieParent = GameObject.Find("CookieParent");
        confirmWagerButton = GameObject.Find("ConfirmWagerButton");
        buttonText = GameObject.Find("ConfirmWagerButtonText");

        buttonText.GetComponent<UnityEngine.UI.Text>().text = "All-In";
        confirmWagerButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(AllIn);
        confirmWagerButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => audioManager.PlaySfx(audioManager.buttonPress));
        confirmWagerButton.SetActive(true);

        PlaceBets();
    }

    public void AllIn()
    {
        wageredCookies = playerCookies;
        playerCookies = 0;
        ConfirmWager();
        confirmWagerButton.SetActive(false);
    }

    public void AddWageredCookies(GameObject obj)
    {
        wageredCookies += 10;
        playerCookies -= 10;
        cookies.Remove(obj);

        buttonText.GetComponent<UnityEngine.UI.Text>().text = "Confirm Wager";
        confirmWagerButton.GetComponent<UnityEngine.UI.Button>().onClick.RemoveListener(AllIn);
        confirmWagerButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ConfirmWager);

        if(wageredCookies >= wagerMinimum)
        {
            confirmWagerButton.SetActive(true);
        }
        else
        {
            confirmWagerButton.SetActive(false);
        }
    }

    public void ConfirmWager()
    {
        cookieGroup.SetActive(false);
        confirmWagerButton.SetActive(false);
        int index = 0;
        while (index < cookies.Count)
        {
            Destroy(cookies[index]);
            index++;
        }
        cookies.Clear();
        betPlaced = true;

        gameManager.StartGame();
    }

    public void ClaimRewards(int mult)
    {
        int reward = (mult * wageredCookies) + wageredCookies;
        playerCookies += reward;
        wageredCookies = 0;
        betPlaced = false;
    }

    public void PlaceBets()
    {
        int index = 10;
        while (index <= playerCookies)
        {
            float xRand = Random.Range(-100f, 100f);
            float yRand = Random.Range(-100f, 100f);
            GameObject obj = Instantiate(cookiePrefab, cookieParent.transform.position + new Vector3(xRand, yRand, 0), Quaternion.identity, cookieParent.transform);
            cookies.Add(obj);
            index += 10;
        }
    }
}
