// filepath: /Users/emonteiro6629/Documents/GitHub/Card-Game-Template/Assets/Scripts/Card_data.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card_data", menuName = "Cards/Card_data", order = 1)]
public class Card_data : ScriptableObject
{
    public string card_name;
    public string description;
    public int castspeed;
    public int damage;
    public int type;
    public GameObject projectile;
    public Sprite sprite;
}