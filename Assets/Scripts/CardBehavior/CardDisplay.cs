using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card CardData;
    public int CardPoints;
    public bool IsPlayable = true;
    public bool IsCopied = false;

    [SerializeField] private Image _cardImage;
    public Image HighlightImage;

    private void Start()
    {
        UpdateCardVisual();

        CardPoints = CardData.CardPoints;
        IsPlayable = CardData.IsPlayable;
    }

    private void UpdateCardVisual()
    {
        _cardImage.sprite = CardData.CardImage;
        HighlightImage.sprite = CardData.CardImage;
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
