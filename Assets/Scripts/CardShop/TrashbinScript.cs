using UnityEngine;
using UnityEngine.EventSystems;

public class TrashbinScript : MonoBehaviour, IDropHandler
{
    private AudioManagerScript _audioManager;
    private HandManagerScript _handManager;

    private void Start()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManagerScript>();
        _handManager = GameObject.Find("PlayerHandManager").GetComponent<HandManagerScript>();
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
            _audioManager.PlaySfx(_audioManager.TrashCard);
            Debug.Log("TrashbinScript: Dropped object is from PlayerHand");
            _handManager.OnHandCards.Remove(droppedObject); // Remove the card from the player's hand
            Destroy(droppedObject); // Destroy the dropped object

            _handManager.UpdateHandPositions(); // Update the positions of the remaining cards in hand
        }
    }

    bool CheckForMinimalDeckRule(GameObject target)
    {
        int index = 0;
        for (int i = 0; i < _handManager.OnHandCards.Count - 1; i++)
        {
            if (_handManager.OnHandCards[i].GetComponent<CardDisplay>().CardData.CardType.Contains(Card.CardTypes.Poker))
            {
                index++;
            }
        }

        if(index > 4 || target.GetComponent<CardDisplay>().CardData.CardType.Contains(Card.CardTypes.Power))
        {
            return true;
        }
        return false;
    }
}
