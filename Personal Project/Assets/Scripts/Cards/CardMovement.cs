using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private RectTransform canvasRectTransform;
    private Vector3 originalScale;
    private int currentState = 0;
    private Quaternion originalRotation;
    private Vector3 originalPosition;
    private GridManager gridManager;
    private readonly int maxColumn = 4;//a changer max column where we can play

    [SerializeField] private float selectScale = 1.1f;
    [SerializeField] private Vector2 cardPlay;
    [SerializeField] private Vector3 playPosition;
    [SerializeField] private GameObject glowEffect;
    [SerializeField] private GameObject playArrow;
    [SerializeField] private float lerpFactor = 0.1f;
    [SerializeField] private int cardPlayDivider = 4;
    [SerializeField] private float cardPlayMultiplier = 1f;
    [SerializeField] private bool needUpdateCardPlayPosition = false;
    [SerializeField] private int playPositionYDivider = 2;
    [SerializeField] private float playPositionYMultiplier = 1f;
    [SerializeField] private int playPositionXDivider = 4;
    [SerializeField] private float playPositionXMultiplier = 1f;
    [SerializeField] private bool needUpdatePlayPosition = false;

    private LayerMask gridLayerMask;
    private LayerMask characterLayerMask;

    private Card cardData;
    private CardDisplay cardDisplay;
    HandManager handManager;
    DiscardManager discardManager;

    bool isPlaced = true;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        if (canvas != null)
        {
            canvasRectTransform = canvas.GetComponent<RectTransform>();
        }

        originalScale = rectTransform.localScale;
        originalPosition = rectTransform.localPosition;
        originalRotation = rectTransform.localRotation;

        UpdateCardPlayPosition();
        UpdatePlayPosition();

        gridManager = FindObjectOfType<GridManager>();
        handManager = FindObjectOfType<HandManager>();
        discardManager = FindObjectOfType<DiscardManager>();
        cardDisplay = GetComponent<CardDisplay>();

        gridLayerMask = LayerMask.GetMask("Grid");
        characterLayerMask = LayerMask.GetMask("Characters");

        cardData = cardDisplay.cardData;
    }

    void Update()
    {
        if (needUpdateCardPlayPosition)
        {
            UpdateCardPlayPosition();
        }
        if (needUpdatePlayPosition)
        {
            UpdatePlayPosition();
        }

        if (cardData != cardDisplay.cardData)
        {
            cardData = cardDisplay.cardData;
        }

        switch (currentState)
        {
            case 1:
                HandleHoverState();
                break;

            case 2:
                HandleDragState();

                if (!Input.GetMouseButton(0)) // check if mouse button is realised
                {
                    StopAllCoroutines();
                    TransitionToState0();
                }
                break;
            case 3:
                HandlePlayState();

                if (!Input.GetMouseButton(0)) // check if mouse button is realised
                {
                    StopAllCoroutines();
                    TransitionToState0();
                }
                break;
        }
    }

    private void TransitionToState0()
    {
        isPlaced = false;

        currentState = 0;
        GameManager.Instance.PlayingCard = false;
        rectTransform.localScale = originalScale;
        rectTransform.localRotation = originalRotation;

        if (rectTransform.localPosition != originalPosition)
        {
            StopAllCoroutines();
            StartCoroutine(Utility.TranslateLocalPos(gameObject, originalPosition, 0.5f, Easing.EaseOutExpo, () => isPlaced = true));
        }
        else
        {
            isPlaced = true;
        }


        //rectTransform.localPosition = originalPosition;

        glowEffect.SetActive(false);//disable glow effect;
        playArrow.SetActive(false);//disable playArrow
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentState == 0 && isPlaced)
        {
            originalPosition = rectTransform.localPosition;
            originalRotation = rectTransform.localRotation;
            originalScale = rectTransform.localScale;

            currentState = 1;
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
        if (currentState == 1)
        {
            currentState = 2;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentState == 2)
        {
            if (Input.mousePosition.y > cardPlay.y)
            {
                currentState = 3;
                playArrow.SetActive(true);

                StopAllCoroutines();
                //move to play position
               StartCoroutine(Utility.TranslateLocalPos(gameObject, playPosition, 0.5f, Easing.EaseOutExpo));
            }
        }
    }


    private void HandleHoverState()
    {
        glowEffect.SetActive(true);
        rectTransform.localScale = originalScale * selectScale;
    }

    private void HandleDragState()
    {
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.position = Easing.EasingVector(rectTransform.position, Input.mousePosition, lerpFactor,Easing.EaseOutQuad) ;
    }

    private void HandlePlayState()
    {
        if (!GameManager.Instance.PlayingCard)
        {
            GameManager.Instance.PlayingCard = true;
        }


        rectTransform.localRotation = Quaternion.identity;

        if (!Input.GetMouseButton(0))
        {
            if (GameManager.Instance.PlayerTurn)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (cardData is Character characterCard)
                {
                    TryToPlayCharacterCard(ray, characterCard);
                }
                else if (cardData is Spell spellCard)
                {
                    TryToPlaySpellCard(ray, spellCard);
                }
            }

            TransitionToState0();
        }

        if (Input.mousePosition.y < cardPlay.y)
        {
           StopAllCoroutines();

            currentState = 2;
            playArrow.SetActive(false);
        }
    }

    private void TryToPlayCharacterCard(Ray ray, Character characterCard)
    {
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, gridLayerMask);

        if (hit.collider != null && hit.collider.TryGetComponent<GridCell>(out var cell))
        {
            Vector2 targetPos = cell.gridIndex;

            if (cell.gridIndex.x < maxColumn && gridManager.AddObjectToGrid(characterCard.prefab, targetPos))
            {
                characterCard.isAllie = true;
                cell.objectInCell.GetComponent<CharacterStats>().characterStartData = characterCard;
                handManager.cardsInPlayerHand.Remove(gameObject);
                discardManager.AddToDiscard(cardData);
                handManager.UpdateHandVisuals();

                GameManager.Instance.CardPlayed = true;
                //Debug.Log($"Placed character {characterCard.prefab}");
                Destroy(gameObject);
            }
        }
    }

    private void TryToPlaySpellCard(Ray ray, Spell spellCard)
    {
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, characterLayerMask);

        if (hit.collider != null && hit.collider.TryGetComponent<CharacterStats>(out var targetStats))
        {
            SpellEffectApplier.ApplySpell(spellCard, targetStats);
            handManager.cardsInPlayerHand.Remove(gameObject);
            discardManager.AddToDiscard(cardData);
            handManager.UpdateHandVisuals();

            GameManager.Instance.CardPlayed = true;
            //Debug.Log($"Played spell; {spellCard.name}");
            Destroy(gameObject);

        }
    }

    private void UpdateCardPlayPosition()
    {
        if (cardPlayDivider != 0 && canvasRectTransform != null)
        {
            float segment = cardPlayMultiplier / cardPlayDivider;

            cardPlay.y = canvasRectTransform.rect.height * segment;
        }
    }

    private void UpdatePlayPosition()
    {
        if (canvasRectTransform != null && playPositionYDivider != 0 && playPositionXDivider != 0)
        {
            float segmentX = playPositionXMultiplier / playPositionXDivider;
            float segmentY = playPositionYMultiplier / playPositionYDivider;

            playPosition.x = canvasRectTransform.rect.width * segmentX;
            playPosition.y = canvasRectTransform.rect.height * segmentY;
        }
    }
}
