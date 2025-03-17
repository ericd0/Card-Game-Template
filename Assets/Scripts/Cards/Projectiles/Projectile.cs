using UnityEngine;
using System.Collections.Generic;

public abstract class Projectile : MonoBehaviour
{
    public float velocity;
    public float lifespan;
    public float damage;
    public int piercing;
    public bool repeatPiercing;
    protected Vector3 direction;
    protected HashSet<GameObject> hitEnemies = new HashSet<GameObject>();

    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Projectile");
        Destroy(gameObject, lifespan);
        OnStart();
    }

    void Update()
    {
        OnUpdate();
    }

    protected abstract void OnStart();
    protected abstract void OnUpdate();

    public void SetStats(Card_data cardData)
    {
        if (cardData is ProjectileCard_data projectileData)
        {
            damage = projectileData.damage;
            velocity = projectileData.velocity;
            piercing = projectileData.piercing;
            repeatPiercing = projectileData.repeatPiercing;
            if (piercing >= 0)
            {
                //Piercing at 0 makes it hit nothing, so we add 1 to make it hit at least once
                piercing += 1;
            }
        }
        else
        {
            Debug.LogError($"Tried to set projectile stats with non-projectile card: {cardData.GetType()}");
        }
    }

    public virtual bool CanHitEnemy(GameObject enemy)
    {
        // Can't hit if already hit and not using repeat piercing
        if (!repeatPiercing && hitEnemies.Contains(enemy))
        {
            return false;
        }

        // Can't hit if out of pierces (unless negative for infinite)
        if (piercing == 0)
        {
            Destroy(gameObject);
            return false;
        }

        return true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!CanHitEnemy(other.gameObject))
            {
                return;
            }

            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.health -= damage;
                enemy.OnTakeDamage(damage);
            }

            hitEnemies.Add(other.gameObject);
            
            // Reduce piercing if not infinite (negative)
            if (piercing > 0)
            {
                piercing--;
                if (piercing == 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}