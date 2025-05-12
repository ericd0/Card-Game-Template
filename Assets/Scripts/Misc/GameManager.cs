using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public GameObject playerPrefab; // Assign in inspector
    private Player currentPlayer;
    
    // Add events for player-related actions
    public static event System.Action<Player> OnPlayerSpawned;
    public static event System.Action<Player> OnPlayerDespawned;
    public static event System.Action<Projectile> OnProjectileCast;
    public List<Card_data> deck = new List<Card_data>();
    public List<Card_data> hand = new List<Card_data>();
    public List<Card_data> discard_pile = new List<Card_data>();
    private int selectedCardIndex = 0;
    private float cardCooldown = 0f;

    // UI Related
    private Canvas gameCanvas;
    public GameObject blankCardPrefab;
    private List<GameObject> cardObjects = new List<GameObject>();
    public float cardSpacing = 150f;
    public float bottomOffset = 100f;
    public float selectedCardRaise = 30f;
    public float leftEdgeOffset = 100f;
    public TextMeshProUGUI deckCountText;
    public TextMeshProUGUI discardCountText;

    public Deck[] startingDecks;
    public int selectedDeckIndex = 0;
    public int handSize = 7;
    public GameObject slimePrefab;
    private bool isShuffling = false;

    private void Awake()
    {
        if (gm != null && gm != this)
        {
            Destroy(gameObject);
        }
        else
        {
            gm = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() 
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "game") // Only spawn in game scene
        {
            SpawnPlayer();
            InitializeGameState();
        }
        else
        {
            DespawnPlayer();
        }
    }

    private void SpawnPlayer()
    {
        if (currentPlayer != null)
        {
            DespawnPlayer();
        }

        GameObject playerObj = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        currentPlayer = playerObj.GetComponent<Player>();
        OnPlayerSpawned?.Invoke(currentPlayer);
    }

    private void DespawnPlayer()
    {
        if (currentPlayer != null)
        {
            OnPlayerDespawned?.Invoke(currentPlayer);
            Destroy(currentPlayer.gameObject);
            currentPlayer = null;
        }
    }

    private void InitializeGameState()
    {
        // Make sure we have the canvas
        if (gameCanvas == null)
        {
            gameCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        }

        // Initialize game state
        InitializeDeck();
        RandomizeDeck();
        FillHand();
        UpdateHandDisplay();
        UpdateDeckUI();
    }

    void Start()
    {
        // Only find the canvas reference, don't re-initialize everything
        gameCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        if (gameCanvas == null)
        {
            Debug.LogError("Main canvas not found! Make sure it's tagged as 'MainCanvas'");
        }
    }

    void Update()
    {
        if (cardCooldown > 0)
        {
            cardCooldown -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.R) && !isShuffling)
        {
            StartCoroutine(ShuffleDeckCoroutine());
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            PrintCardCollections();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            selectedCardIndex--;
            if (selectedCardIndex < 0)
                selectedCardIndex = hand.Count - 1;
            UpdateHandDisplay();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            selectedCardIndex++;
            if (selectedCardIndex >= hand.Count)
                selectedCardIndex = 0;
            UpdateHandDisplay();
        }
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            Instantiate(slimePrefab, Vector3.zero, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            if (SceneManager.GetActiveScene().name == "shop")
            {
                SceneManager.LoadScene("game");
            }
            else
            {
                SceneManager.LoadScene("shop");
            }
        }
    }

    void UpdateHandDisplay()
    {
        foreach (GameObject card in cardObjects)
        {
            Destroy(card);
        }
        cardObjects.Clear();

        RectTransform canvasRect = gameCanvas.GetComponent<RectTransform>();
        float canvasHeight = canvasRect.rect.height;
        float canvasWidth = canvasRect.rect.width;

        float baseY = (-canvasHeight/2) + bottomOffset;
        float startX = (-canvasWidth/2) + leftEdgeOffset;

        for (int i = 0; i < hand.Count; i++)
        {
            GameObject cardObj = Instantiate(blankCardPrefab, gameCanvas.transform);
            cardObjects.Add(cardObj);

            float xPos = startX + (i * cardSpacing);
            float yPos = baseY + (i == selectedCardIndex ? selectedCardRaise : 0);
            cardObj.transform.localPosition = new Vector3(xPos, yPos, 0);

            CardObject cardComponent = cardObj.GetComponent<CardObject>();
            if (cardComponent != null)
            {
                cardComponent.data = hand[i];
            }
        }
    }

    void UpdateDeckUI()
    {
        if (deckCountText != null)
            deckCountText.text = $"Deck: {deck.Count}";
        if (discardCountText != null)
            discardCountText.text = $"Discard: {discard_pile.Count}";
    }

    void InitializeDeck()
    {
        deck.Clear();
        if (startingDecks != null && startingDecks.Length > selectedDeckIndex)
        {
            foreach (Card_data card in startingDecks[selectedDeckIndex].cards)
            {
                deck.Add(card);
            }
        }
    }

    void Draw()
    {
        if (deck.Count > 0 && hand.Count < handSize)
        {
            Card_data drawnCard = deck[0];
            deck.RemoveAt(0);
            hand.Add(drawnCard);
            UpdateHandDisplay();
            UpdateDeckUI();
        }
        else if (deck.Count == 0)
        {
            Debug.Log("Deck is empty!");
        }
        else if (hand.Count >= handSize)
        {
            Debug.Log("Hand is full!");
        }
    }

    public void PlayCard()
    {
        if (!isShuffling && currentPlayer != null)  // Use currentPlayer instead of caster
        {
            if (cardCooldown > 0)
            {
                Debug.Log($"Card on cooldown: {cardCooldown:F1} seconds remaining");
                return;
            }

            if (hand.Count > 0)
            {
                Card_data selectedCard = hand[selectedCardIndex];
                cardCooldown = selectedCard.castspeed * 0.03f * currentPlayer.castSpeedMultiplier;
                if (cardCooldown < 0.01f)
                {
                    cardCooldown = 0.01f;
                }
                // Handle different card types using 'is' operator
                if (selectedCard is ProjectileCard_data projectileData)
                {
                    if (projectileData.projectile != null)
                    {
                        GameObject projectileObj = Instantiate(projectileData.projectile, 
                            currentPlayer.transform.position, 
                            currentPlayer.transform.rotation);
                        
                        Projectile projectile = projectileObj.GetComponent<Projectile>();
                        if (projectile != null)
                        {
                            projectile.SetStats(projectileData);
                            projectile.caster = currentPlayer.gameObject; // Set the caster reference
                            OnProjectileCast?.Invoke(projectile);
                        }
                    }
                }
                else if (selectedCard is EffectCard_data buffData)
                {
                    if (buffData.effects != null)
                    {
                        foreach (Effect effectPrefab in buffData.effects)
                        {
                            if (effectPrefab != null)
                            {
                                Effect effect = currentPlayer.gameObject.AddComponent(effectPrefab.GetType()) as Effect;
                                if (effect != null)
                                {
                                    JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(effectPrefab), effect);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"Unhandled card type: {selectedCard.GetType()}");
                }

                hand.RemoveAt(selectedCardIndex);
                discard_pile.Add(selectedCard);

                if (selectedCardIndex >= hand.Count && hand.Count > 0)
                {
                    selectedCardIndex = hand.Count - 1;
                }

                FillHand();
                UpdateHandDisplay();
                UpdateDeckUI();
            }
        }
    }

    void FillHand()
    {
        while (hand.Count < handSize && deck.Count > 0)
        {
            Draw();
        }
    }
    void RandomizeDeck()
    {
        deck.AddRange(discard_pile);
        discard_pile.Clear();

        for (int i = 0; i < deck.Count; i++)
        {
            Card_data temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    IEnumerator ShuffleDeckCoroutine()
    {
        isShuffling = true;
        Debug.Log("Started shuffling...");

        // Calculate shuffle time
        float baseTime = currentPlayer.shuffleSpeed;
        float shuffleSpeedMultiplier = currentPlayer.shuffleSpeedMultiplier;
        float cardMultiplier = deck.Count * 0.02f;
        float notEmptyMultiplier = deck.Count > 0 ? 0.1f : 0f;
        float totalShuffleTime = (baseTime + cardMultiplier + notEmptyMultiplier) * shuffleSpeedMultiplier;
        yield return new WaitForSeconds(totalShuffleTime);

        // Perform the shuffle
        RandomizeDeck();

        FillHand();
        UpdateHandDisplay();
        UpdateDeckUI();
        Debug.Log($"Deck shuffled after {totalShuffleTime:F2} seconds!");
        isShuffling = false;
    }

    void PrintCardCollections()
    {
        List<string> handNames = hand.Select((card, index) => 
            index == selectedCardIndex ? $"[{card.card_name}]" : card.card_name).ToList();
        List<string> deckNames = deck.Select(card => card.card_name).ToList();
        List<string> discardNames = discard_pile.Select(card => card.card_name).ToList();

        Debug.Log($"Hand ({hand.Count}): {string.Join(", ", handNames)}");
        Debug.Log($"Deck ({deck.Count}): {string.Join(", ", deckNames)}");
        Debug.Log($"Discard ({discard_pile.Count}): {string.Join(", ", discardNames)}");
    }
}