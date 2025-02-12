using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public List<Card_data> deck = new List<Card_data>();
    public List<Card_data> hand = new List<Card_data>();
    public List<Card_data> discard_pile = new List<Card_data>();

    public Deck[] startingDecks; // Array to hold multiple starting decks
    public int selectedDeckIndex = 0; // Index to select which starting deck to use

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
    // Start is called before the first frame update
    void Start()
    {
        InitializeDeck();
        ShuffleDeck();
    }

    // Update is called once per frame
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
    }

    void InitializeDeck()
    {
        // Clear the current deck
        deck.Clear();

        // Add cards to the deck from the selected starting deck
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
        if (deck.Count > 0)
        {
            Card_data drawnCard = deck[0];
            deck.RemoveAt(0);
            hand.Add(drawnCard);
        }
        else
        {
            Debug.Log("Deck is empty!");
        }
    }

    void ShuffleDeck()
    {
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
        Debug.Log("Hand:");
        foreach (Card_data card in hand)
        {
            Debug.Log(card.card_name);
        }

        Debug.Log("Deck:");
        foreach (Card_data card in deck)
        {
            Debug.Log(card.card_name);
        }

        Debug.Log("Discard Pile:");
        foreach (Card_data card in discard_pile)
        {
            Debug.Log(card.card_name);
        }
    }
}