using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public abstract class Enemy : MonoBehaviour
{
    public float health;
    public float maxHealth;  // Add this field
    public bool collisionDamage;
    public float damage;
    public GameObject healthBarPrefab;  // Assign the HealthBarCanvas prefab
    public float iFrameDuration = 0.5f;  // Duration of invincibility after being hit
    private Dictionary<GameObject, float> iFrameTimers = new Dictionary<GameObject, float>();

    private GameObject healthBarInstance;
    private Image healthFill;
    private float healthBarOffset = 0.5f;  // Adjust based on sprite size
    private Canvas mainCanvas;
    private RectTransform healthBarRect;
    protected virtual void Start()
    {
        maxHealth = health;
        mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>(); // Make sure to tag your main canvas
        InitializeHealthBar();
    }

    protected virtual void Update()
    {
        // Health bar position update
        if (healthBarInstance != null)
        {
            Vector3 worldPosition = transform.position + Vector3.up * healthBarOffset;
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            healthBarRect.position = screenPosition;
        }

        // Create a separate list for keys to remove
        List<GameObject> expiredTimers = new List<GameObject>();
        
        // Use ToList() to create a copy of the keys for safe iteration
        foreach (var kvp in iFrameTimers.ToList())
        {
            iFrameTimers[kvp.Key] -= Time.deltaTime;
            if (iFrameTimers[kvp.Key] <= 0)
            {
                expiredTimers.Add(kvp.Key);
            }
        }
        
        // Remove expired timers after iteration is complete
        foreach (var key in expiredTimers)
        {
            iFrameTimers.Remove(key);
        }
    }

    private void InitializeHealthBar()
    {
        if (mainCanvas == null)
        {
            Debug.LogError("Main canvas not found! Make sure it's tagged as 'MainCanvas'");
            return;
        }

        healthBarInstance = Instantiate(healthBarPrefab, mainCanvas.transform);
        healthBarRect = healthBarInstance.GetComponent<RectTransform>();
        healthFill = healthBarInstance.GetComponentsInChildren<Image>()[1];
        
        // Set initial scale - you may need to adjust this value
        healthBarRect.localScale = Vector3.one * 0.5f;
    }
    protected virtual void Die()
    {
        if (healthBarInstance != null)
        {
            Destroy(healthBarInstance);
        }
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
        if (healthFill != null)
        {
            // Update fill amount based on current health percentage
            healthFill.fillAmount = Mathf.Clamp01(health / maxHealth);
        }
        if (health <= 0)
        {
            Die();
        }
        Debug.Log($"{gameObject.name} hit! Health: {health}");
    }
    protected virtual void OnCollisionStay2D(Collision2D other)
    {
        if (collisionDamage == true)
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