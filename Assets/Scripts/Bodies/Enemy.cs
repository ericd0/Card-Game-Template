using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class Enemy : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public bool collisionDamage;
    public float damage;
    public float iFrameDuration = 0.5f;
    private Dictionary<GameObject, float> iFrameTimers = new Dictionary<GameObject, float>();

    protected virtual void Start()
    {
        maxHealth = health;
    }

    protected virtual void Update()
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

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    public bool HasIFramesFor(GameObject attacker)
    {
        return iFrameTimers.ContainsKey(attacker) && iFrameTimers[attacker] > 0;
    }

    public void AddIFramesFor(GameObject attacker)
    {
        iFrameTimers[attacker] = iFrameDuration;
    }

    public virtual void OnTakeDamage(float damage)
    {
        if (health <= 0)
        {
            Die();
        }
        Debug.Log($"{gameObject.name} hit! Health: {health}");
    }

    protected virtual void OnCollisionStay2D(Collision2D other)
    {
        if (collisionDamage)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Player player = other.gameObject.GetComponent<Player>();
                if (!HasIFramesFor(other.gameObject))
                {
                    player.TakeDamage(damage);
                    AddIFramesFor(other.gameObject);
                }
            }
        }
    }
}