using UnityEngine;
using System.Collections.Generic;

public abstract class Enemy : MonoBehaviour
{
    public float health;
    public bool collisionDamage;
    public float damage;
    protected HashSet<GameObject> hitByProjectiles = new HashSet<GameObject>();

    protected virtual void Start()
    {
        // Base initialization if needed
    }

    protected virtual void Update()
    {
        // Base update logic if needed
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerProjectile"))
        {
            if (hitByProjectiles.Contains(other.gameObject))
            {
                return;
            }

            Projectile projectile = other.gameObject.GetComponent<Projectile>();
            if (projectile != null)
            {
                hitByProjectiles.Add(other.gameObject);
                health -= projectile.damage;
                OnTakeDamage(projectile.damage);
                
                if (health <= 0)
                {
                    Die();
                }
            }
        }
    }

    protected virtual void OnTakeDamage(float damage)
    {
        Debug.Log($"{gameObject.name} hit! Health: {health}");
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}