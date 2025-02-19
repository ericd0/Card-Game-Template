using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float dashSpeed = 15f;
    public float dashDuration = 0.1f;
    public float dashCooldown = 1f;
    private bool isDashing = false;
    private Vector3 dashDirection;
    private float lastDashTime = -Mathf.Infinity;

    void Start()
    {
        
    }

    void Update()
    {
        if (!isDashing)
        {
            Move();
            RotateTowardsMouse();
            if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && Time.time >= lastDashTime + dashCooldown)
            {
                StartCoroutine(Dash());
            }
        }

        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            PlaySelectedCard();
        }
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
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
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
}