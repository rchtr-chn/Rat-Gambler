using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CardMovementScript : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform _rectTransform;
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;
    
    public AudioManagerScript AudioManager;

    private Vector2 _originalLocalPointerPos;
    private Vector3 _originalPanelLocalPos;
    private Vector3 _originalScale;

    private int _currentState = 0;

    private Quaternion _originalRotation;
    private Vector3 _originalPosition;

    [SerializeField] private float _selectScale = 1.2f;
    public Vector2 CardPlay;
    [SerializeField] private Vector3 _playPos;
    [SerializeField] private GameObject _highlightEffect;
    [SerializeField] private GameObject _playArrow;
    [SerializeField] private float _lerpFactor = 0.1f;

    private FieldManagerScript _fieldManager;
    private HandManagerScript _handManager;
    private GameManagerScript _gameManager;

    private void Awake()
    {
        AudioManager = FindObjectOfType<AudioManagerScript>();

        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        _originalScale = _rectTransform.localScale;
        _originalRotation = _rectTransform.localRotation;
        _originalPosition = _rectTransform.localPosition;


        _fieldManager = GameObject.Find("PlayerFieldManager").GetComponent<FieldManagerScript>();
        _handManager = GameObject.Find("PlayerHandManager").GetComponent<HandManagerScript>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
    }

    private void Update()
    {
        switch (_currentState)
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
                if (_gameManager.IsPlayerTurn)
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
        _currentState = 0;
        _rectTransform.localScale = _originalScale;
        _rectTransform.localRotation = _originalRotation;
        _rectTransform.localPosition = _originalPosition;
        //highlightEffect.SetActive(false);
        _playArrow.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_currentState == 0)
        {
            _originalPosition = _rectTransform.localPosition;
            _originalRotation = _rectTransform.localRotation;
            _originalScale = _rectTransform.localScale;

            AudioManager.PlaySfx(AudioManager.HoverCard);

            _currentState = 1; // Hover state
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_currentState == 1)
        {
            TransitionToState0();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_currentState == 1 && _gameManager.IsPlayerTurn)
        {
            _currentState = 2; // Drag state

            RectTransformUtility.ScreenPointToLocalPointInRectangle
                (_canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out _originalLocalPointerPos);

            _originalPanelLocalPos = _rectTransform.localPosition;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_currentState == 2)
        {
            _canvasGroup.blocksRaycasts = false; //Disable raycast blocking so that other UI elements can receive raycasts while dragging
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(_currentState == 2)
        {
            Vector2 localPointerPos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localPointerPos))
            {
                _rectTransform.position = Vector3.Lerp(_rectTransform.position, Input.mousePosition, _lerpFactor);

                if (_rectTransform.localPosition.y > CardPlay.y)
                {
                    _currentState = 3; // Play state
                    _playArrow.SetActive(true);
                    _rectTransform.localPosition = Vector3.Lerp(_rectTransform.position, _playPos, _lerpFactor);
                }
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_currentState == 2 || _currentState == 3)
        {
            _canvasGroup.blocksRaycasts = true; //Re-enable raycast blocking so that the UI object can receive raycasts again
            if (_currentState == 2)
            {
                TransitionToState0();
            }
        }
    }

    private void HandleHoverState()
    {
        //highlightEffect.SetActive(true);
        _rectTransform.localScale = _originalScale * _selectScale;
    }

    private void HandleDragState()
    {
        //set card rotation to zero
        _rectTransform.localRotation = Quaternion.identity;
    }

    private void HandlePlayState()
    {
        _rectTransform.localPosition = _playPos;
        _rectTransform.localRotation = Quaternion.identity;

        if(Input.mousePosition.y < CardPlay.y)
        {
            _currentState = 2; // Transition back to drag state
            _playArrow.SetActive(false);
        }

        if (Input.GetMouseButtonUp(0))
        {
            AudioManager.PlaySfx(AudioManager.PlayCard);
            PlayCard();
        }
    }

    void PlayCard()
    {
        CardDisplay cardDisplay = GetComponent<CardDisplay>();
        Card target = cardDisplay.CardData;
        // Here you can add logic to handle the card being played
        TransitionToState0();

        if(cardDisplay.IsCopied)
        {
            _fieldManager.AddCopiedCardToField(target);
        }
        else
        {
            _fieldManager.AddCardToField(target);
        }
        _handManager.RemoveCardFromHand(target);


        if (target.CardType.Contains(Card.CardTypes.Power))
        {
            target.CardEffect.ApplyEffect();
        }
        else
        {
            _gameManager.EndPlayerTurn();
        }
    }
}
