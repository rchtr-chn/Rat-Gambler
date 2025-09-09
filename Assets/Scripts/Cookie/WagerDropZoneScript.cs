using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WagerDropZoneScript : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag; // Get the object being dragged

        if (droppedObject != null && droppedObject.CompareTag("Cookie")) // Check if the dropped object has the "Wager" tag
        {
            // Set the parent of the dropped object to this drop zone
            droppedObject.transform.SetParent(transform);
            // Optionally, reset the position of the dropped object to the center of the drop zone
            RectTransform dropZoneRect = GetComponent<RectTransform>();
            RectTransform droppedRect = droppedObject.GetComponent<RectTransform>();
            if (dropZoneRect != null && droppedRect != null)
            {
                float xRand = Random.Range(-25f, 25f);
                float yRand = Random.Range(-25f, 25f);

                droppedRect.anchoredPosition = new Vector2(xRand, yRand); // Center the dropped object in the drop zone

                droppedObject.GetComponent<DragUIObject>().enabled = false; // Disable dragging for the dropped object
                droppedObject.GetComponent<CanvasGroup>().blocksRaycasts = false; // Prevent further raycast blocking
                droppedObject.GetComponent<CanvasGroup>().alpha = 1.0f; // Ensure the object is fully opaque

                FindObjectOfType<CookieManagerScript>().AddWageredCookies(droppedObject); // Increment the wagered cookies count
            }
        }
    }
}
