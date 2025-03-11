// filepath: /Users/emonteiro6629/Documents/GitHub/Card-Game-Template/Assets/Scripts/Card_data.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card_data", menuName = "Cards/Card_data", order = 1)]
public class Card_data : ScriptableObject
{
    public string card_name;
    public string description;
    public float castspeed;
    public float damage;
    public float velocity;
    public int type;
    public int piercing;
    public GameObject projectile;
    public BuffEffect buffEffect;
    public Sprite sprite;
}