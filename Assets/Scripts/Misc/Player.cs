using UnityEngine;
using System.Collections;

public class Player : GenericBody
{
    public float moveSpeed = 4f;
    public float dashSpeed = 15f;
    public float dashDuration = 0.1f;
    public float dashCooldown = 1f;
    public float maxHealth = 100f;
    public float health;
    private float iFrames = 0f;
    private float regenTimer = 0f;
    private float maxRegenTimer = 0.5f;
    public float regen = 0.8f;
    private bool isDashing = false;
    private Vector3 dashDirection;
    private float lastDashTime = -Mathf.Infinity;

    protected override void Start()
    {
        base.Start();
        teamId = 1; // Player team
        health = maxHealth;
    }

    void Update()
    {
<<<<<<< HEAD
<<<<<<< HEAD
        base.Update();
        HandleRegen();
=======
=======
>>>>>>> parent of 9527cb5 (Player Health Bar, Camera Follow, iFrame system.)
        iFrames -= Time.deltaTime;
        regenTimer -= Time.deltaTime;
        if (regenTimer <= 0f && health < maxHealth)
        {
            if (maxHealth - health < regen)
            {
                health = maxHealth;
            }
            else
            {
                health += regen;
            }
            regenTimer = maxRegenTimer;
        }
>>>>>>> parent of 9527cb5 (Player Health Bar, Camera Follow, iFrame system.)
        if (!isDashing)
        {
            Move();
            RotateTowardsMouse();
            HandleDashInput();
        }

        if (Input.GetMouseButtonDown(0))
        {
            PlaySelectedCard();
        }
<<<<<<< HEAD
<<<<<<< HEAD

        UpdateHealthBar();
    }

    private void HandleRegen()
    {
        regenTimer -= Time.deltaTime;
        if (regenTimer <= 0f && health < maxHealth)
        {
            health = Mathf.Min(maxHealth, health + regen);
            regenTimer = maxRegenTimer;
        }
    }

    private void HandleDashInput()
    {
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) 
            && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(Dash());
        }
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
=======
>>>>>>> parent of 9527cb5 (Player Health Bar, Camera Follow, iFrame system.)
=======
>>>>>>> parent of 9527cb5 (Player Health Bar, Camera Follow, iFrame system.)
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
        GameManager.gm.PlayCard();
    }
<<<<<<< HEAD
<<<<<<< HEAD

    public void TakeDamage(float damage)
=======
    void OnCollisionStay2D(Collision2D other)
>>>>>>> parent of 9527cb5 (Player Health Bar, Camera Follow, iFrame system.)
=======
    void OnCollisionStay2D(Collision2D other)
>>>>>>> parent of 9527cb5 (Player Health Bar, Camera Follow, iFrame system.)
    {
        if(other.gameObject.CompareTag("Enemy")&& iFrames <= 0f)
        {
            if (other.gameObject.GetComponent<Enemy>().collisionDamage==true)
            {
            health -= other.gameObject.GetComponent<Enemy>().damage;
            Debug.Log("Player hit! Health: " + health);
            iFrames = 1f;
            }
        }
    }
} 
    