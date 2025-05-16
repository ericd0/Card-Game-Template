using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Card", menuName = "Cards/Projectile Card")]
public class ProjectileCard_data : Card_data
{
    public float damage;
    public float velocity;
    public int piercing;
    [Tooltip("Allows the projectile to hit the same target multiple times")]
    public bool repeatPiercing;
    public GameObject projectile;
    public Effect[] onHitEffects; // Store Effect components directly
}