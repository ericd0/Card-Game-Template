using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Card", menuName = "Cards/Projectile Card")]
public class ProjectileCard_data : Card_data
{
    public float damage;
    public float velocity;
    public int piercing;
    public bool repeatPiercing;
    public GameObject projectile;
}