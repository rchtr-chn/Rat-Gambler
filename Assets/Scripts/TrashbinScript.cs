using UnityEngine;
using UnityEngine.EventSystems;

public class TrashbinScript : MonoBehaviour, IDropHandler
{
    AudioManagerScript audioManager;
    HandManagerScript handManager;

    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManagerScript>();
        handManager = GameObject.Find("PlayerHandManager").GetComponent<HandManagerScript>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerDrag;
        if (!CheckForMinimalDeckRule(obj))
        {
            Debug.Log("TrashbinScript: Cannot discard card, minimal deck rule not met");
            return;
        }

        GameObject droppedObject = eventData.pointerDrag; // Get the object being dragged
        Debug.Log("Dropped object: " + droppedObject);
        if (droppedObject != null && droppedObject.transform.parent.gameObject.name == "PlayerHand") // Check if the dropped object has the "Card" tag
        {
            audioManager.PlaySfx(audioManager.trashCard);
            Debug.Log("TrashbinScript: Dropped object is from PlayerHand");
            handManager.onHandCards.Remove(droppedObject); // Remove the card from the player's hand
            Destroy(droppedObject); // Destroy the dropped object

            handManager.UpdateHandPositions(); // Update the positions of the remaining cards in hand
        }
    }

    bool CheckForMinimalDeckRule(GameObject target)
    {
        int index = 0;
        for (int i = 0; i < handManager.onHandCards.Count - 1; i++)
        {
            if (handManager.onHandCards[i].GetComponent<CardDisplay>().cardData.cardType.Contains(Card.CardType.Poker))
            {
                index++;
            }
        }

        if(index > 4 || target.GetComponent<CardDisplay>().cardData.cardType.Contains(Card.CardType.Power))
        {
            return true;
        }
        return false;
    }
}
