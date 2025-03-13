using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class Enemy : MonoBehaviour
{
    public float health;
    public float maxHealth;  // Add this field
    public bool collisionDamage;
    public float damage;
    public GameObject healthBarPrefab;  // Assign the HealthBarCanvas prefab
    protected HashSet<GameObject> hitByProjectiles = new HashSet<GameObject>();
    
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
        if (healthBarInstance != null)
        {
            // Convert world position to screen position
            Vector3 worldPosition = transform.position + Vector3.up * healthBarOffset;
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            healthBarRect.position = screenPosition;
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
    protected virtual void OnTriggerEnter2D(Collider2D other)
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
                OnTakeDamage(projectile.damage);  // This calls the health bar update
                
                if (health <= 0)
                {
                    Die();
                }
            }
        }
    }
    protected virtual void OnTakeDamage(float damage)
    {
        if (healthFill != null)
        {
            // Update fill amount based on current health percentage
            healthFill.fillAmount = Mathf.Clamp01(health / maxHealth);
        }
        Debug.Log($"{gameObject.name} hit! Health: {health}");
    }
}