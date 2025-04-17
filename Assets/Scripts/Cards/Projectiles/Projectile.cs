using UnityEngine;
using System.Collections.Generic;

public abstract class Projectile : Card
{
    public float velocity;
    public float lifespan;
    public float damage;
    public int piercing;
    public bool repeatPiercing;
    protected Vector3 direction;
    protected HashSet<GameObject> hitBodies = new HashSet<GameObject>();
    public int team;

    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Projectile");
        Destroy(gameObject, lifespan);
        team = caster.GetComponent<Body>().team;
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

    public virtual bool CanHitBody(Body targetBody)
    {
        if (!repeatPiercing && hitBodies.Contains(targetBody.gameObject))
        {
            return false;
        }

        if (piercing == 0)
        {
            Destroy(gameObject);
            return false;
        }

        return true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Body hitBody = other.GetComponent<Body>();
        if (hitBody != null && hitBody.team != team && team != 2 && hitBody.team != 2) // Using team check directly
        {
            if (!CanHitBody(hitBody))
            {
                return;
            }

            hitBody.TakeDamage(damage, gameObject);
            hitBodies.Add(other.gameObject);
            
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