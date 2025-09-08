using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card cardData;
    public int cardPoints; // You can set this based on your card data
    public bool isPlayable = true; // Default to false, can be set based on game logic
    public bool isCopied = false; // To track if the card is a copy

    [SerializeField] Image cardImage; // Assign in inspector
    public Image highlightImage; // Assign in inspector

    private void Start()
    {
        UpdateCardVisual();

        cardPoints = cardData.cardPoints; // Set the card value based on card data
        isPlayable = cardData.isPlayable; // Set playability based on card data
    }

    private void UpdateCardVisual()
    {
        cardImage.sprite = cardData.cardImage; // Set the card image
        highlightImage.sprite = cardData.cardImage; // Set the highlight image to the same as the card image
    }

    //private void Update()
    //{
    //    if (!isPlayable)
    //    {
    //        highlightImage.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        highlightImage.gameObject.SetActive(false);
    //    }
    //}
}
