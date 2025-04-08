using UnityEngine;
using UnityEngine.UI;

public class HealthBarAbove : MonoBehaviour
{
    private GameObject healthBarInstance;
    private Image healthFill;
    private float healthBarOffset = 0.5f;
    private Canvas mainCanvas;
    private RectTransform healthBarRect;
    public GameObject healthBarPrefab;

    public void Initialize()
    {
        mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
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

    void Update()
    {
        if (healthBarInstance != null)
        {
            Vector3 worldPosition = transform.position + Vector3.up * healthBarOffset;
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            healthBarRect.position = screenPosition;
        }
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthFill != null)
        {
            healthFill.fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
        }
    }

    public void DestroyHealthBar()
    {
        if (healthBarInstance != null)
        {
            Destroy(healthBarInstance);
        }
    }

    private void OnDestroy()
    {
        DestroyHealthBar();
    }
}
