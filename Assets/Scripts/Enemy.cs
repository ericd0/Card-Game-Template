using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    private HashSet<GameObject> hitByProjectiles = new HashSet<GameObject>();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerProjectile"))
        {
            // Check if this projectile has already hit this enemy
            if (hitByProjectiles.Contains(other.gameObject))
            {
                return;
            }

            Projectile projectile = other.gameObject.GetComponent<Projectile>();
            if (projectile != null)
            {
                hitByProjectiles.Add(other.gameObject);
                health -= projectile.damage;
                Debug.Log("Enemy hit! Health: " + health);
                if (health <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}