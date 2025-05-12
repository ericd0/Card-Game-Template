using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Body : MonoBehaviour
{
    [Header("Stats")]
    public float health;
    public float maxHealth;
    public float damage;
    public float moveSpeed;
    public float moveSpeedMultiplier = 1f;

    [Header("Team")]
    public int team; // 0 = player, 1 = enemy, 2 = neutral

    [Header("Combat")]
    public bool collisionDamage;
    public float iFrameDuration = 0.5f;
    private Dictionary<GameObject, float> iFrameTimers = new Dictionary<GameObject, float>();

    [Header("Health Regeneration")]
    public bool hasHealthRegen;
    public float regenAmount = 0.8f;
    public float regenInterval = 1f;
    private float regenTimer = 0f;

    protected virtual void Start()
    {
        health = maxHealth;
    }

    protected virtual void Update()
    {
        UpdateIFrames();
        
        if (hasHealthRegen)
        {
            UpdateHealthRegen();
        }
    }

    private void UpdateIFrames()
    {
        List<GameObject> expiredTimers = new List<GameObject>();
        
        foreach (var kvp in iFrameTimers.ToList())
        {
            iFrameTimers[kvp.Key] -= Time.deltaTime;
            if (iFrameTimers[kvp.Key] <= 0)
            {
                expiredTimers.Add(kvp.Key);
            }
        }
        
        foreach (var key in expiredTimers)
        {
            iFrameTimers.Remove(key);
        }
    }

    private void UpdateHealthRegen()
    {
        regenTimer -= Time.deltaTime;
        if (regenTimer <= 0f && health < maxHealth)
        {
            if (maxHealth - health < regenAmount)
            {
                health = maxHealth;
            }
            else
            {
                health += regenAmount;
            }
            regenTimer = regenInterval;
        }
    }

    public bool HasIFramesFor(GameObject attacker)
    {
        return iFrameTimers.ContainsKey(attacker) && iFrameTimers[attacker] > 0;
    }

    public void AddIFramesFor(GameObject attacker)
    {
        iFrameTimers[attacker] = iFrameDuration;
    }

    public virtual void TakeDamage(float incomingDamage, GameObject attacker = null)
    {
        if (attacker != null && HasIFramesFor(attacker))
            return;

        health -= incomingDamage;
        if (attacker != null)
            AddIFramesFor(attacker);

        // Update health bar if present
        HealthBarAbove healthBar = GetComponent<HealthBarAbove>();
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(health, maxHealth);
        }

        if (health <= 0)
            Die();
    }
    public virtual void SetStats()
    {
        moveSpeed = moveSpeed * moveSpeedMultiplier;
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    public bool IsHostileTo(Body other)
    {
        // Neutral (team 2) is not hostile to anyone
        if (team == 2 || other.team == 2)
            return false;
            
        // Different non-neutral teams are hostile to each other
        return team != other.team;
    }

    protected virtual void OnCollisionStay2D(Collision2D other)
    {
        if (!collisionDamage) return;

        Body otherBody = other.gameObject.GetComponent<Body>();
        if (otherBody != null && IsHostileTo(otherBody) && !HasIFramesFor(other.gameObject))
        {
            otherBody.TakeDamage(damage, gameObject);
            AddIFramesFor(other.gameObject);
        }
    }
}
