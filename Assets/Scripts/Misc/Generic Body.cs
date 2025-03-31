using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public abstract class GenericBody : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 100f;
    public float health;
    public float damage = 10f;
    public bool collisionDamage;
    public float iFrameDuration = 0.5f;

    [Header("UI")]
    public bool showHealthBar = true;
    public GameObject healthBarPrefab;
    protected GameObject healthBarInstance;
    protected Image healthFill;
    protected float healthBarOffset = 0.5f;
    protected Canvas mainCanvas;
    protected RectTransform healthBarRect;

    [Header("Team")]
    public int teamId = 0; // 0 = neutral, 1 = player team, 2 = enemy team, etc.

    protected Dictionary<GameObject, float> iFrameTimers = new Dictionary<GameObject, float>();

    protected virtual void Start()
    {
        health = maxHealth;
        mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        InitializeHealthBar();
    }

    protected virtual void Update()
    {
        UpdateHealthBar();
        UpdateIFrames();
    }

    protected virtual void InitializeHealthBar()
    {
        if (!showHealthBar) return;

        if (mainCanvas == null)
        {
            Debug.LogError("Main canvas not found! Make sure it's tagged as 'MainCanvas'");
            return;
        }

        healthBarInstance = Instantiate(healthBarPrefab, mainCanvas.transform);
        healthBarRect = healthBarInstance.GetComponent<RectTransform>();
        healthFill = healthBarInstance.GetComponentsInChildren<Image>()[1];
    }

    protected virtual void UpdateHealthBar()
    {
        if (healthBarInstance != null)
        {
            healthFill.fillAmount = Mathf.Clamp01(health / maxHealth);
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

    public virtual void TakeDamage(float damage, GameObject attacker = null)
    {
        if (attacker != null && HasIFramesFor(attacker))
            return;

        health -= damage;
        if (attacker != null)
            AddIFramesFor(attacker);

        if (health <= 0)
            Die();

        UpdateHealthBar();
    }

    public bool HasIFramesFor(GameObject attacker)
    {
        return iFrameTimers.ContainsKey(attacker) && iFrameTimers[attacker] > 0;
    }

    public void AddIFramesFor(GameObject attacker)
    {
        iFrameTimers[attacker] = iFrameDuration;
    }

    protected virtual void Die()
    {
        if (healthBarInstance != null)
            Destroy(healthBarInstance);
        
        Destroy(gameObject);
    }

    public bool IsHostile(GenericBody other)
    {
        return teamId != other.teamId && teamId != 0 && other.teamId != 0;
    }
}