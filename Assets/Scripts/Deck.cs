// filepath: /Users/emonteiro6629/Documents/GitHub/Card-Game-Template/Assets/Scripts/Deck.cs
using UnityEngine;

[CreateAssetMenu(fileName = "NewDeck", menuName = "Deck")]
public class Deck : ScriptableObject
{
    public Card_data[] cards;
}