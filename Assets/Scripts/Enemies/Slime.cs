using UnityEngine;

public class Slime : Enemy
{
    public float moveSpeed = 3f;
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

    public override void OnTakeDamage(float damage)
    {
        base.OnTakeDamage(damage);
        // Add slime-specific damage reactions here
    }

    protected override void Die()
    {
        // Add slime-specific death effects here
        base.Die();
    }
}