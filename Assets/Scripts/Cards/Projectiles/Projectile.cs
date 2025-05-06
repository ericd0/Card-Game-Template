using UnityEngine;
using System.Collections.Generic;

public abstract class Projectile : Card
{
    protected Card_data cardData;
    public float velocity;
    public float lifespan;
    public float damage;
    public int piercing;
    public bool repeatPiercing;
    protected Vector3 direction;
    protected HashSet<GameObject> hitBodies = new HashSet<GameObject>();
    public int team;
    protected Effect[] onHitEffects;

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

    public virtual void SetStats(Card_data cardData)
    {
        if (cardData is ProjectileCard_data projectileData)
        {
            damage = projectileData.damage;
            velocity = projectileData.velocity;
            piercing = projectileData.piercing;
            repeatPiercing = projectileData.repeatPiercing;
            onHitEffects = projectileData.onHitEffects;
            
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

    protected virtual void ApplyOnHitEffects(Body target)
    {
        if (onHitEffects == null) return;
        
        foreach (Effect effectPrefab in onHitEffects)
        {
            if (effectPrefab != null)
            {
                Effect effect = target.gameObject.AddComponent(effectPrefab.GetType()) as Effect;
                if (effect != null)
                {
                    effect.Initialize(damage); // Pass the projectile's damage
                }
            }
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
            ApplyOnHitEffects(hitBody);
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