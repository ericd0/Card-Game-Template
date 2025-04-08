using UnityEngine;

public class Slime : Body
{
    public float rotateSpeed = 2f;
    private Vector3 moveDirection;
    private GameObject player;
    private Rigidbody2D rb;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        moveDirection = Vector3.right;
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    protected override void Update()
    {
        base.Update();
        if (player != null)
        {
            Vector3 targetDirection = (player.transform.position - transform.position).normalized;
            moveDirection = Vector3.Lerp(moveDirection, targetDirection, rotateSpeed * Time.deltaTime);
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }
    protected override void Die()
    {
        // Add slime-specific death effects here
        base.Die();
    }
}