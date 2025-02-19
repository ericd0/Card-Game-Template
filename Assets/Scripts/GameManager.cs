using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public List<Card_data> deck = new List<Card_data>();
    public List<Card_data> hand = new List<Card_data>();
    public List<Card_data> discard_pile = new List<Card_data>();
    public int selectedCardIndex = 0;

    public Deck[] startingDecks;
    public int selectedDeckIndex = 0;
    public int handSize = 7;

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

    void Start()
    {
        InitializeDeck();
        ShuffleDeck();
        FillHand();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ShuffleDeck();
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
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            selectedCardIndex++;
            if (selectedCardIndex >= hand.Count)
                selectedCardIndex = 0;
        }
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
            if (hand.Count > 0)
            {
                // Move selected card to discard
                Card_data selectedCard = hand[selectedCardIndex];
                hand.RemoveAt(selectedCardIndex);
                discard_pile.Add(selectedCard);

                // Adjust selected card index if needed
                if (selectedCardIndex >= hand.Count && hand.Count > 0)
                {
                    selectedCardIndex = hand.Count - 1;
                }

                // Draw new cards if possible
                FillHand();
            }
        }
    void FillHand()
    {
        while (hand.Count < handSize && deck.Count > 0)
        {
            Draw();
        }
    }

    void ShuffleDeck()
    {
        // Add discard pile back to deck
        deck.AddRange(discard_pile);
        discard_pile.Clear();

        // Shuffle the deck
        for (int i = 0; i < deck.Count; i++)
        {
            Card_data temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
        Debug.Log("Deck shuffled!");
    }
    void PrintCardCollections()
    {
        List<string> handNames = hand.Select((card, index) => 
            index == selectedCardIndex ? $"[{card.card_name}]" : card.card_name).ToList();
        List<string> deckNames = deck.Select(card => card.card_name).ToList();
        List<string> discardNames = discard_pile.Select(card => card.card_name).ToList();

        Debug.Log($"Hand ({hand.Count}): {string.Join(", ", handNames)}");
        Debug.Log("Selected Card: " + selectedCardIndex);
        Debug.Log($"Deck ({deck.Count}): {string.Join(", ", deckNames)}");
        Debug.Log($"Discard ({discard_pile.Count}): {string.Join(", ", discardNames)}");
    }
}