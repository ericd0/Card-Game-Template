using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Player : Body
{
    public float dashSpeed;
    public float dashDuration = 0.1f;
    public float dashCooldown = 1f;
    private bool isDashing = false;
    private Vector3 dashDirection;
    private float lastDashTime = -Mathf.Infinity;
    public float shuffleSpeed;
    public float shuffleSpeedMultiplier = 1f;
    public float castSpeedMultiplier = 1f;

    // Keeping player-specific health bar for now
    private Canvas gameCanvas;
    public GameObject healthBar;
    private Image healthFill;

    protected override void Start()
    {
        base.Start();
        health = maxHealth;
        gameCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        InitializeHealthBar();
    }

    protected override void Update()
    {
        base.Update(); // Handles iFrames and health regen

        if (!isDashing)
        {
            Move();
            RotateTowardsMouse();
            if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
                && Time.time >= lastDashTime + dashCooldown)
            {
                StartCoroutine(Dash());
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            PlaySelectedCard();
        }

        UpdateHealthBar();
    }
    public override void SetStats()
    {
        base.SetStats();
        dashSpeed = moveSpeed * 3.5f;
    }

    private void InitializeHealthBar()
    {
        if (gameCanvas == null)
        {
            Debug.LogError("Main canvas not found! Make sure it's tagged as 'MainCanvas'");
            return;
        }

        healthBar = Instantiate(healthBar, gameCanvas.transform);
        RectTransform healthBarRect = healthBar.GetComponent<RectTransform>();
        healthFill = healthBar.GetComponentsInChildren<Image>()[1]; // Get the fill image

        // Position at bottom left with some padding
        float padding = 20f;
        healthBarRect.anchorMin = new Vector2(0, 0);
        healthBarRect.anchorMax = new Vector2(0, 0);
        healthBarRect.pivot = new Vector2(0, 0);
        healthBarRect.anchoredPosition = new Vector2(padding, padding);

        // Set initial size
        healthBarRect.sizeDelta = new Vector2(200, 20); // Width and height of health bar
    }

    private void UpdateHealthBar()
    {
        if (healthFill != null)
        {
            healthFill.fillAmount = Mathf.Clamp01(health / maxHealth);
        }
    }

    private void OnDestroy()
    {
        if (healthBar != null)
        {
            Destroy(healthBar);
        }
    }
    protected override void Die()
    {
        //Not sure how I want this to work yet, so I'll leave it blank
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, moveY, 0f).normalized;
        transform.position += move * moveSpeed * Time.deltaTime;

        if (move != Vector3.zero)
        {
            dashDirection = move;
        }
    }

    void RotateTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;
        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            transform.position += dashDirection * dashSpeed * Time.deltaTime;
            yield return null;
        }

        isDashing = false;
    }

    void PlaySelectedCard()
    {
        DeckManager.dm.PlayCard();
    }
}