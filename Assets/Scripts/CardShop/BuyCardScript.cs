using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyCardScript : MonoBehaviour, IDropHandler
{
    AudioManagerScript audioManager;
    public HandManagerScript handManager;
    public CookieManagerScript cookieManager;
    public ShopCardScript shopCard;

    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManagerScript>();
        handManager = GameObject.Find("PlayerHandManager").GetComponent<HandManagerScript>();
        cookieManager = GameObject.FindGameObjectWithTag("CookieManager").GetComponent<CookieManagerScript>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerDrag;
        shopCard = obj.GetComponent<ShopCardScript>();

        if(!CheckRequirements(obj))
        {
            Debug.Log("BuyCardScript: Requirements not met, cannot buy card");
            return;
        }

        cookieManager.playerCookies -= shopCard.cost;
        audioManager.PlaySfx(audioManager.buyCard);
        GameObject droppedObject = eventData.pointerDrag; // Get the object being dragged
        handManager.AddCardToHand(droppedObject.GetComponent<CardDisplay>().cardData);
        Destroy(droppedObject); // Destroy the dropped object from shop
        handManager.UpdateHandPositions();
    }

    bool CheckRequirements(GameObject obj)
    {
        if (handManager.onHandCards.Count >= 7)
        {
            Debug.Log("BuyCardScript: Cannot buy more cards, hand limit reached");
            return false;
        }
        if (cookieManager.playerCookies <= shopCard.cost)
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
