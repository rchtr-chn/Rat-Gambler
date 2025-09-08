using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CardMovementScript : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    
    public AudioManagerScript audioManager;

    private Vector2 originalLocalPointerPos;
    private Vector3 originalPanelLocalPos;
    private Vector3 originalScale;

    private int currentState = 0;

    private Quaternion originalRotation;
    private Vector3 originalPosition;

    [SerializeField] private float selectScale = 1.2f;
    public Vector2 cardPlay;
    [SerializeField] private Vector3 playPos;
    [SerializeField] private GameObject highlightEffect;
    [SerializeField] private GameObject playArrow;
    [SerializeField] private float lerpFactor = 0.1f;

    FieldManagerScript fieldManager;
    HandManagerScript handManager;
    GameManagerScript gameManager;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManagerScript>();

        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        originalScale = rectTransform.localScale;
        originalRotation = rectTransform.localRotation;
        originalPosition = rectTransform.localPosition;


        fieldManager = GameObject.Find("PlayerFieldManager").GetComponent<FieldManagerScript>();
        handManager = GameObject.Find("PlayerHandManager").GetComponent<HandManagerScript>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case 1:
                HandleHoverState();
                break;

            case 2:
                HandleDragState();
                if(!Input.GetMouseButton(0)) // If mouse button is released
                {
                    TransitionToState0();
                }
                break;

            case 3:
                if (gameManager.isPlayerTurn)
                {
                    HandlePlayState();
                }
                if (!Input.GetMouseButton(0)) // If mouse button is released
                {
                    TransitionToState0();
                }
                break;
        }
    }

    private void TransitionToState0()
    {
        currentState = 0;
        rectTransform.localScale = originalScale;
        rectTransform.localRotation = originalRotation;
        rectTransform.localPosition = originalPosition;
        //highlightEffect.SetActive(false);
        playArrow.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentState == 0)
        {
            originalPosition = rectTransform.localPosition;
            originalRotation = rectTransform.localRotation;
            originalScale = rectTransform.localScale;

            audioManager.PlaySfx(audioManager.hoverCard);

            currentState = 1; // Hover state
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentState == 1)
        {
            TransitionToState0();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentState == 1 && gameManager.IsPlayerTurn)
        {
            currentState = 2; // Drag state

            RectTransformUtility.ScreenPointToLocalPointInRectangle
                (canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out originalLocalPointerPos);

            originalPanelLocalPos = rectTransform.localPosition;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentState == 2)
        {
            canvasGroup.blocksRaycasts = false; //Disable raycast blocking so that other UI elements can receive raycasts while dragging
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(currentState == 2)
        {
            Vector2 localPointerPos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localPointerPos))
            {
                rectTransform.position = Vector3.Lerp(rectTransform.position, Input.mousePosition, lerpFactor);

                if (rectTransform.localPosition.y > cardPlay.y)
                {
                    currentState = 3; // Play state
                    playArrow.SetActive(true);
                    rectTransform.localPosition = Vector3.Lerp(rectTransform.position, playPos, lerpFactor);
                }
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentState == 2 || currentState == 3)
        {
            canvasGroup.blocksRaycasts = true; //Re-enable raycast blocking so that the UI object can receive raycasts again
            if (currentState == 2)
            {
                TransitionToState0();
            }
        }
    }

    private void HandleHoverState()
    {
        //highlightEffect.SetActive(true);
        rectTransform.localScale = originalScale * selectScale;
    }

    private void HandleDragState()
    {
        //set card rotation to zero
        rectTransform.localRotation = Quaternion.identity;
    }

    private void HandlePlayState()
    {
        rectTransform.localPosition = playPos;
        rectTransform.localRotation = Quaternion.identity;

        if(Input.mousePosition.y < cardPlay.y)
        {
            currentState = 2; // Transition back to drag state
            playArrow.SetActive(false);
        }

        if (Input.GetMouseButtonUp(0))
        {
            audioManager.PlaySfx(audioManager.playCard);
            PlayCard();
        }
    }

    void PlayCard()
    {
        CardDisplay cardDisplay = GetComponent<CardDisplay>();
        Card target = cardDisplay.cardData;
        // Here you can add logic to handle the card being played
        TransitionToState0();

        if(cardDisplay.isCopied)
        {
            fieldManager.AddCopiedCardToField(target);
        }
        else
        {
            fieldManager.AddCardToField(target);
        }
        handManager.RemoveCardFromHand(target);


        if (target.cardType.Contains(Card.CardType.Power))
        {
            target.cardEffect.ApplyEffect();
        }
        else
        {
            gameManager.EndPlayerTurn();
        }
    }
}
