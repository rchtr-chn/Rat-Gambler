using UnityEngine;
using UnityEngine.EventSystems;

public class BuyCardScript : MonoBehaviour, IDropHandler
{
    private AudioManagerScript _audioManager;
    public HandManagerScript HandManager;
    public CookieManagerScript CookieManager;
    public ShopCardScript ShopCard;

    private void Start()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManagerScript>();
        HandManager = GameObject.Find("PlayerHandManager").GetComponent<HandManagerScript>();
        CookieManager = GameObject.FindGameObjectWithTag("CookieManager").GetComponent<CookieManagerScript>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerDrag;
        ShopCard = obj.GetComponent<ShopCardScript>();

        if(!CheckRequirements(obj))
        {
            Debug.Log("BuyCardScript: Requirements not met, cannot buy card");
            return;
        }

        CookieManager.PlayerCookies -= ShopCard.Cost;
        _audioManager.PlaySfx(_audioManager.BuyCard);
        GameObject droppedObject = eventData.pointerDrag; // Get the object being dragged
        HandManager.AddCardToHand(droppedObject.GetComponent<CardDisplay>().CardData);
        Destroy(droppedObject); // Destroy the dropped object from shop
        HandManager.UpdateHandPositions();
    }

    bool CheckRequirements(GameObject obj)
    {
        if (HandManager.OnHandCards.Count >= 7)
        {
            Debug.Log("BuyCardScript: Cannot buy more cards, hand limit reached");
            return false;
        }
        if (CookieManager.PlayerCookies <= ShopCard.Cost)
        {
            Debug.Log("BuyCardScript: Cannot buy card, not enough cookies");
            return false;
        }
        if(obj.transform.parent.gameObject.name == "PlayerHand")
        {
            Debug.Log("BuyCardScript: Cannot buy card, card not from shop area");
            return false;
        }

        return true;
    }
}
